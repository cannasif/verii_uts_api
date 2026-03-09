using uts_api.Domain.Common;

namespace uts_api.Domain.Entities;

public sealed class HangfireJobLog : BaseEntity
{
    public string JobId { get; set; } = string.Empty;
    public string JobName { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public DateTime OccurredAtUtc { get; set; }
    public string? Reason { get; set; }
    public string? ExceptionType { get; set; }
    public string? ExceptionMessage { get; set; }
    public string? StackTrace { get; set; }
    public string? Queue { get; set; }
    public int RetryCount { get; set; }
}
