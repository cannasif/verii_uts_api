using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.PermissionDefinitions;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/permission-definitions")]
public sealed class PermissionDefinitionsController : BaseApiController
{
    private readonly IPermissionDefinitionService _permissionDefinitionService;

    public PermissionDefinitionsController(IPermissionDefinitionService permissionDefinitionService)
    {
        _permissionDefinitionService = permissionDefinitionService;
    }

    [PermissionAuthorize(PermissionConstants.PermissionDefinitions.View)]
    [HttpGet]
    public async Task<ActionResult<PagedApiResponse<PermissionDefinitionDto>>> GetAll([FromQuery] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _permissionDefinitionService.GetAllAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.PermissionDefinitions.View)]
    [HttpPost("search")]
    public async Task<ActionResult<PagedApiResponse<PermissionDefinitionDto>>> Search([FromBody] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _permissionDefinitionService.GetAllAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }
}
