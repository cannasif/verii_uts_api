using uts_api.Domain.Common;

namespace uts_api.Domain.Entities;

public sealed class PermissionGroupPermissionDefinition : BaseEntity
{
    public long PermissionGroupId { get; set; }
    public long PermissionDefinitionId { get; set; }

    public PermissionGroup PermissionGroup { get; set; } = null!;
    public PermissionDefinition PermissionDefinition { get; set; } = null!;
}
