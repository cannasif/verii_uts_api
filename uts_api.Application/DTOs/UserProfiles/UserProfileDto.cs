namespace uts_api.Application.DTOs.UserProfiles;

public sealed class UserProfileDto
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string? ProfilePictureUrl { get; init; }
    public string? PhoneNumber { get; init; }
    public string? JobTitle { get; init; }
    public string? Department { get; init; }
    public string? Bio { get; init; }
}
