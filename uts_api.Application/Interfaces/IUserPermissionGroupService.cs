using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UserPermissionGroups;

namespace uts_api.Application.Interfaces;

public interface IUserPermissionGroupService
{
    Task<PagedResult<UserPermissionGroupDto>> GetByUserIdAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(long userId, UpdateUserPermissionGroupsRequestDto request, CancellationToken cancellationToken = default);
}
