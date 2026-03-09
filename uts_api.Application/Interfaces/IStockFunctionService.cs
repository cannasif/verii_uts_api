using uts_api.Application.DTOs.Stocks;

namespace uts_api.Application.Interfaces;

public interface IStockFunctionService
{
    Task<IReadOnlyList<StockFunctionDto>> GetStocksAsync(string? stockCode = null, short? branchCode = null, CancellationToken cancellationToken = default);
}
