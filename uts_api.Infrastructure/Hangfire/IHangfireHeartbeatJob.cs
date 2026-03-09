namespace uts_api.Infrastructure.Hangfire;

public interface IHangfireHeartbeatJob
{
    Task ExecuteAsync();
}
