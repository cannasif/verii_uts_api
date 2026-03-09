namespace uts_api.Application.DTOs.UserPermissionGroups;

public sealed class UserPermissionGroupDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool IsAssigned { get; init; }
}
