namespace uts_api.Application.DTOs.Stocks;

public sealed class StockSyncTriggerResponseDto
{
    public string JobId { get; init; } = string.Empty;
    public string Queue { get; init; } = "default";
    public DateTime EnqueuedAtUtc { get; init; }
}
