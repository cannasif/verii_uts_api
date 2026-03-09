using uts_api.Domain.Common;

namespace uts_api.Domain.Entities;

public sealed class PermissionDefinition : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;

    public ICollection<PermissionGroupPermissionDefinition> PermissionGroupPermissionDefinitions { get; set; } = new List<PermissionGroupPermissionDefinition>();
}
