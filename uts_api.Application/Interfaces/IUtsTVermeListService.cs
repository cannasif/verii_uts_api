using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsTVermeList;

namespace uts_api.Application.Interfaces;

public interface IUtsTVermeListService
{
    Task<IReadOnlyCollection<UtsTVermeListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<UtsTVermeListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
}
