using uts_api.Application.DTOs.HangfireMonitoring;

namespace uts_api.Application.Interfaces;

public interface IHangfireMonitoringService
{
    Task<HangfireStatsDto> GetStatsAsync(CancellationToken cancellationToken = default);
    Task<HangfireFailedJobsResponseDto> GetFailuresFromDbAsync(int from = 0, int count = 50, CancellationToken cancellationToken = default);
    Task<HangfireDeadLetterResponseDto> GetDeadLetterAsync(int from = 0, int count = 20, CancellationToken cancellationToken = default);
}
