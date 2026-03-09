namespace uts_api.Application.DTOs.Auth;

public sealed class ResetPasswordRequestDto
{
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
