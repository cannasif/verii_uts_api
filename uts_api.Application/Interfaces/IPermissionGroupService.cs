using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.PermissionGroups;

namespace uts_api.Application.Interfaces;

public interface IPermissionGroupService
{
    Task<PagedResult<PermissionGroupListItemDto>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<PermissionGroupDetailDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PermissionGroupDetailDto> CreateAsync(CreatePermissionGroupRequestDto request, CancellationToken cancellationToken = default);
    Task<PermissionGroupDetailDto> UpdateAsync(long id, UpdatePermissionGroupRequestDto request, CancellationToken cancellationToken = default);
    Task<PermissionGroupDetailDto> UpdatePermissionsAsync(long id, UpdatePermissionGroupPermissionsRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
