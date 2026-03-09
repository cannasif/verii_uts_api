namespace uts_api.Application.DTOs.Smtp;

public sealed class SmtpSettingUpsertRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string FromEmail { get; set; } = string.Empty;
    public string? FromName { get; set; }
    public bool EnableSsl { get; set; }
    public bool IsActive { get; set; } = true;
}
