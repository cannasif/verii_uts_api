using Microsoft.Extensions.Logging;
using uts_api.Application.Common.Localization;

namespace uts_api.Infrastructure.Hangfire;

public sealed class HangfireDeadLetterJob : IHangfireDeadLetterJob
{
    private readonly ILogger<HangfireDeadLetterJob> _logger;

    public HangfireDeadLetterJob(ILogger<HangfireDeadLetterJob> logger)
    {
        _logger = logger;
    }

    public Task ProcessAsync(HangfireDeadLetterPayload payload)
    {
        _logger.LogError(
            AppLocalizer.Get(
                LocalizationKeys.HangfireDeadLetterCaptured,
                payload.JobId ?? string.Empty,
                payload.JobName ?? string.Empty,
                payload.Queue ?? string.Empty,
                payload.RetryCount,
                payload.Reason ?? string.Empty,
                payload.ExceptionType ?? string.Empty,
                payload.ExceptionMessage ?? string.Empty),
            payload.JobId,
            payload.JobName,
            payload.Queue,
            payload.RetryCount,
            payload.Reason,
            payload.ExceptionType,
            payload.ExceptionMessage);

        return Task.CompletedTask;
    }
}
