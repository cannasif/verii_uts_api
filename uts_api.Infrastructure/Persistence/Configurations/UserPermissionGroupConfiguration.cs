using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class UserPermissionGroupConfiguration : IEntityTypeConfiguration<UserPermissionGroup>
{
    public void Configure(EntityTypeBuilder<UserPermissionGroup> builder)
    {
        builder.ToTable("RII_USER_PERMISSION_GROUPS");
        builder.ConfigureBaseColumns();
        builder.Property(x => x.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(x => x.PermissionGroupId).HasColumnName("PermissionGroupId").IsRequired();
        builder.HasIndex(x => new { x.UserId, x.PermissionGroupId }).IsUnique();

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserPermissionGroups)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.PermissionGroup)
            .WithMany(x => x.UserPermissionGroups)
            .HasForeignKey(x => x.PermissionGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
