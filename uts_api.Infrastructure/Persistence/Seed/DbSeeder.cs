using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Security;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(UtsDbContext dbContext, IPasswordHasher passwordHasher, CancellationToken cancellationToken = default)
    {
        if (!await dbContext.Roles.IgnoreQueryFilters().AnyAsync(x => x.NormalizedName == SeedDataDefaults.AdminRoleNormalizedName, cancellationToken))
        {
            dbContext.Roles.Add(new Role
            {
                Name = SeedDataDefaults.AdminRoleName,
                NormalizedName = SeedDataDefaults.AdminRoleNormalizedName,
                Description = "System administrator",
                IsSystem = true
            });
        }

        if (!await dbContext.Roles.IgnoreQueryFilters().AnyAsync(x => x.NormalizedName == SeedDataDefaults.UserRoleNormalizedName, cancellationToken))
        {
            dbContext.Roles.Add(new Role
            {
                Name = SeedDataDefaults.UserRoleName,
                NormalizedName = SeedDataDefaults.UserRoleNormalizedName,
                Description = "Standard user",
                IsSystem = true
            });
        }

        var existingDefinitions = await dbContext.PermissionDefinitions.IgnoreQueryFilters()
            .ToDictionaryAsync(x => x.Code, StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var definition in PermissionConstants.All)
        {
            if (existingDefinitions.TryGetValue(definition.Code, out var existingDefinition))
            {
                existingDefinition.Name = definition.Name;
                existingDefinition.Module = definition.Module;
                existingDefinition.Description = definition.Description;
                continue;
            }

            dbContext.PermissionDefinitions.Add(new PermissionDefinition
            {
                Name = definition.Name,
                Code = definition.Code,
                Description = definition.Description,
                Module = definition.Module
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var adminRole = await dbContext.Roles.IgnoreQueryFilters()
            .FirstAsync(x => x.NormalizedName == SeedDataDefaults.AdminRoleNormalizedName, cancellationToken);
        if (!await dbContext.PermissionGroups.IgnoreQueryFilters().AnyAsync(x => x.NormalizedName == SeedDataDefaults.SystemAdminPermissionGroupNormalizedName, cancellationToken))
        {
            dbContext.PermissionGroups.Add(new PermissionGroup
            {
                Name = SeedDataDefaults.SystemAdminPermissionGroupName,
                NormalizedName = SeedDataDefaults.SystemAdminPermissionGroupNormalizedName,
                Description = "Full access permission group",
                IsSystem = true
            });
        }

        if (!await dbContext.PermissionGroups.IgnoreQueryFilters().AnyAsync(x => x.NormalizedName == SeedDataDefaults.StandardUserPermissionGroupNormalizedName, cancellationToken))
        {
            dbContext.PermissionGroups.Add(new PermissionGroup
            {
                Name = SeedDataDefaults.StandardUserPermissionGroupName,
                NormalizedName = SeedDataDefaults.StandardUserPermissionGroupNormalizedName,
                Description = "Basic authenticated user permissions",
                IsSystem = true
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var systemAdminPermissionGroup = await dbContext.PermissionGroups.IgnoreQueryFilters()
            .FirstAsync(x => x.NormalizedName == SeedDataDefaults.SystemAdminPermissionGroupNormalizedName, cancellationToken);
        var standardUserPermissionGroup = await dbContext.PermissionGroups.IgnoreQueryFilters()
            .FirstAsync(x => x.NormalizedName == SeedDataDefaults.StandardUserPermissionGroupNormalizedName, cancellationToken);

        var definitions = await dbContext.PermissionDefinitions.IgnoreQueryFilters().ToListAsync(cancellationToken);
        var standardCodes = new[]
        {
            PermissionConstants.Auth.Me,
            PermissionConstants.Auth.MyPermissions
        };

        var existingGroupDefinitionLinks = await dbContext.PermissionGroupPermissionDefinitions.IgnoreQueryFilters()
            .ToListAsync(cancellationToken);

        foreach (var definition in definitions)
        {
            if (!existingGroupDefinitionLinks.Any(x => x.PermissionGroupId == systemAdminPermissionGroup.Id && x.PermissionDefinitionId == definition.Id))
            {
                dbContext.PermissionGroupPermissionDefinitions.Add(new PermissionGroupPermissionDefinition
                {
                    PermissionGroupId = systemAdminPermissionGroup.Id,
                    PermissionDefinitionId = definition.Id
                });
            }
        }

        foreach (var definition in definitions.Where(x => standardCodes.Contains(x.Code)))
        {
            if (!existingGroupDefinitionLinks.Any(x => x.PermissionGroupId == standardUserPermissionGroup.Id && x.PermissionDefinitionId == definition.Id))
            {
                dbContext.PermissionGroupPermissionDefinitions.Add(new PermissionGroupPermissionDefinition
                {
                    PermissionGroupId = standardUserPermissionGroup.Id,
                    PermissionDefinitionId = definition.Id
                });
            }
        }

        if (!await dbContext.Users.IgnoreQueryFilters().AnyAsync(x => x.NormalizedEmail == SeedDataDefaults.InitialAdminNormalizedEmail, cancellationToken))
        {
            dbContext.Users.Add(new User
            {
                FirstName = "System",
                LastName = "Admin",
                Email = SeedDataDefaults.InitialAdminEmail,
                NormalizedEmail = SeedDataDefaults.InitialAdminNormalizedEmail,
                PasswordHash = passwordHasher.Hash(SeedDataDefaults.InitialAdminPassword),
                RoleId = adminRole.Id
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var initialAdminUser = await dbContext.Users.IgnoreQueryFilters()
            .FirstAsync(x => x.NormalizedEmail == SeedDataDefaults.InitialAdminNormalizedEmail, cancellationToken);

        if (!await dbContext.UserPermissionGroups.IgnoreQueryFilters().AnyAsync(x => x.UserId == initialAdminUser.Id, cancellationToken))
        {
            dbContext.UserPermissionGroups.Add(new UserPermissionGroup
            {
                UserId = initialAdminUser.Id,
                PermissionGroupId = systemAdminPermissionGroup.Id
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
