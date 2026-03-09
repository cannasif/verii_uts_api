using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.Users;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/users")]
public sealed class UsersController : BaseApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [PermissionAuthorize(PermissionConstants.Users.View)]
    [HttpGet]
    public async Task<ActionResult<PagedApiResponse<UserListItemDto>>> GetAll([FromQuery] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _userService.GetPagedAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.Users.View)]
    [HttpPost("search")]
    public async Task<ActionResult<PagedApiResponse<UserListItemDto>>> Search([FromBody] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _userService.GetPagedAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.Users.View)]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<UserDetailDto>>> GetById(long id, CancellationToken cancellationToken)
    {
        return OkResponse(await _userService.GetByIdAsync(id, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.Users.Create)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserDetailDto>>> Create([FromBody] CreateUserRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _userService.CreateAsync(request, cancellationToken);
        return CreatedResponse(nameof(GetById), new { id = response.Id }, response, LocalizationKeys.Created);
    }

    [PermissionAuthorize(PermissionConstants.Users.Update)]
    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<UserDetailDto>>> Update(long id, [FromBody] UpdateUserRequestDto request, CancellationToken cancellationToken)
    {
        return OkResponse(await _userService.UpdateAsync(id, request, cancellationToken), LocalizationKeys.Updated);
    }

    [PermissionAuthorize(PermissionConstants.Users.Delete)]
    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ApiResponse>> Delete(long id, CancellationToken cancellationToken)
    {
        await _userService.DeleteAsync(id, cancellationToken);
        return OkMessage(LocalizationKeys.Deleted);
    }
}
