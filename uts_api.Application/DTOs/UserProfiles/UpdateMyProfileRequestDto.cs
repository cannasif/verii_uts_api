namespace uts_api.Application.DTOs.UserProfiles;

public sealed class UpdateMyProfileRequestDto
{
    public string? ProfilePictureUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public string? JobTitle { get; set; }
    public string? Department { get; set; }
    public string? Bio { get; set; }
}
