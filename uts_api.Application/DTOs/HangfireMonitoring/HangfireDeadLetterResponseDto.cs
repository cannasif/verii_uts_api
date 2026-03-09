namespace uts_api.Application.DTOs.HangfireMonitoring;

public sealed class HangfireDeadLetterResponseDto
{
    public string Queue { get; set; } = "dead-letter";
    public int Enqueued { get; set; }
    public List<HangfireJobLogItemDto> Items { get; set; } = [];
    public DateTime Timestamp { get; set; }
}
