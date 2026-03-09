namespace uts_api.Application.DTOs.PermissionDefinitions;

public sealed class PermissionDefinitionDto
{
    public long Id { get; init; }
    public string Module { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
