using AutoMapper;
using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Exceptions;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.Users;
using uts_api.Application.Interfaces;
using uts_api.Domain.Entities;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class UserService : IUserService
{
    private static readonly IReadOnlyDictionary<string, string> AllowedColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["id"] = "Id",
        ["firstName"] = "FirstName",
        ["lastName"] = "LastName",
        ["email"] = "Email",
        ["roleId"] = "RoleId",
        ["roleName"] = "Role.Name",
        ["createdAtUtc"] = "CreatedAtUtc"
    };

    private readonly IApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public UserService(IApplicationDbContext dbContext, IPasswordHasher passwordHasher, IMapper mapper)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<PagedResult<UserListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .ApplySearch(request.Search, "FirstName", "LastName", "Email", "Role.Name")
            .ApplyFilters(request.Filters, AllowedColumns, request.FilterLogic)
            .ApplySorting(request.SortBy, request.SortDirection, AllowedColumns)
            .Select(x => new UserListItemDto
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            RoleName = x.Role.Name,
            CreatedAtUtc = x.CreatedAtUtc
        });

        return await query.ToPagedResultAsync(request, cancellationToken);
    }

    public async Task<UserDetailDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .Include(x => x.UserPermissionGroups)
            .Where(x => x.Id == id)
            .Select(x => new UserDetailDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                RoleId = x.RoleId,
                RoleName = x.Role.Name,
                PermissionGroupIds = x.UserPermissionGroups.Select(y => y.PermissionGroupId).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new AppException(LocalizationKeys.UserNotFound, 404);
    }

    public async Task<UserDetailDto> CreateAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();
        if (await _dbContext.Users.AnyAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken))
        {
            throw new AppException(LocalizationKeys.EmailAlreadyInUse);
        }

        await EnsureRoleExists(request.RoleId, cancellationToken);
        await EnsurePermissionGroupsExist(request.PermissionGroupIds, cancellationToken);

        var user = _mapper.Map<User>(request);
        user.NormalizedEmail = normalizedEmail;
        user.PasswordHash = _passwordHasher.Hash(request.Password);

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        if (request.PermissionGroupIds.Count > 0)
        {
            _dbContext.UserPermissionGroups.AddRange(request.PermissionGroupIds.Distinct().Select(permissionGroupId => new Domain.Entities.UserPermissionGroup
            {
                UserId = user.Id,
                PermissionGroupId = permissionGroupId
            }));

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return await GetByIdAsync(user.Id, cancellationToken);
    }

    public async Task<UserDetailDto> UpdateAsync(long id, UpdateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new AppException(LocalizationKeys.UserNotFound, 404);

        var normalizedEmail = request.Email.Trim().ToUpperInvariant();
        var duplicateExists = await _dbContext.Users.AnyAsync(x => x.Id != id && x.NormalizedEmail == normalizedEmail, cancellationToken);
        if (duplicateExists)
        {
            throw new AppException(LocalizationKeys.EmailAlreadyInUse);
        }

        await EnsureRoleExists(request.RoleId, cancellationToken);

        _mapper.Map(request, user);
        user.NormalizedEmail = normalizedEmail;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new AppException(LocalizationKeys.UserNotFound, 404);

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureRoleExists(long roleId, CancellationToken cancellationToken)
    {
        if (!await _dbContext.Roles.AnyAsync(x => x.Id == roleId, cancellationToken))
        {
            throw new AppException(LocalizationKeys.RoleNotFound, 404);
        }
    }

    private async Task EnsurePermissionGroupsExist(IEnumerable<long> permissionGroupIds, CancellationToken cancellationToken)
    {
        var ids = permissionGroupIds.Distinct().ToList();
        if (ids.Count == 0)
        {
            return;
        }

        var existingCount = await _dbContext.PermissionGroups.CountAsync(x => ids.Contains(x.Id), cancellationToken);
        if (existingCount != ids.Count)
        {
            throw new AppException(LocalizationKeys.PermissionGroupsNotFound, 404);
        }
    }
}
