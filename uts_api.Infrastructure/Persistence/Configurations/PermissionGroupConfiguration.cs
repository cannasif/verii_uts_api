using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class PermissionGroupConfiguration : IEntityTypeConfiguration<PermissionGroup>
{
    public void Configure(EntityTypeBuilder<PermissionGroup> builder)
    {
        builder.ToTable("RII_PERMISSION_GROUPS");
        builder.ConfigureBaseColumns();
        builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
        builder.Property(x => x.NormalizedName).HasColumnName("NormalizedName").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description).HasColumnName("Description").HasMaxLength(500);
        builder.Property(x => x.IsSystem).HasColumnName("IsSystem").IsRequired();
        builder.HasIndex(x => x.NormalizedName).IsUnique();
    }
}
