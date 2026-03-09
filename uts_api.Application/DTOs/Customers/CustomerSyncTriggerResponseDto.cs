namespace uts_api.Application.DTOs.Customers;

public sealed class CustomerSyncTriggerResponseDto
{
    public string JobId { get; set; } = string.Empty;
    public string Queue { get; set; } = string.Empty;
    public DateTime EnqueuedAtUtc { get; set; }
}
