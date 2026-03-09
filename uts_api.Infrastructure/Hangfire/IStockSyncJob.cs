namespace uts_api.Infrastructure.Hangfire;

public interface IStockSyncJob
{
    Task ExecuteAsync();
}
