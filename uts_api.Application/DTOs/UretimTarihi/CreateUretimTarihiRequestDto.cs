namespace uts_api.Application.DTOs.UretimTarihi;

public sealed class CreateUretimTarihiRequestDto
{
    public byte? WfState { get; set; }
    public string? StokKodu { get; set; }
    public string? SeriLotNo { get; set; }
    public DateTime? Tarih { get; set; }
    public string? LotNo { get; set; }
    public DateTime? SonKulTarih { get; set; }
    public string? SYedek1 { get; set; }
    public string? SYedek2 { get; set; }
    public DateTime? DYedek1 { get; set; }
    public int? IYedek1 { get; set; }
    public int? IYedek2 { get; set; }
    public double? FYedek1 { get; set; }
    public double? FYedek2 { get; set; }
}
