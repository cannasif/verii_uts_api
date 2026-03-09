namespace uts_api.Application.DTOs.Users;

public sealed class UserDetailDto
{
    public long Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public long RoleId { get; init; }
    public string RoleName { get; init; } = string.Empty;
    public IReadOnlyCollection<long> PermissionGroupIds { get; init; } = Array.Empty<long>();
}
