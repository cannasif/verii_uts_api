using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.Roles;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/roles")]
public sealed class RolesController : BaseApiController
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [PermissionAuthorize(PermissionConstants.Roles.View)]
    [HttpGet]
    public async Task<ActionResult<PagedApiResponse<RoleDto>>> GetAll([FromQuery] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _roleService.GetAllAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.Roles.View)]
    [HttpPost("search")]
    public async Task<ActionResult<PagedApiResponse<RoleDto>>> Search([FromBody] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _roleService.GetAllAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }
}
