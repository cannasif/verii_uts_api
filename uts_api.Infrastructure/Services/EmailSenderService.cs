using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Interfaces;

namespace uts_api.Infrastructure.Services;

public sealed class EmailSenderService : IEmailSenderService
{
    private readonly IApplicationDbContext _dbContext;

    public EmailSenderService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        var smtp = await _dbContext.SmtpSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsActive, cancellationToken);

        if (smtp is null)
        {
            return false;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(smtp.FromEmail, smtp.FromName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        message.To.Add(toEmail);

        using var client = new SmtpClient(smtp.Host, smtp.Port)
        {
            EnableSsl = smtp.EnableSsl
        };

        if (!string.IsNullOrWhiteSpace(smtp.UserName))
        {
            client.Credentials = new NetworkCredential(smtp.UserName, smtp.Password);
        }

        await client.SendMailAsync(message, cancellationToken);
        return true;
    }
}
