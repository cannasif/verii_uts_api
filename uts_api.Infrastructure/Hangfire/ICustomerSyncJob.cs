namespace uts_api.Infrastructure.Hangfire;

public interface ICustomerSyncJob
{
    Task ExecuteAsync();
}
