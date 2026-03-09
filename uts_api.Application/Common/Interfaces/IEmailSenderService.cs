namespace uts_api.Application.Common.Interfaces;

public interface IEmailSenderService
{
    Task<bool> SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default);
}
