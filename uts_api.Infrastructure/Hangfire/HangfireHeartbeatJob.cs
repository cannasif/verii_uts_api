using Microsoft.Extensions.Logging;
using uts_api.Application.Common.Localization;

namespace uts_api.Infrastructure.Hangfire;

public sealed class HangfireHeartbeatJob : IHangfireHeartbeatJob
{
    private readonly ILogger<HangfireHeartbeatJob> _logger;

    public HangfireHeartbeatJob(ILogger<HangfireHeartbeatJob> logger)
    {
        _logger = logger;
    }

    public Task ExecuteAsync()
    {
        _logger.LogInformation(AppLocalizer.Get(LocalizationKeys.HangfireHeartbeatExecuted, DateTime.UtcNow), DateTime.UtcNow);
        return Task.CompletedTask;
    }
}
