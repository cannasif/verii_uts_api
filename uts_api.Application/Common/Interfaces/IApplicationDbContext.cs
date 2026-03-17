using Microsoft.EntityFrameworkCore;
using uts_api.Domain.Entities;

namespace uts_api.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<PermissionDefinition> PermissionDefinitions { get; }
    DbSet<PermissionGroup> PermissionGroups { get; }
    DbSet<UserPermissionGroup> UserPermissionGroups { get; }
    DbSet<PermissionGroupPermissionDefinition> PermissionGroupPermissionDefinitions { get; }
    DbSet<PasswordResetToken> PasswordResetTokens { get; }
    DbSet<SmtpSetting> SmtpSettings { get; }
    DbSet<UserProfile> UserProfiles { get; }
    DbSet<HangfireJobLog> HangfireJobLogs { get; }
    DbSet<Stock> Stocks { get; }
    DbSet<Customer> Customers { get; }
    DbSet<UtsLog> UtsLogs { get; }
    DbSet<UretimTarihi> UretimTarihleri { get; }
    DbSet<UtsVermeListItem> UtsVermeListItems { get; }
    DbSet<UtsUretimListItem> UtsUretimListItems { get; }
    DbSet<UtsTVermeListItem> UtsTVermeListItems { get; }
    DbSet<UtsTuketiciVermeListItem> UtsTuketiciVermeListItems { get; }
    DbSet<UtsIthalatListItem> UtsIthalatListItems { get; }
    DbSet<UtsImhaListItem> UtsImhaListItems { get; }
    DbSet<UtsIhracatListItem> UtsIhracatListItems { get; }
    DbSet<UtsAlmaListItem> UtsAlmaListItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
