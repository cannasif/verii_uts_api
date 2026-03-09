namespace uts_api.Infrastructure.Hangfire;

public interface IHangfireDeadLetterJob
{
    Task ProcessAsync(HangfireDeadLetterPayload payload);
}
