using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using uts_api.Application.DTOs.Roles;
using uts_api.Application.Interfaces;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class RoleService : IRoleService
{
    private static readonly IReadOnlyDictionary<string, string> AllowedColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["id"] = "Id",
        ["name"] = "Name",
        ["description"] = "Description",
        ["createdAtUtc"] = "CreatedAtUtc"
    };

    private readonly IApplicationDbContext _dbContext;

    public RoleService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<RoleDto>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Roles
            .AsNoTracking()
            .ApplySearch(request.Search, "Name", "Description")
            .ApplyFilters(request.Filters, AllowedColumns, request.FilterLogic)
            .ApplySorting(request.SortBy, request.SortDirection, AllowedColumns)
            .Select(x => new RoleDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            });

        return await query.ToPagedResultAsync(request, cancellationToken);
    }
}
