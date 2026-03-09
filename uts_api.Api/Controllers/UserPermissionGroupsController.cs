using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.UserPermissionGroups;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/user-permission-groups")]
public sealed class UserPermissionGroupsController : BaseApiController
{
    private readonly IUserPermissionGroupService _userPermissionGroupService;

    public UserPermissionGroupsController(IUserPermissionGroupService userPermissionGroupService)
    {
        _userPermissionGroupService = userPermissionGroupService;
    }

    [PermissionAuthorize(PermissionConstants.UserPermissionGroups.View)]
    [HttpGet("users/{userId:long}")]
    public async Task<ActionResult<PagedApiResponse<UserPermissionGroupDto>>> GetByUserId(long userId, [FromQuery] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _userPermissionGroupService.GetByUserIdAsync(userId, request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.UserPermissionGroups.View)]
    [HttpPost("users/{userId:long}/search")]
    public async Task<ActionResult<PagedApiResponse<UserPermissionGroupDto>>> SearchByUserId(long userId, [FromBody] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _userPermissionGroupService.GetByUserIdAsync(userId, request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.UserPermissionGroups.Update)]
    [HttpPut("users/{userId:long}")]
    public async Task<ActionResult<ApiResponse>> Update(long userId, [FromBody] UpdateUserPermissionGroupsRequestDto request, CancellationToken cancellationToken)
    {
        await _userPermissionGroupService.UpdateAsync(userId, request, cancellationToken);
        return OkMessage(LocalizationKeys.Updated);
    }
}
