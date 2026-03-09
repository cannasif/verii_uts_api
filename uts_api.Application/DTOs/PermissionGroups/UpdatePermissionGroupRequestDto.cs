namespace uts_api.Application.DTOs.PermissionGroups;

public sealed class UpdatePermissionGroupRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
