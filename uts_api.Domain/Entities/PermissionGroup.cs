using uts_api.Domain.Common;

namespace uts_api.Domain.Entities;

public sealed class PermissionGroup : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsSystem { get; set; }

    public ICollection<PermissionGroupPermissionDefinition> PermissionGroupPermissionDefinitions { get; set; } = new List<PermissionGroupPermissionDefinition>();
    public ICollection<UserPermissionGroup> UserPermissionGroups { get; set; } = new List<UserPermissionGroup>();
}
