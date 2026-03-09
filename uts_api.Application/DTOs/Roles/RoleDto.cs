namespace uts_api.Application.DTOs.Roles;

public sealed class RoleDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
