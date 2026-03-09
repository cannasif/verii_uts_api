using uts_api.Application.DTOs.Smtp;

namespace uts_api.Application.Interfaces;

public interface ISmtpSettingService
{
    Task<SmtpSettingDto?> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<SmtpSettingDto> UpsertAsync(SmtpSettingUpsertRequestDto request, CancellationToken cancellationToken = default);
}
