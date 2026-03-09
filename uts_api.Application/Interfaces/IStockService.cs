using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.Stocks;

namespace uts_api.Application.Interfaces;

public interface IStockService
{
    Task<PagedResult<StockListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
}
