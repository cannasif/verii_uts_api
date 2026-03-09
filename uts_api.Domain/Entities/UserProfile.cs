using uts_api.Domain.Common;

namespace uts_api.Domain.Entities;

public sealed class UserProfile : BaseEntity
{
    public long UserId { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public string? JobTitle { get; set; }
    public string? Department { get; set; }
    public string? Bio { get; set; }

    public User User { get; set; } = null!;
}
