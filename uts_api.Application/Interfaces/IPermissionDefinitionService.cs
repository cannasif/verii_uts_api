using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.PermissionDefinitions;

namespace uts_api.Application.Interfaces;

public interface IPermissionDefinitionService
{
    Task<PagedResult<PermissionDefinitionDto>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
}
