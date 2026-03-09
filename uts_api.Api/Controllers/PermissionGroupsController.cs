using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.PermissionGroups;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/permission-groups")]
public sealed class PermissionGroupsController : BaseApiController
{
    private readonly IPermissionGroupService _permissionGroupService;

    public PermissionGroupsController(IPermissionGroupService permissionGroupService)
    {
        _permissionGroupService = permissionGroupService;
    }

    [PermissionAuthorize(PermissionConstants.PermissionGroups.View)]
    [HttpGet]
    public async Task<ActionResult<PagedApiResponse<PermissionGroupListItemDto>>> GetAll([FromQuery] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _permissionGroupService.GetAllAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.PermissionGroups.View)]
    [HttpPost("search")]
    public async Task<ActionResult<PagedApiResponse<PermissionGroupListItemDto>>> Search([FromBody] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _permissionGroupService.GetAllAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.PermissionGroups.View)]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<PermissionGroupDetailDto>>> GetById(long id, CancellationToken cancellationToken)
    {
        return OkResponse(await _permissionGroupService.GetByIdAsync(id, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.PermissionGroups.Create)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PermissionGroupDetailDto>>> Create([FromBody] CreatePermissionGroupRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _permissionGroupService.CreateAsync(request, cancellationToken);
        return CreatedResponse(nameof(GetById), new { id = response.Id }, response, LocalizationKeys.Created);
    }

    [PermissionAuthorize(PermissionConstants.PermissionGroups.Update)]
    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<PermissionGroupDetailDto>>> Update(long id, [FromBody] UpdatePermissionGroupRequestDto request, CancellationToken cancellationToken)
    {
        return OkResponse(await _permissionGroupService.UpdateAsync(id, request, cancellationToken), LocalizationKeys.Updated);
    }

    [PermissionAuthorize(PermissionConstants.PermissionGroups.ManagePermissions)]
    [HttpPut("{id:long}/permissions")]
    public async Task<ActionResult<ApiResponse<PermissionGroupDetailDto>>> UpdatePermissions(long id, [FromBody] UpdatePermissionGroupPermissionsRequestDto request, CancellationToken cancellationToken)
    {
        return OkResponse(await _permissionGroupService.UpdatePermissionsAsync(id, request, cancellationToken), LocalizationKeys.Updated);
    }

    [PermissionAuthorize(PermissionConstants.PermissionGroups.Update)]
    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ApiResponse<object?>>> Delete(long id, CancellationToken cancellationToken)
    {
        await _permissionGroupService.DeleteAsync(id, cancellationToken);
        return OkResponse<object?>(null, LocalizationKeys.Deleted);
    }
}
