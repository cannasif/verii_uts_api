using uts_api.Domain.Common;

namespace uts_api.Domain.Entities;

public sealed class UtsLog : BaseEntity
{
    public string Bno { get; set; } = string.Empty;
    public int? Sira { get; set; }
    public int? SeriTraInc { get; set; }
    public string? StokKodu { get; set; }
    public string? SeriNo { get; set; }
    public decimal? Miktar { get; set; }
    public DateTime? GonderimTarihi { get; set; }
    public string? GonderenKisi { get; set; }
    public string? GonderimTipi { get; set; }
    public string? Sonuc { get; set; }
    public string? Durum { get; set; }
}
