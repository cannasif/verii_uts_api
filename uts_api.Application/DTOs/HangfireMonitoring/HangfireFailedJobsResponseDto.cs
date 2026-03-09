namespace uts_api.Application.DTOs.HangfireMonitoring;

public sealed class HangfireFailedJobsResponseDto
{
    public List<HangfireJobLogItemDto> Items { get; set; } = [];
    public int Total { get; set; }
    public DateTime Timestamp { get; set; }
}
