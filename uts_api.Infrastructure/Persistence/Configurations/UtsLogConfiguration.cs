using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class UtsLogConfiguration : IEntityTypeConfiguration<UtsLog>
{
    public void Configure(EntityTypeBuilder<UtsLog> builder)
    {
        builder.ToTable("RII_UTS_LOG");
        builder.ConfigureBaseColumns();

        builder.Property(x => x.Bno).HasColumnName("BNO").HasMaxLength(255).IsRequired();
        builder.Property(x => x.Sira).HasColumnName("SIRA");
        builder.Property(x => x.SeriTraInc).HasColumnName("SERITRAINC");
        builder.Property(x => x.StokKodu).HasColumnName("STOK_KODU").HasMaxLength(35);
        builder.Property(x => x.SeriNo).HasColumnName("SERI_NO").HasMaxLength(50);
        builder.Property(x => x.Miktar).HasColumnName("MIKTAR").HasPrecision(28, 8);
        builder.Property(x => x.GonderimTarihi).HasColumnName("GONDERIM_TARIHI");
        builder.Property(x => x.GonderenKisi).HasColumnName("GONDEREN_KISI").HasMaxLength(128);
        builder.Property(x => x.GonderimTipi).HasColumnName("GONDERIM_TIPI").HasMaxLength(5);
        builder.Property(x => x.Sonuc).HasColumnName("SONUC");
        builder.Property(x => x.Durum).HasColumnName("DURUM").HasMaxLength(1).IsFixedLength();

        builder.HasIndex(x => x.Bno).HasDatabaseName("IX_UtsLog_Bno");
        builder.HasIndex(x => x.StokKodu).HasDatabaseName("IX_UtsLog_StokKodu");
        builder.HasIndex(x => x.SeriNo).HasDatabaseName("IX_UtsLog_SeriNo");
        builder.HasIndex(x => x.GonderimTarihi).HasDatabaseName("IX_UtsLog_GonderimTarihi");
        builder.HasIndex(x => x.Durum).HasDatabaseName("IX_UtsLog_Durum");
        builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_UtsLog_IsDeleted");
    }
}
