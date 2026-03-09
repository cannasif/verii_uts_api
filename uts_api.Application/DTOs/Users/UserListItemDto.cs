namespace uts_api.Application.DTOs.Users;

public sealed class UserListItemDto
{
    public long Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
    public DateTime CreatedAtUtc { get; init; }
}
