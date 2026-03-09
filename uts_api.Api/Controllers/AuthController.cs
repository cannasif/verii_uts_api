using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.Auth;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/auth")]
public sealed class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        return OkResponse(await _authService.LoginAsync(request, cancellationToken), LocalizationKeys.LoginSuccessful);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterRequestDto request, CancellationToken cancellationToken)
    {
        return OkResponse(await _authService.RegisterAsync(request, cancellationToken), LocalizationKeys.RegisterSuccessful);
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<ActionResult<ApiResponse<ForgotPasswordResponseDto>>> ForgotPassword([FromBody] ForgotPasswordRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _authService.ForgotPasswordAsync(request, cancellationToken);
        var localized = new ForgotPasswordResponseDto
        {
            Message = Localizer[response.Message],
            EmailSent = response.EmailSent,
            ResetTokenPreview = response.ResetTokenPreview
        };

        return OkResponse(localized, LocalizationKeys.Success);
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordRequestDto request, CancellationToken cancellationToken)
    {
        await _authService.ResetPasswordAsync(request, cancellationToken);
        return OkMessage(LocalizationKeys.PasswordResetSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.Auth.Me)]
    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<CurrentUserDto>>> Me(CancellationToken cancellationToken)
    {
        return OkResponse(await _authService.GetCurrentUserAsync(cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.Auth.MyPermissions)]
    [HttpGet("my-permissions")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<string>>>> MyPermissions(CancellationToken cancellationToken)
    {
        return OkResponse(await _authService.GetMyPermissionsAsync(cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResponse>> ChangePassword([FromBody] ChangePasswordRequestDto request, CancellationToken cancellationToken)
    {
        await _authService.ChangePasswordAsync(request, cancellationToken);
        return OkMessage(LocalizationKeys.PasswordChanged);
    }
}
