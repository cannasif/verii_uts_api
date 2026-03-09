using System.Globalization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Models;

namespace uts_api.Infrastructure.Persistence;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> ApplySearch<TEntity>(
        this IQueryable<TEntity> query,
        string? search,
        params string[] searchableProperties)
    {
        if (string.IsNullOrWhiteSpace(search) || searchableProperties.Length == 0)
        {
            return query;
        }

        var parameter = Expression.Parameter(typeof(TEntity), "x");
        Expression? combined = null;
        var searchConstant = Expression.Constant(search.Trim().ToLowerInvariant());

        foreach (var propertyPath in searchableProperties)
        {
            var property = BuildPropertyExpression(parameter, propertyPath);
            if (property.Type != typeof(string))
            {
                continue;
            }

            var notNull = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
            var toLower = Expression.Call(property, typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!);
            var contains = Expression.Call(toLower, typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!, searchConstant);
            var current = Expression.AndAlso(notNull, contains);
            combined = combined is null ? current : Expression.OrElse(combined, current);
        }

        return combined is null ? query : query.Where(Expression.Lambda<Func<TEntity, bool>>(combined, parameter));
    }

    public static IQueryable<TEntity> ApplyFilters<TEntity>(
        this IQueryable<TEntity> query,
        IReadOnlyCollection<FilterRule> filters,
        IReadOnlyDictionary<string, string> allowedColumns,
        string filterLogic = "and")
    {
        if (filters.Count == 0)
        {
            return query;
        }

        var parameter = Expression.Parameter(typeof(TEntity), "x");
        Expression? combined = null;
        var useOr = string.Equals(filterLogic, "or", StringComparison.OrdinalIgnoreCase);

        foreach (var filter in filters)
        {
            if (!allowedColumns.TryGetValue(filter.Column, out var propertyPath))
            {
                continue;
            }

            var property = BuildPropertyExpression(parameter, propertyPath);
            var filterExpression = BuildFilterExpression(property, filter);
            if (filterExpression is null)
            {
                continue;
            }

            combined = combined is null
                ? filterExpression
                : useOr
                    ? Expression.OrElse(combined, filterExpression)
                    : Expression.AndAlso(combined, filterExpression);
        }

        return combined is null ? query : query.Where(Expression.Lambda<Func<TEntity, bool>>(combined, parameter));
    }

    public static IQueryable<TEntity> ApplySorting<TEntity>(
        this IQueryable<TEntity> query,
        string? sortBy,
        string? sortDirection,
        IReadOnlyDictionary<string, string> allowedColumns)
    {
        if (string.IsNullOrWhiteSpace(sortBy) || !allowedColumns.TryGetValue(sortBy, out var propertyPath))
        {
            return query;
        }

        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var property = BuildPropertyExpression(parameter, propertyPath);
        var lambda = Expression.Lambda(property, parameter);
        var methodName = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) ? "OrderBy" : "OrderByDescending";

        var method = typeof(Queryable).GetMethods()
            .Single(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(TEntity), property.Type);

        return (IQueryable<TEntity>)method.Invoke(null, [query, lambda])!;
    }

    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PagedRequest request,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = request.PageSize > 0 ? (int)Math.Ceiling(totalCount / (double)request.PageSize) : 0
        };
    }

    private static Expression? BuildFilterExpression(Expression property, FilterRule filter)
    {
        var operatorName = filter.Operator.Trim().ToLowerInvariant();
        var propertyType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;

        if (propertyType == typeof(string))
        {
            return BuildStringFilter(property, operatorName, filter.Value);
        }

        if (!TryConvertValue(filter.Value, propertyType, out var typedValue))
        {
            return null;
        }

        var constant = Expression.Constant(typedValue, propertyType);
        Expression right = property.Type == propertyType ? constant : Expression.Convert(constant, property.Type);

        return operatorName switch
        {
            "equals" or "eq" => Expression.Equal(property, right),
            "notequals" or "neq" => Expression.NotEqual(property, right),
            "gt" => Expression.GreaterThan(property, right),
            "gte" => Expression.GreaterThanOrEqual(property, right),
            "lt" => Expression.LessThan(property, right),
            "lte" => Expression.LessThanOrEqual(property, right),
            _ => null
        };
    }

    private static Expression? BuildStringFilter(Expression property, string operatorName, string value)
    {
        var notNull = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));
        var lowered = Expression.Call(property, typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!);
        var constant = Expression.Constant(value.ToLowerInvariant());

        Expression comparison = operatorName switch
        {
            "contains" => Expression.Call(lowered, typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!, constant),
            "startswith" => Expression.Call(lowered, typeof(string).GetMethod(nameof(string.StartsWith), [typeof(string)])!, constant),
            "endswith" => Expression.Call(lowered, typeof(string).GetMethod(nameof(string.EndsWith), [typeof(string)])!, constant),
            "notequals" or "neq" => Expression.NotEqual(lowered, constant),
            _ => Expression.Equal(lowered, constant)
        };

        return Expression.AndAlso(notNull, comparison);
    }

    private static bool TryConvertValue(string rawValue, Type targetType, out object? converted)
    {
        try
        {
            if (targetType.IsEnum)
            {
                converted = Enum.Parse(targetType, rawValue, true);
                return true;
            }

            if (targetType == typeof(DateTime))
            {
                converted = DateTime.Parse(rawValue, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                return true;
            }

            if (targetType == typeof(bool))
            {
                converted = bool.Parse(rawValue);
                return true;
            }

            converted = Convert.ChangeType(rawValue, targetType, CultureInfo.InvariantCulture);
            return true;
        }
        catch
        {
            converted = null;
            return false;
        }
    }

    private static Expression BuildPropertyExpression(Expression parameter, string propertyPath)
    {
        Expression current = parameter;
        foreach (var part in propertyPath.Split('.', StringSplitOptions.RemoveEmptyEntries))
        {
            current = Expression.PropertyOrField(current, part);
        }

        return current;
    }
}
