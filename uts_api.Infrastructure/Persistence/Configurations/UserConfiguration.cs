using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("RII_USERS");
        builder.ConfigureBaseColumns();
        builder.Property(x => x.FirstName).HasColumnName("FirstName").HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastName).HasColumnName("LastName").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).HasColumnName("Email").HasMaxLength(255).IsRequired();
        builder.Property(x => x.NormalizedEmail).HasColumnName("NormalizedEmail").HasMaxLength(255).IsRequired();
        builder.Property(x => x.PasswordHash).HasColumnName("PasswordHash").IsRequired();
        builder.Property(x => x.RoleId).HasColumnName("RoleId").IsRequired();
        builder.HasIndex(x => x.NormalizedEmail).IsUnique();

        builder.HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
