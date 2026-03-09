using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.Roles;

namespace uts_api.Application.Interfaces;

public interface IRoleService
{
    Task<PagedResult<RoleDto>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
}
