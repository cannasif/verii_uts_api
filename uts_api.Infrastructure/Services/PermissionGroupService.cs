using AutoMapper;
using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Exceptions;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.PermissionGroups;
using uts_api.Application.Interfaces;
using uts_api.Domain.Entities;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class PermissionGroupService : IPermissionGroupService
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
    private readonly IMapper _mapper;

    public PermissionGroupService(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PagedResult<PermissionGroupListItemDto>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.PermissionGroups
            .AsNoTracking()
            .ApplySearch(request.Search, "Name", "Description")
            .ApplyFilters(request.Filters, AllowedColumns, request.FilterLogic)
            .ApplySorting(request.SortBy, request.SortDirection, AllowedColumns)
            .Select(x => new PermissionGroupListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsSystem = x.IsSystem,
                PermissionCount = x.PermissionGroupPermissionDefinitions.Count,
                AssignedUserCount = x.UserPermissionGroups.Count
            });

        return await query.ToPagedResultAsync(request, cancellationToken);
    }

    public async Task<PermissionGroupDetailDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.PermissionGroups
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new PermissionGroupDetailDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsSystem = x.IsSystem,
                PermissionDefinitionIds = x.PermissionGroupPermissionDefinitions.Select(y => y.PermissionDefinitionId).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new AppException(LocalizationKeys.PermissionGroupNotFound, 404);
    }

    public async Task<PermissionGroupDetailDto> CreateAsync(CreatePermissionGroupRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedName = request.Name.Trim().ToUpperInvariant();
        await EnsureNameAvailable(normalizedName, null, cancellationToken);

        var permissionGroup = _mapper.Map<PermissionGroup>(request);
        permissionGroup.NormalizedName = normalizedName;

        _dbContext.PermissionGroups.Add(permissionGroup);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(permissionGroup.Id, cancellationToken);
    }

    public async Task<PermissionGroupDetailDto> UpdateAsync(long id, UpdatePermissionGroupRequestDto request, CancellationToken cancellationToken = default)
    {
        var permissionGroup = await _dbContext.PermissionGroups.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new AppException(LocalizationKeys.PermissionGroupNotFound, 404);

        var normalizedName = request.Name.Trim().ToUpperInvariant();
        await EnsureNameAvailable(normalizedName, id, cancellationToken);

        permissionGroup.Name = request.Name.Trim();
        permissionGroup.NormalizedName = normalizedName;
        permissionGroup.Description = request.Description.Trim();

        await _dbContext.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<PermissionGroupDetailDto> UpdatePermissionsAsync(long id, UpdatePermissionGroupPermissionsRequestDto request, CancellationToken cancellationToken = default)
    {
        var permissionGroup = await _dbContext.PermissionGroups
            .Include(x => x.PermissionGroupPermissionDefinitions)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new AppException(LocalizationKeys.PermissionGroupNotFound, 404);

        var definitionIds = request.PermissionDefinitionIds.Distinct().ToList();
        var existingCount = await _dbContext.PermissionDefinitions.CountAsync(x => definitionIds.Contains(x.Id), cancellationToken);
        if (existingCount != definitionIds.Count)
        {
            throw new AppException(LocalizationKeys.PermissionDefinitionsNotFound, 404);
        }

        _dbContext.PermissionGroupPermissionDefinitions.RemoveRange(permissionGroup.PermissionGroupPermissionDefinitions);
        _dbContext.PermissionGroupPermissionDefinitions.AddRange(definitionIds.Select(definitionId => new Domain.Entities.PermissionGroupPermissionDefinition
        {
            PermissionGroupId = id,
            PermissionDefinitionId = definitionId
        }));

        await _dbContext.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var permissionGroup = await _dbContext.PermissionGroups
            .Include(x => x.UserPermissionGroups)
            .Include(x => x.PermissionGroupPermissionDefinitions)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new AppException(LocalizationKeys.PermissionGroupNotFound, 404);

        if (permissionGroup.IsSystem)
        {
            throw new AppException(LocalizationKeys.SystemPermissionGroupCannotBeDeleted, 400);
        }

        if (permissionGroup.UserPermissionGroups.Any(x => !x.IsDeleted))
        {
            throw new AppException(LocalizationKeys.PermissionGroupInUse, 400);
        }

        _dbContext.PermissionGroupPermissionDefinitions.RemoveRange(permissionGroup.PermissionGroupPermissionDefinitions);
        _dbContext.PermissionGroups.Remove(permissionGroup);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureNameAvailable(string normalizedName, long? currentId, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.PermissionGroups
            .AnyAsync(x => x.NormalizedName == normalizedName && (!currentId.HasValue || x.Id != currentId.Value), cancellationToken);

        if (exists)
        {
            throw new AppException(LocalizationKeys.PermissionGroupNameAlreadyInUse);
        }
    }
}
