using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Common;

namespace uts_api.Infrastructure.Persistence.Configurations;

public static class BaseEntityConfigurationExtensions
{
    public static void ConfigureBaseColumns<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("Id");
        builder.Property(x => x.CreateUser).HasColumnName("CreateUser").HasMaxLength(100);
        builder.Property(x => x.CreatedAtUtc).HasColumnName("CreatedAtUtc").IsRequired();
        builder.Property(x => x.UpdateUser).HasColumnName("UpdateUser").HasMaxLength(100);
        builder.Property(x => x.UpdatedAtUtc).HasColumnName("UpdatedAtUtc");
        builder.Property(x => x.DeleteUser).HasColumnName("DeleteUser").HasMaxLength(100);
        builder.Property(x => x.DeletedAtUtc).HasColumnName("DeletedAtUtc");
        builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").IsRequired();
    }
}
