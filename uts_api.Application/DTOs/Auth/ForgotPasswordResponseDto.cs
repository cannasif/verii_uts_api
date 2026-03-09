namespace uts_api.Application.DTOs.Auth;

public sealed class ForgotPasswordResponseDto
{
    public string Message { get; init; } = string.Empty;
    public bool EmailSent { get; init; }
    public string? ResetTokenPreview { get; init; }
}
