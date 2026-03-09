using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class PermissionGroupPermissionDefinitionConfiguration : IEntityTypeConfiguration<PermissionGroupPermissionDefinition>
{
    public void Configure(EntityTypeBuilder<PermissionGroupPermissionDefinition> builder)
    {
        builder.ToTable("RII_PERMISSION_GROUP_PERMISSION_DEFINITIONS");
        builder.ConfigureBaseColumns();
        builder.Property(x => x.PermissionGroupId).HasColumnName("PermissionGroupId").IsRequired();
        builder.Property(x => x.PermissionDefinitionId).HasColumnName("PermissionDefinitionId").IsRequired();
        builder.HasIndex(x => new { x.PermissionGroupId, x.PermissionDefinitionId }).IsUnique();

        builder.HasOne(x => x.PermissionGroup)
            .WithMany(x => x.PermissionGroupPermissionDefinitions)
            .HasForeignKey(x => x.PermissionGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.PermissionDefinition)
            .WithMany(x => x.PermissionGroupPermissionDefinitions)
            .HasForeignKey(x => x.PermissionDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
