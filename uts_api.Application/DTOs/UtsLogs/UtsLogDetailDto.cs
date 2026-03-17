namespace uts_api.Application.DTOs.UtsLogs;

public sealed class UtsLogDetailDto
{
    public long Id { get; set; }
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
    public string? CreateUser { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public string? UpdateUser { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
