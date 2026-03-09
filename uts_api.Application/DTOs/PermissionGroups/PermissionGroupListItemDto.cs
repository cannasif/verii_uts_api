namespace uts_api.Application.DTOs.PermissionGroups;

public sealed class PermissionGroupListItemDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool IsSystem { get; init; }
    public int PermissionCount { get; init; }
    public int AssignedUserCount { get; init; }
}
