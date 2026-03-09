namespace uts_api.Application.DTOs.PermissionGroups;

public sealed class UpdatePermissionGroupPermissionsRequestDto
{
    public List<long> PermissionDefinitionIds { get; set; } = new();
}
