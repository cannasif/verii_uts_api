using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("RII_ROLES");
        builder.ConfigureBaseColumns();
        builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
        builder.Property(x => x.NormalizedName).HasColumnName("NormalizedName").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description).HasColumnName("Description").HasMaxLength(500);
        builder.Property(x => x.IsSystem).HasColumnName("IsSystem").IsRequired();
        builder.HasIndex(x => x.NormalizedName).IsUnique();
    }
}
