using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using uts_api.Domain.Entities;

namespace uts_api.Infrastructure.Persistence.Configurations;

public sealed class UtsIthalatListItemConfiguration : IEntityTypeConfiguration<UtsIthalatListItem>
{
    public void Configure(EntityTypeBuilder<UtsIthalatListItem> builder)
    {
        builder.ToView(UtsQueries.UtsIthalatListViewName);
        builder.HasNoKey();

        builder.Property(x => x.Chk).HasColumnName("CHK").HasMaxLength(1).IsRequired();
        builder.Property(x => x.SiraNo).HasColumnName("SIRA_NO").IsRequired();
        builder.Property(x => x.Bno).HasColumnName("BNO").HasMaxLength(16);
        builder.Property(x => x.Sira).HasColumnName("SIRA");
        builder.Property(x => x.Git).HasColumnName("GIT").HasMaxLength(10);
        builder.Property(x => x.Kun).HasColumnName("KUN").HasMaxLength(50);
        builder.Property(x => x.Uno).HasColumnName("UNO").HasMaxLength(50);
        builder.Property(x => x.LsNo).HasColumnName("LS_NO").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Adt).HasColumnName("ADT");
        builder.Property(x => x.Sinif).HasColumnName("SINIF").HasMaxLength(50).IsRequired();
        builder.Property(x => x.SeriMiLotMu).HasColumnName("SERIMILOTMU").HasMaxLength(1).IsRequired();
        builder.Property(x => x.CariKodu).HasColumnName("CARI_KODU").HasMaxLength(15).IsRequired();
        builder.Property(x => x.CariIsim).HasColumnName("CARI_ISIM").HasMaxLength(100);
        builder.Property(x => x.StokKodu).HasColumnName("STOK_KODU").HasMaxLength(35).IsRequired();
        builder.Property(x => x.StokAdi).HasColumnName("STOK_ADI").HasMaxLength(200);
        builder.Property(x => x.UtsDurum).HasColumnName("UTS_DURUM").HasMaxLength(1).IsRequired();
        builder.Property(x => x.UretimLsNo).HasColumnName("URETIM_LS_NO").HasMaxLength(53);
        builder.Property(x => x.Urt).HasColumnName("URT").HasMaxLength(10);
        builder.Property(x => x.Skt).HasColumnName("SKT").HasMaxLength(10);
        builder.Property(x => x.DepoKod).HasColumnName("DEPOKOD");
        builder.Property(x => x.OlcuBr).HasColumnName("OLCUBR");
        builder.Property(x => x.StharGcMik).HasColumnName("STHAR_GCMIK").HasPrecision(28, 8);
        builder.Property(x => x.StraInc).HasColumnName("STRA_INC");
        builder.Property(x => x.ImalIthal).HasColumnName("IMALITHAL").HasMaxLength(50);
        builder.Property(x => x.Gbn).HasColumnName("GBN").HasMaxLength(20);
        builder.Property(x => x.Ieu).HasColumnName("IEU").HasMaxLength(50);
        builder.Property(x => x.Meu).HasColumnName("MEU").HasMaxLength(50);
        builder.Property(x => x.Udi).HasColumnName("UDI");
    }
}
