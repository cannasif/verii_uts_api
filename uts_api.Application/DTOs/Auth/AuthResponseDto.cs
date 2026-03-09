namespace uts_api.Application.DTOs.Auth;

public sealed class AuthResponseDto
{
    public long UserId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Permissions { get; init; } = Array.Empty<string>();
    public AuthTokenDto Token { get; init; } = new();
}
