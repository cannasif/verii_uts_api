namespace uts_api.Application.DTOs.Users;

public sealed class CreateUserRequestDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public long RoleId { get; set; }
    public List<long> PermissionGroupIds { get; set; } = new();
}
