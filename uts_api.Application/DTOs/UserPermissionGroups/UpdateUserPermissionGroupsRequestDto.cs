namespace uts_api.Application.DTOs.UserPermissionGroups;

public sealed class UpdateUserPermissionGroupsRequestDto
{
    public List<long> PermissionGroupIds { get; set; } = new();
}
