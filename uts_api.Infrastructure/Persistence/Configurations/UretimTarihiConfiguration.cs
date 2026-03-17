using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class UretimTarihiConfiguration : IEntityTypeConfiguration<UretimTarihi>
{
    public void Configure(EntityTypeBuilder<UretimTarihi> builder)
    {
        builder.ToTable("RII_URETIM_TARIHI");
        builder.ConfigureBaseColumns();

        builder.Property(x => x.WfState).HasColumnName("WfState");
        builder.Property(x => x.StokKodu).HasColumnName("StokKodu").HasMaxLength(50);
        builder.Property(x => x.SeriLotNo).HasColumnName("SeriLotNo").HasMaxLength(50);
        builder.Property(x => x.Tarih).HasColumnName("Tarih");
        builder.Property(x => x.LotNo).HasColumnName("LotNo").HasMaxLength(50);
        builder.Property(x => x.SonKulTarih).HasColumnName("SonKulTarih");
        builder.Property(x => x.SYedek1).HasColumnName("SYedek1").HasMaxLength(150);
        builder.Property(x => x.SYedek2).HasColumnName("SYedek2").HasMaxLength(250);
        builder.Property(x => x.DYedek1).HasColumnName("DYedek1");
        builder.Property(x => x.IYedek1).HasColumnName("IYedek1");
        builder.Property(x => x.IYedek2).HasColumnName("IYedek2");
        builder.Property(x => x.FYedek1).HasColumnName("FYedek1");
        builder.Property(x => x.FYedek2).HasColumnName("FYedek2");

        builder.HasIndex(x => x.StokKodu).HasDatabaseName("IX_UretimTarihi_StokKodu");
        builder.HasIndex(x => x.SeriLotNo).HasDatabaseName("IX_UretimTarihi_SeriLotNo");
        builder.HasIndex(x => x.LotNo).HasDatabaseName("IX_UretimTarihi_LotNo");
        builder.HasIndex(x => x.Tarih).HasDatabaseName("IX_UretimTarihi_Tarih");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_UretimTarihi_IsDeleted");
    }
}
