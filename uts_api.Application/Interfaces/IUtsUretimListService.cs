using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsUretimList;

namespace uts_api.Application.Interfaces;

public interface IUtsUretimListService
{
    Task<IReadOnlyCollection<UtsUretimListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<UtsUretimListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
}
