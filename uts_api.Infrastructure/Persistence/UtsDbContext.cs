using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Interfaces;
using uts_api.Domain.Common;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence;

public sealed class UtsDbContext : DbContext, IApplicationDbContext
{
    public UtsDbContext(DbContextOptions<UtsDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<PermissionDefinition> PermissionDefinitions => Set<PermissionDefinition>();
    public DbSet<PermissionGroup> PermissionGroups => Set<PermissionGroup>();
    public DbSet<UserPermissionGroup> UserPermissionGroups => Set<UserPermissionGroup>();
    public DbSet<PermissionGroupPermissionDefinition> PermissionGroupPermissionDefinitions => Set<PermissionGroupPermissionDefinition>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
    public DbSet<SmtpSetting> SmtpSettings => Set<SmtpSetting>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<HangfireJobLog> HangfireJobLogs => Set<HangfireJobLog>();
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UtsDbContext).Assembly);
        modelBuilder.Entity<RiiFnStockRow>().HasNoKey();
        modelBuilder.Entity<RiiFnCariRow>().HasNoKey();
        ApplySoftDeleteQueryFilters(modelBuilder);
    }

    private static void ApplySoftDeleteQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var parameter = Expression.Parameter(entityType.ClrType, "entity");
            var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var filter = Expression.Lambda(Expression.Equal(property, Expression.Constant(false)), parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
        }
    }
}
