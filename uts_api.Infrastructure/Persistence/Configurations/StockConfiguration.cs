using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.ToTable("RII_STOCK");
        builder.ConfigureBaseColumns();

        builder.Property(x => x.ErpStockCode).HasColumnName("ErpStockCode").HasMaxLength(50).IsRequired();
        builder.Property(x => x.StockName).HasColumnName("StockName").HasMaxLength(250).IsRequired();
        builder.Property(x => x.Unit).HasColumnName("Unit").HasMaxLength(20);
        builder.Property(x => x.UreticiKodu).HasColumnName("UreticiKodu").HasMaxLength(50);
        builder.Property(x => x.GrupKodu).HasColumnName("GrupKodu").HasMaxLength(50);
        builder.Property(x => x.GrupAdi).HasColumnName("GrupAdi").HasMaxLength(250);
        builder.Property(x => x.Kod1).HasColumnName("Kod1").HasMaxLength(50);
        builder.Property(x => x.Kod1Adi).HasColumnName("Kod1Adi").HasMaxLength(250);
        builder.Property(x => x.Kod2).HasColumnName("Kod2").HasMaxLength(50);
        builder.Property(x => x.Kod2Adi).HasColumnName("Kod2Adi").HasMaxLength(250);
        builder.Property(x => x.Kod3).HasColumnName("Kod3").HasMaxLength(50);
        builder.Property(x => x.Kod3Adi).HasColumnName("Kod3Adi").HasMaxLength(250);
        builder.Property(x => x.Kod4).HasColumnName("Kod4").HasMaxLength(50);
        builder.Property(x => x.Kod4Adi).HasColumnName("Kod4Adi").HasMaxLength(250);
        builder.Property(x => x.Kod5).HasColumnName("Kod5").HasMaxLength(50);
        builder.Property(x => x.Kod5Adi).HasColumnName("Kod5Adi").HasMaxLength(250);
        builder.Property(x => x.BranchCode).HasColumnName("BranchCode").IsRequired().HasDefaultValue(0);

        builder.HasIndex(x => x.ErpStockCode).HasDatabaseName("IX_Stock_ErpStockCode");
        builder.HasIndex(x => x.StockName).HasDatabaseName("IX_Stock_StockName");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_Stock_IsDeleted");
    }
}
