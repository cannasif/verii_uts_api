using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.DTOs.HangfireMonitoring;
using uts_api.Application.Interfaces;

namespace uts_api.Infrastructure.Hangfire;

public sealed class HangfireMonitoringService : IHangfireMonitoringService
{
    private readonly IApplicationDbContext _dbContext;

    public HangfireMonitoringService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<HangfireStatsDto> GetStatsAsync(CancellationToken cancellationToken = default)
    {
        var monitoringApi = JobStorage.Current.GetMonitoringApi();
        var queues = monitoringApi.Queues();
        var stats = monitoringApi.GetStatistics();

        return Task.FromResult(new HangfireStatsDto
        {
            Enqueued = stats.Enqueued,
            Processing = stats.Processing,
            Scheduled = stats.Scheduled,
            Succeeded = stats.Succeeded,
            Failed = stats.Failed,
            Deleted = stats.Deleted,
            Servers = monitoringApi.Servers().Count,
            Queues = queues.Count,
            Timestamp = DateTime.UtcNow
        });
    }

    public async Task<HangfireFailedJobsResponseDto> GetFailuresFromDbAsync(int from = 0, int count = 50, CancellationToken cancellationToken = default)
    {
        from = Math.Max(0, from);
        count = Math.Clamp(count, 1, 200);

        var query = _dbContext.HangfireJobLogs
            .AsNoTracking()
            .Where(x => x.State == "Failed");

        var items = await query
            .OrderByDescending(x => x.OccurredAtUtc)
            .Skip(from)
            .Take(count)
            .Select(x => new HangfireJobLogItemDto
            {
                JobId = x.JobId,
                JobName = x.JobName,
                FailedAt = x.OccurredAtUtc,
                State = x.State,
                Reason = x.ExceptionMessage ?? x.Reason,
                ExceptionType = x.ExceptionType,
                RetryCount = x.RetryCount,
                Queue = x.Queue
            })
            .ToListAsync(cancellationToken);

        var total = await query.CountAsync(cancellationToken);

        return new HangfireFailedJobsResponseDto
        {
            Items = items,
            Total = total,
            Timestamp = DateTime.UtcNow
        };
    }

    public async Task<HangfireDeadLetterResponseDto> GetDeadLetterAsync(int from = 0, int count = 20, CancellationToken cancellationToken = default)
    {
        from = Math.Max(0, from);
        count = Math.Clamp(count, 1, 200);

        var query = _dbContext.HangfireJobLogs
            .AsNoTracking()
            .Where(x => x.Queue == "dead-letter");

        var items = await query
            .OrderByDescending(x => x.OccurredAtUtc)
            .Skip(from)
            .Take(count)
            .Select(x => new HangfireJobLogItemDto
            {
                JobId = x.JobId,
                JobName = x.JobName,
                EnqueuedAt = x.OccurredAtUtc,
                State = x.State,
                Reason = x.ExceptionMessage ?? x.Reason,
                ExceptionType = x.ExceptionType,
                RetryCount = x.RetryCount,
                Queue = x.Queue
            })
            .ToListAsync(cancellationToken);

        var total = await query.CountAsync(cancellationToken);

        return new HangfireDeadLetterResponseDto
        {
            Queue = "dead-letter",
            Enqueued = total,
            Items = items,
            Timestamp = DateTime.UtcNow
        };
    }
}
