using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using uts_api.Application.DTOs.PermissionDefinitions;
using uts_api.Application.Interfaces;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class PermissionDefinitionService : IPermissionDefinitionService
{
    private static readonly IReadOnlyDictionary<string, string> AllowedColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["id"] = "Id",
        ["module"] = "Module",
        ["name"] = "Name",
        ["code"] = "Code",
        ["description"] = "Description",
        ["createdAtUtc"] = "CreatedAtUtc"
    };

    private readonly IApplicationDbContext _dbContext;

    public PermissionDefinitionService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<PermissionDefinitionDto>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.PermissionDefinitions
            .AsNoTracking()
            .ApplySearch(request.Search, "Module", "Name", "Code", "Description")
            .ApplyFilters(request.Filters, AllowedColumns, request.FilterLogic)
            .ApplySorting(request.SortBy, request.SortDirection, AllowedColumns)
            .Select(x => new PermissionDefinitionDto
            {
                Id = x.Id,
                Module = x.Module,
                Name = x.Name,
                Code = x.Code,
                Description = x.Description
            });

        return await query.ToPagedResultAsync(request, cancellationToken);
    }
}
