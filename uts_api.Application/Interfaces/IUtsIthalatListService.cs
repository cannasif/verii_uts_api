using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsIthalatList;

namespace uts_api.Application.Interfaces;

public interface IUtsIthalatListService
{
    Task<IReadOnlyCollection<UtsIthalatListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<UtsIthalatListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
}
