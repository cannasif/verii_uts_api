using uts_api.Application.DTOs.UserProfiles;

namespace uts_api.Application.Interfaces;

public interface IUserProfileService
{
    Task<UserProfileDto> GetMyProfileAsync(CancellationToken cancellationToken = default);
    Task<UserProfileDto> UpdateMyProfileAsync(UpdateMyProfileRequestDto request, CancellationToken cancellationToken = default);
    Task<UserProfileDto> UploadMyProfilePictureAsync(Stream fileStream, string fileExtension, CancellationToken cancellationToken = default);
}
