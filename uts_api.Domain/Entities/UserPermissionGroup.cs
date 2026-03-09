using uts_api.Domain.Common;

namespace uts_api.Domain.Entities;

public sealed class UserPermissionGroup : BaseEntity
{
    public long UserId { get; set; }
    public long PermissionGroupId { get; set; }

    public User User { get; set; } = null!;
    public PermissionGroup PermissionGroup { get; set; } = null!;
}
