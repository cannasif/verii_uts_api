namespace uts_api.Application.DTOs.UtsAlmaList;

public sealed class UtsAlmaListItemDto
{
    public string Chk { get; set; } = string.Empty;
    public int? SiraNo { get; set; }
    public string? Bno { get; set; }
    public short? Sira { get; set; }
    public string? Git { get; set; }
    public string? Kun { get; set; }
    public string? Uno { get; set; }
    public string? LsNo { get; set; }
    public int? Adt { get; set; }
    public string Sinif { get; set; } = string.Empty;
    public string SeriMiLotMu { get; set; } = string.Empty;
    public string CariKodu { get; set; } = string.Empty;
    public string? CariIsim { get; set; }
    public string StokKodu { get; set; } = string.Empty;
    public string? StokAdi { get; set; }
    public string? Acik16 { get; set; }
    public string UtsDurum { get; set; } = string.Empty;
}
