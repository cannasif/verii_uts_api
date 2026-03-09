namespace uts_api.Application.DTOs.PermissionGroups;

public sealed class PermissionGroupDetailDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool IsSystem { get; init; }
    public IReadOnlyCollection<long> PermissionDefinitionIds { get; init; } = Array.Empty<long>();
}
