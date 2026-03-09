using Microsoft.EntityFrameworkCore;
using uts_api.Application.DTOs.Stocks;
using uts_api.Application.Interfaces;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class StockFunctionService : IStockFunctionService
{
    private readonly UtsDbContext _dbContext;

    public StockFunctionService(UtsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<StockFunctionDto>> GetStocksAsync(string? stockCode = null, short? branchCode = null, CancellationToken cancellationToken = default)
    {
        var rows = await _dbContext.Set<RiiFnStockRow>()
            .FromSqlRaw(
                "SELECT * FROM dbo.RII_FN_STOK({0}, {1})",
                string.IsNullOrWhiteSpace(stockCode) ? DBNull.Value : stockCode,
                branchCode.HasValue ? branchCode.Value : DBNull.Value)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return rows.Select(x => new StockFunctionDto
        {
            SubeKodu = x.SubeKodu,
            IsletmeKodu = x.IsletmeKodu,
            StokKodu = x.StokKodu,
            OlcuBr1 = x.OlcuBr1,
            UreticiKodu = x.UreticiKodu,
            StokAdi = x.StokAdi,
            GrupKodu = x.GrupKodu,
            GrupIsim = x.GrupIsim,
            Kod1 = x.Kod1,
            Kod1Adi = x.Kod1Adi,
            Kod2 = x.Kod2,
            Kod2Adi = x.Kod2Adi,
            Kod3 = x.Kod3,
            Kod3Adi = x.Kod3Adi,
            Kod4 = x.Kod4,
            Kod4Adi = x.Kod4Adi,
            Kod5 = x.Kod5,
            Kod5Adi = x.Kod5Adi,
            IngIsim = x.IngIsim
        }).ToList();
    }
}
