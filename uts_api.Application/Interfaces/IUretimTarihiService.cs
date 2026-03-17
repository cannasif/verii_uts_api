using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UretimTarihi;

namespace uts_api.Application.Interfaces;

public interface IUretimTarihiService
{
    Task<PagedResult<UretimTarihiListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<UretimTarihiDetailDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<UretimTarihiDetailDto> CreateAsync(CreateUretimTarihiRequestDto request, CancellationToken cancellationToken = default);
    Task<UretimTarihiDetailDto> UpdateAsync(long id, UpdateUretimTarihiRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
