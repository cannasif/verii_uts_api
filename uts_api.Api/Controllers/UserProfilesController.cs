using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UserProfiles;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Authorize]
[Route("api/user-profiles")]
public sealed class UserProfilesController : BaseApiController
{
    private readonly IUserProfileService _userProfileService;

    public UserProfilesController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<UserProfileDto>>> Me(CancellationToken cancellationToken)
    {
        return OkResponse(await _userProfileService.GetMyProfileAsync(cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [HttpPut("me")]
    public async Task<ActionResult<ApiResponse<UserProfileDto>>> UpdateMe([FromBody] UpdateMyProfileRequestDto request, CancellationToken cancellationToken)
    {
        return OkResponse(await _userProfileService.UpdateMyProfileAsync(request, cancellationToken), LocalizationKeys.Updated);
    }

    [HttpPost("me/profile-picture")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<ActionResult<ApiResponse<UserProfileDto>>> UploadMyProfilePicture(IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(ApiResponse.Fail(Localizer[LocalizationKeys.InvalidRequest]));
        }

        await using var stream = file.OpenReadStream();
        var response = await _userProfileService.UploadMyProfilePictureAsync(stream, Path.GetExtension(file.FileName), cancellationToken);
        return OkResponse(response, LocalizationKeys.Updated);
    }
}
