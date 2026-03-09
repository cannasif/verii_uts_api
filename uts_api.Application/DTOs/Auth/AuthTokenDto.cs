namespace uts_api.Application.DTOs.Auth;

public sealed class AuthTokenDto
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime ExpiresAtUtc { get; init; }
}
