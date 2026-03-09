using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("RII_USER_PROFILES");
        builder.ConfigureBaseColumns();
        builder.Property(x => x.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(x => x.ProfilePictureUrl).HasColumnName("ProfilePictureUrl").HasMaxLength(500);
        builder.Property(x => x.PhoneNumber).HasColumnName("PhoneNumber").HasMaxLength(20);
        builder.Property(x => x.JobTitle).HasColumnName("JobTitle").HasMaxLength(150);
        builder.Property(x => x.Department).HasColumnName("Department").HasMaxLength(150);
        builder.Property(x => x.Bio).HasColumnName("Bio").HasMaxLength(2000);

        builder.HasIndex(x => x.UserId).IsUnique();

        builder.HasOne(x => x.User)
            .WithOne(x => x.Profile)
            .HasForeignKey<UserProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
