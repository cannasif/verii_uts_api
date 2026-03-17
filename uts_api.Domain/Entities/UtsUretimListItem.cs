namespace uts_api.Domain.Entities;

public sealed class UtsUretimListItem
{
    public string Chk { get; set; } = string.Empty;
    public int SiraNo { get; set; }
    public string? Bno { get; set; }
    public int Sira { get; set; }
    public string? Git { get; set; }
    public string? Uno { get; set; }
    public string LsNo { get; set; } = string.Empty;
    public int? Adt { get; set; }
    public string Sinif { get; set; } = string.Empty;
    public string SeriMiLotMu { get; set; } = string.Empty;
    public string StokKodu { get; set; } = string.Empty;
    public string? StokAdi { get; set; }
    public string? Urt { get; set; }
    public string? Skt { get; set; }
    public string UtsDurum { get; set; } = string.Empty;
}
