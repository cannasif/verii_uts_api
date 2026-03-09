using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable("RII_PASSWORD_RESET_TOKENS");
        builder.ConfigureBaseColumns();
        builder.Property(x => x.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(x => x.Token).HasColumnName("Token").HasMaxLength(100).IsRequired();
        builder.Property(x => x.ExpiresAtUtc).HasColumnName("ExpiresAtUtc").IsRequired();
        builder.Property(x => x.IsUsed).HasColumnName("IsUsed").IsRequired();
        builder.Property(x => x.UsedAtUtc).HasColumnName("UsedAtUtc");
        builder.HasIndex(x => x.Token).IsUnique();

        builder.HasOne(x => x.User)
            .WithMany(x => x.PasswordResetTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
