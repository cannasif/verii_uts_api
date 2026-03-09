using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Exceptions;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UserPermissionGroups;
using uts_api.Application.Interfaces;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class UserPermissionGroupService : IUserPermissionGroupService
{
    private static readonly IReadOnlyDictionary<string, string> AllowedColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["id"] = "Id",
        ["name"] = "Name",
        ["description"] = "Description",
        ["isSystem"] = "IsSystem",
        ["createdAtUtc"] = "CreatedAtUtc"
    };

    private readonly IApplicationDbContext _dbContext;

    public UserPermissionGroupService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<UserPermissionGroupDto>> GetByUserIdAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        await EnsureUserExists(userId, cancellationToken);

        var assignedGroupIds = await _dbContext.UserPermissionGroups
            .Where(x => x.UserId == userId)
            .Select(x => x.PermissionGroupId)
            .ToListAsync(cancellationToken);

        var query = _dbContext.PermissionGroups
            .AsNoTracking()
            .ApplySearch(request.Search, "Name", "Description")
            .ApplyFilters(request.Filters, AllowedColumns, request.FilterLogic)
            .ApplySorting(request.SortBy, request.SortDirection, AllowedColumns)
            .Select(x => new UserPermissionGroupDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsAssigned = assignedGroupIds.Contains(x.Id)
            });

        return await query.ToPagedResultAsync(request, cancellationToken);
    }

    public async Task UpdateAsync(long userId, UpdateUserPermissionGroupsRequestDto request, CancellationToken cancellationToken = default)
    {
        await EnsureUserExists(userId, cancellationToken);

        var groupIds = request.PermissionGroupIds.Distinct().ToList();
        var existingCount = await _dbContext.PermissionGroups.CountAsync(x => groupIds.Contains(x.Id), cancellationToken);
        if (existingCount != groupIds.Count)
        {
            throw new AppException(LocalizationKeys.PermissionGroupsNotFound, 404);
        }

        var existingRelations = await _dbContext.UserPermissionGroups.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
        _dbContext.UserPermissionGroups.RemoveRange(existingRelations);
        _dbContext.UserPermissionGroups.AddRange(groupIds.Select(groupId => new Domain.Entities.UserPermissionGroup
        {
            UserId = userId,
            PermissionGroupId = groupId
        }));

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureUserExists(long userId, CancellationToken cancellationToken)
    {
        if (!await _dbContext.Users.AnyAsync(x => x.Id == userId, cancellationToken))
        {
            throw new AppException(LocalizationKeys.UserNotFound, 404);
        }
    }
}
