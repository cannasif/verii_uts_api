using Hangfire;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Domain.Entities;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Hangfire;

public sealed class HangfireJobStateFilter : IApplyStateFilter
{
    private readonly ILogger<HangfireJobStateFilter> _logger;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly HangfireMonitoringOptions _options;
    private readonly IServiceScopeFactory _scopeFactory;

    public HangfireJobStateFilter(
        ILogger<HangfireJobStateFilter> logger,
        IBackgroundJobClient backgroundJobClient,
        IOptions<HangfireMonitoringOptions> options,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _backgroundJobClient = backgroundJobClient;
        _options = options.Value;
        _scopeFactory = scopeFactory;
    }

    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        var jobId = context.BackgroundJob?.Id ?? "unknown";
        var job = context.BackgroundJob?.Job;
        var jobName = job == null ? "unknown" : $"{job.Type.FullName}.{job.Method.Name}";
        var queue = context.GetJobParameter<string>("Queue");

        if (context.NewState is FailedState failedState)
        {
            var retryCount = context.GetJobParameter<int>("RetryCount");

            _logger.LogError(
                failedState.Exception,
                AppLocalizer.Get(LocalizationKeys.HangfireJobFailed, jobId, jobName, retryCount, failedState.Reason),
                jobId,
                jobName,
                retryCount,
                failedState.Reason);

            if (_options.EnableFailureSqlLog)
            {
                TryPersistFailureLog(jobId, jobName, "Failed", failedState.Reason, failedState.Exception, queue, retryCount);
            }

            if (IsCriticalJob(jobName) && retryCount >= _options.FinalRetryCountThreshold)
            {
                var payload = new HangfireDeadLetterPayload
                {
                    JobId = jobId,
                    JobName = jobName,
                    Queue = "dead-letter",
                    RetryCount = retryCount,
                    Reason = failedState.Reason,
                    ExceptionType = failedState.Exception?.GetType().FullName,
                    ExceptionMessage = failedState.Exception?.Message,
                    OccurredAtUtc = DateTime.UtcNow
                };

                _backgroundJobClient.Create<IHangfireDeadLetterJob>(
                    x => x.ProcessAsync(payload),
                    new EnqueuedState("dead-letter"));
            }
        }
        else if (context.NewState is SucceededState succeededState)
        {
            _logger.LogInformation(
                AppLocalizer.Get(LocalizationKeys.HangfireJobSucceeded, jobId, jobName, succeededState.Latency, succeededState.PerformanceDuration),
                jobId,
                jobName,
                succeededState.Latency,
                succeededState.PerformanceDuration);
        }
        else if (context.NewState is EnqueuedState enqueuedState && string.Equals(enqueuedState.Queue, "dead-letter", StringComparison.OrdinalIgnoreCase))
        {
            TryPersistFailureLog(jobId, jobName, "Enqueued", AppLocalizer.Get(LocalizationKeys.HangfireMovedToDeadLetter), null, enqueuedState.Queue, context.GetJobParameter<int>("RetryCount"));
        }
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
    }

    private void TryPersistFailureLog(string jobId, string jobName, string state, string? reason, Exception? exception, string? queue, int retryCount)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<UtsDbContext>();

            dbContext.HangfireJobLogs.Add(new HangfireJobLog
            {
                JobId = jobId,
                JobName = jobName,
                State = state,
                OccurredAtUtc = DateTime.UtcNow,
                Reason = reason,
                ExceptionType = exception?.GetType().FullName,
                ExceptionMessage = exception?.Message,
                StackTrace = exception?.StackTrace?.Length > 8000 ? exception.StackTrace[..8000] : exception?.StackTrace,
                Queue = queue,
                RetryCount = retryCount
            });

            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, AppLocalizer.Get(LocalizationKeys.HangfireSqlLogFailed, jobId), jobId);
        }
    }

    private bool IsCriticalJob(string jobName)
    {
        if (_options.CriticalJobs.Count == 0)
        {
            return false;
        }

        return _options.CriticalJobs.Any(pattern =>
            !string.IsNullOrWhiteSpace(pattern) &&
            jobName.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }
}
