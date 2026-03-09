namespace uts_api.Domain.Common;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public string? CreateUser { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public string? UpdateUser { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public string? DeleteUser { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
}
