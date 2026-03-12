using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsVermeList;

namespace uts_api.Application.Interfaces;

public interface IUtsVermeListService
{
    Task<PagedResult<UtsVermeListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
}
