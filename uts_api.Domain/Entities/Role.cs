using uts_api.Domain.Common;

namespace uts_api.Domain.Entities;

public sealed class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsSystem { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}
