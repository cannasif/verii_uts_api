namespace uts_api.Application.DTOs.UtsLogs;

public sealed class CreateUtsLogRequestDto
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
