using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsImhaList;

namespace uts_api.Application.Interfaces;

public interface IUtsImhaListService
{
    Task<IReadOnlyCollection<UtsImhaListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<UtsImhaListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
}
