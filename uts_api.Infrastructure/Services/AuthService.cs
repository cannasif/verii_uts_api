using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Exceptions;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Localization;
using uts_api.Application.DTOs.Auth;
using uts_api.Domain.Entities;
using uts_api.Application.Interfaces;
using uts_api.Infrastructure.Persistence;
using uts_api.Infrastructure.Persistence.Seed;

namespace uts_api.Infrastructure.Services;

public sealed class AuthService : IAuthService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IEmailSenderService _emailSenderService;

    public AuthService(
        IApplicationDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        ICurrentUserService currentUserService,
        IEmailSenderService emailSenderService)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _currentUserService = currentUserService;
        _emailSenderService = emailSenderService;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();
        var user = await _dbContext.Users
            .Include(x => x.Role)
            .Include(x => x.UserPermissionGroups)
                .ThenInclude(x => x.PermissionGroup)
                    .ThenInclude(x => x.PermissionGroupPermissionDefinitions)
                        .ThenInclude(x => x.PermissionDefinition)
            .FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new AppException(LocalizationKeys.InvalidCredentials, 401);
        }

        var permissions = GetPermissions(user);
        var token = _jwtTokenService.CreateToken(user, permissions);

        return ToAuthResponse(user, permissions, token);
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();
        var emailExists = await _dbContext.Users.AnyAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);
        if (emailExists)
        {
            throw new AppException(LocalizationKeys.EmailAlreadyInUse);
        }

        var defaultRole = await _dbContext.Roles
            .FirstOrDefaultAsync(x => x.NormalizedName == SeedDataDefaults.UserRoleNormalizedName, cancellationToken)
            ?? throw new AppException(LocalizationKeys.RoleNotFound, 404);

        var user = new Domain.Entities.User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim(),
            NormalizedEmail = normalizedEmail,
            PasswordHash = _passwordHasher.Hash(request.Password),
            RoleId = defaultRole.Id
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var standardUserPermissionGroup = await _dbContext.PermissionGroups
            .FirstOrDefaultAsync(x => x.NormalizedName == SeedDataDefaults.StandardUserPermissionGroupNormalizedName, cancellationToken)
            ?? throw new AppException(LocalizationKeys.PermissionGroupNotFound, 404);

        _dbContext.UserPermissionGroups.Add(new UserPermissionGroup
        {
            UserId = user.Id,
            PermissionGroupId = standardUserPermissionGroup.Id
        });

        await _dbContext.SaveChangesAsync(cancellationToken);

        user = await _dbContext.Users
            .Include(x => x.Role)
            .Include(x => x.UserPermissionGroups)
                .ThenInclude(x => x.PermissionGroup)
                    .ThenInclude(x => x.PermissionGroupPermissionDefinitions)
                        .ThenInclude(x => x.PermissionDefinition)
            .FirstAsync(x => x.Id == user.Id, cancellationToken);

        var permissions = GetPermissions(user);
        var token = _jwtTokenService.CreateToken(user, permissions);

        return ToAuthResponse(user, permissions, token);
    }

    public async Task<CurrentUserDto> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId ?? throw new AppException(LocalizationKeys.Unauthorized, 401);

        return await _dbContext.Users
            .Include(x => x.Role)
            .Where(x => x.Id == userId)
            .Select(x => new CurrentUserDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Role = x.Role.Name
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new AppException(LocalizationKeys.UserNotFound, 404);
    }

    public async Task<IReadOnlyCollection<string>> GetMyPermissionsAsync(CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId ?? throw new AppException(LocalizationKeys.Unauthorized, 401);

        return await _dbContext.UserPermissionGroups
            .Where(x => x.UserId == userId)
            .SelectMany(x => x.PermissionGroup.PermissionGroupPermissionDefinitions.Select(y => y.PermissionDefinition.Code))
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync(cancellationToken);
    }

    public async Task ChangePasswordAsync(ChangePasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId ?? throw new AppException(LocalizationKeys.Unauthorized, 401);
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
            ?? throw new AppException(LocalizationKeys.UserNotFound, 404);

        if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
        {
            throw new AppException(LocalizationKeys.CurrentPasswordIncorrect, 400);
        }

        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ForgotPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);

        if (user is null)
        {
            return new ForgotPasswordResponseDto
            {
                Message = LocalizationKeys.ForgotPasswordIfExists,
                EmailSent = false
            };
        }

        var tokenValue = Guid.NewGuid().ToString("N");
        _dbContext.PasswordResetTokens.Add(new Domain.Entities.PasswordResetToken
        {
            UserId = user.Id,
            Token = tokenValue,
            ExpiresAtUtc = DateTime.UtcNow.AddHours(2)
        });

        await _dbContext.SaveChangesAsync(cancellationToken);

        var body = $"""
            <p>{AppLocalizer.Get(LocalizationKeys.PasswordResetEmailGreeting, user.FirstName)}</p>
            <p>{AppLocalizer.Get(LocalizationKeys.PasswordResetEmailBody)}</p>
            <p><strong>{AppLocalizer.Get(LocalizationKeys.PasswordResetEmailToken, tokenValue)}</strong></p>
            <p>{AppLocalizer.Get(LocalizationKeys.PasswordResetEmailExpiry)}</p>
            """;

        var emailSent = false;
        try
        {
            emailSent = await _emailSenderService.SendAsync(
                user.Email,
                AppLocalizer.Get(LocalizationKeys.PasswordResetEmailSubject),
                body,
                cancellationToken);
        }
        catch
        {
            emailSent = false;
        }

        return new ForgotPasswordResponseDto
        {
            Message = emailSent
                ? LocalizationKeys.ForgotPasswordEmailSent
                : LocalizationKeys.ForgotPasswordPreview,
            EmailSent = emailSent,
            ResetTokenPreview = emailSent ? null : tokenValue
        };
    }

    public async Task ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        var token = await _dbContext.PasswordResetTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == request.Token, cancellationToken);

        if (token is null || token.User is null)
        {
            throw new AppException(LocalizationKeys.InvalidResetToken, 404);
        }

        if (token.IsUsed || token.ExpiresAtUtc < DateTime.UtcNow)
        {
            throw new AppException(LocalizationKeys.ResetTokenExpiredOrUsed);
        }

        token.User.PasswordHash = _passwordHasher.Hash(request.NewPassword);
        token.IsUsed = true;
        token.UsedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static IReadOnlyCollection<string> GetPermissions(Domain.Entities.User user)
    {
        return user.UserPermissionGroups
            .SelectMany(x => x.PermissionGroup.PermissionGroupPermissionDefinitions.Select(y => y.PermissionDefinition.Code))
            .Distinct()
            .OrderBy(x => x)
            .ToList();
    }

    private static AuthResponseDto ToAuthResponse(Domain.Entities.User user, IReadOnlyCollection<string> permissions, AuthTokenDto token)
    {
        return new AuthResponseDto
        {
            UserId = user.Id,
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            Email = user.Email,
            Role = user.Role.Name,
            Permissions = permissions,
            Token = token
        };
    }
}
