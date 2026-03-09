namespace uts_api.Application.DTOs.HangfireMonitoring;

public sealed class HangfireJobLogItemDto
{
    public string JobId { get; set; } = string.Empty;
    public string JobName { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public DateTime? FailedAt { get; set; }
    public DateTime? EnqueuedAt { get; set; }
    public string? Reason { get; set; }
    public string? ExceptionType { get; set; }
    public int RetryCount { get; set; }
    public string? Queue { get; set; }
}
