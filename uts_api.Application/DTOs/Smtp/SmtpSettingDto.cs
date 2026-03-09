namespace uts_api.Application.DTOs.Smtp;

public sealed class SmtpSettingDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
    public string? UserName { get; init; }
    public string FromEmail { get; init; } = string.Empty;
    public string? FromName { get; init; }
    public bool EnableSsl { get; init; }
    public bool IsActive { get; init; }
}
