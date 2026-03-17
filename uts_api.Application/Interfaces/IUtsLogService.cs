using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsLogs;

namespace uts_api.Application.Interfaces;

public interface IUtsLogService
{
    Task<PagedResult<UtsLogListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<UtsLogDetailDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<UtsLogDetailDto> CreateAsync(CreateUtsLogRequestDto request, CancellationToken cancellationToken = default);
    Task<UtsLogDetailDto> UpdateAsync(long id, UpdateUtsLogRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
