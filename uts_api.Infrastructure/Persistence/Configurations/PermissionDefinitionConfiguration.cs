using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class PermissionDefinitionConfiguration : IEntityTypeConfiguration<PermissionDefinition>
{
    public void Configure(EntityTypeBuilder<PermissionDefinition> builder)
    {
        builder.ToTable("RII_PERMISSION_DEFINITIONS");
        builder.ConfigureBaseColumns();
        builder.Property(x => x.Module).HasColumnName("Module").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(150).IsRequired();
        builder.Property(x => x.Code).HasColumnName("Code").HasMaxLength(150).IsRequired();
        builder.Property(x => x.Description).HasColumnName("Description").HasMaxLength(500);
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
