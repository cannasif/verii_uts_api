using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsIhracatList;

namespace uts_api.Application.Interfaces;

public interface IUtsIhracatListService
{
    Task<IReadOnlyCollection<UtsIhracatListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<UtsIhracatListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
}
