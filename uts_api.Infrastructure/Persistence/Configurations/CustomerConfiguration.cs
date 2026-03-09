using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("RII_CUSTOMERS");
        builder.ConfigureBaseColumns();

        builder.Property(x => x.CustomerCode).HasColumnName("CustomerCode").HasMaxLength(50).IsRequired();
        builder.Property(x => x.CustomerName).HasColumnName("CustomerName").HasMaxLength(255).IsRequired();
        builder.Property(x => x.TaxOffice).HasColumnName("TaxOffice").HasMaxLength(150);
        builder.Property(x => x.TaxNumber).HasColumnName("TaxNumber").HasMaxLength(50);
        builder.Property(x => x.TcknNumber).HasColumnName("TcknNumber").HasMaxLength(50);
        builder.Property(x => x.Email).HasColumnName("Email").HasMaxLength(255);
        builder.Property(x => x.Website).HasColumnName("Website").HasMaxLength(255);
        builder.Property(x => x.Phone).HasColumnName("Phone").HasMaxLength(50);
        builder.Property(x => x.Address).HasColumnName("Address").HasMaxLength(500);
        builder.Property(x => x.City).HasColumnName("City").HasMaxLength(100);
        builder.Property(x => x.District).HasColumnName("District").HasMaxLength(100);
        builder.Property(x => x.CountryCode).HasColumnName("CountryCode").HasMaxLength(20);
        builder.Property(x => x.BranchCode).HasColumnName("BranchCode").IsRequired().HasDefaultValue((short)0);
        builder.Property(x => x.BusinessUnitCode).HasColumnName("BusinessUnitCode").IsRequired().HasDefaultValue((short)0);
        builder.Property(x => x.IsErpIntegrated).HasColumnName("IsErpIntegrated").IsRequired().HasDefaultValue(false);
        builder.Property(x => x.ErpIntegrationNumber).HasColumnName("ErpIntegrationNumber").HasMaxLength(50);
        builder.Property(x => x.LastSyncDateUtc).HasColumnName("LastSyncDateUtc");

        builder.HasIndex(x => x.CustomerCode).HasDatabaseName("IX_Customer_CustomerCode");
        builder.HasIndex(x => x.CustomerName).HasDatabaseName("IX_Customer_CustomerName");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_Customer_IsDeleted");
    }
}
