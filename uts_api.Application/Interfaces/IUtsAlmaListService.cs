using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsAlmaList;

namespace uts_api.Application.Interfaces;

public interface IUtsAlmaListService
{
    Task<IReadOnlyCollection<UtsAlmaListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<UtsAlmaListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
}
