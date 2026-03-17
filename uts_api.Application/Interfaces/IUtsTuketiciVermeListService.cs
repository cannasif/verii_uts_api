using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsTuketiciVermeList;

namespace uts_api.Application.Interfaces;

public interface IUtsTuketiciVermeListService
{
    Task<IReadOnlyCollection<UtsTuketiciVermeListItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<UtsTuketiciVermeListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
}
