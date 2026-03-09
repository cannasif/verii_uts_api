using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class SmtpSettingConfiguration : IEntityTypeConfiguration<SmtpSetting>
{
    public void Configure(EntityTypeBuilder<SmtpSetting> builder)
    {
        builder.ToTable("RII_SMTP_SETTINGS");
        builder.ConfigureBaseColumns();
        builder.Property(x => x.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Host).HasColumnName("Host").HasMaxLength(200).IsRequired();
        builder.Property(x => x.Port).HasColumnName("Port").IsRequired();
        builder.Property(x => x.UserName).HasColumnName("UserName").HasMaxLength(255);
        builder.Property(x => x.Password).HasColumnName("Password").HasMaxLength(500);
        builder.Property(x => x.FromEmail).HasColumnName("FromEmail").HasMaxLength(255).IsRequired();
        builder.Property(x => x.FromName).HasColumnName("FromName").HasMaxLength(100);
        builder.Property(x => x.EnableSsl).HasColumnName("EnableSsl").IsRequired();
        builder.Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
    }
}
