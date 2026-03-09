using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.Stocks;
using uts_api.Application.Interfaces;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class StockService : IStockService
{
    private static readonly IReadOnlyDictionary<string, string> AllowedColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["id"] = "Id",
        ["erpStockCode"] = "ErpStockCode",
        ["stockName"] = "StockName",
        ["unit"] = "Unit",
        ["grupKodu"] = "GrupKodu",
        ["grupAdi"] = "GrupAdi",
        ["branchCode"] = "BranchCode",
        ["createdAtUtc"] = "CreatedAtUtc"
    };

    private readonly IApplicationDbContext _dbContext;

    public StockService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<StockListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Stocks
            .AsNoTracking()
            .ApplySearch(request.Search, "ErpStockCode", "StockName", "GrupKodu", "GrupAdi", "UreticiKodu")
            .ApplyFilters(request.Filters, AllowedColumns, request.FilterLogic)
            .ApplySorting(request.SortBy, request.SortDirection, AllowedColumns)
            .Select(x => new StockListItemDto
            {
                Id = x.Id,
                ErpStockCode = x.ErpStockCode,
                StockName = x.StockName,
                Unit = x.Unit,
                GrupKodu = x.GrupKodu,
                GrupAdi = x.GrupAdi,
                BranchCode = x.BranchCode,
                CreatedAtUtc = x.CreatedAtUtc
            });

        return await query.ToPagedResultAsync(request, cancellationToken);
    }
}
