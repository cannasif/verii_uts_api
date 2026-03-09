namespace uts_api.Application.DTOs.Stocks;

public sealed class StockListItemDto
{
    public long Id { get; init; }
    public string ErpStockCode { get; init; } = string.Empty;
    public string StockName { get; init; } = string.Empty;
    public string? Unit { get; init; }
    public string? GrupKodu { get; init; }
    public string? GrupAdi { get; init; }
    public int BranchCode { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}
