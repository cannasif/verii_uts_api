using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Exceptions;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Localization;
using uts_api.Application.DTOs.UserProfiles;
using uts_api.Application.Interfaces;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Services;

public sealed class UserProfileService : IUserProfileService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly string _uploadRootPath;

    public UserProfileService(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _uploadRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "user-profiles");
    }

    public async Task<UserProfileDto> GetMyProfileAsync(CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId ?? throw new AppException(LocalizationKeys.Unauthorized, 401);

        var user = await _dbContext.Users
            .Include(x => x.Role)
            .Include(x => x.Profile)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
            ?? throw new AppException(LocalizationKeys.UserNotFound, 404);

        return Map(user);
    }

    public async Task<UserProfileDto> UpdateMyProfileAsync(UpdateMyProfileRequestDto request, CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId ?? throw new AppException(LocalizationKeys.Unauthorized, 401);

        var user = await _dbContext.Users
            .Include(x => x.Role)
            .Include(x => x.Profile)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
            ?? throw new AppException(LocalizationKeys.UserNotFound, 404);

        var profile = user.Profile;
        if (profile is null)
        {
            profile = new UserProfile
            {
                UserId = user.Id
            };

            _dbContext.UserProfiles.Add(profile);
            user.Profile = profile;
        }

        profile.ProfilePictureUrl = Normalize(request.ProfilePictureUrl);
        profile.PhoneNumber = Normalize(request.PhoneNumber);
        profile.JobTitle = Normalize(request.JobTitle);
        profile.Department = Normalize(request.Department);
        profile.Bio = Normalize(request.Bio);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Map(user);
    }

    public async Task<UserProfileDto> UploadMyProfilePictureAsync(Stream fileStream, string fileExtension, CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId ?? throw new AppException(LocalizationKeys.Unauthorized, 401);

        var user = await _dbContext.Users
            .Include(x => x.Role)
            .Include(x => x.Profile)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
            ?? throw new AppException(LocalizationKeys.UserNotFound, 404);

        var profile = user.Profile;
        if (profile is null)
        {
            profile = new UserProfile
            {
                UserId = user.Id
            };

            _dbContext.UserProfiles.Add(profile);
            user.Profile = profile;
        }

        Directory.CreateDirectory(_uploadRootPath);

        DeleteExistingProfilePicture(profile.ProfilePictureUrl);

        var safeExtension = string.IsNullOrWhiteSpace(fileExtension) ? ".bin" : fileExtension.Trim().ToLowerInvariant();
        var fileName = $"{user.Id}_{Guid.NewGuid():N}{safeExtension}";
        var filePath = Path.Combine(_uploadRootPath, fileName);

        await using (var targetStream = File.Create(filePath))
        {
            await fileStream.CopyToAsync(targetStream, cancellationToken);
        }

        profile.ProfilePictureUrl = $"/uploads/user-profiles/{fileName}";

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Map(user);
    }

    private void DeleteExistingProfilePicture(string? profilePictureUrl)
    {
        if (string.IsNullOrWhiteSpace(profilePictureUrl))
        {
            return;
        }

        var normalizedPath = profilePictureUrl.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);
        var existingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", normalizedPath);

        if (File.Exists(existingFilePath))
        {
            File.Delete(existingFilePath);
        }
    }

    private static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }

    private static UserProfileDto Map(User user)
    {
        return new UserProfileDto
        {
            Id = user.Profile?.Id ?? 0,
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role.Name,
            ProfilePictureUrl = user.Profile?.ProfilePictureUrl,
            PhoneNumber = user.Profile?.PhoneNumber,
            JobTitle = user.Profile?.JobTitle,
            Department = user.Profile?.Department,
            Bio = user.Profile?.Bio
        };
    }
}
