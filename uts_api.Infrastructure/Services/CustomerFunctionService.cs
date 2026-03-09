using Microsoft.EntityFrameworkCore;
using uts_api.Application.DTOs.Customers;
using uts_api.Application.Interfaces;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class CustomerFunctionService : ICustomerFunctionService
{
    private readonly UtsDbContext _dbContext;

    public CustomerFunctionService(UtsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<CustomerFunctionDto>> GetCustomersAsync(string? customerCode = null, short? branchCode = null, CancellationToken cancellationToken = default)
    {
        var rows = await _dbContext.Set<RiiFnCariRow>()
            .FromSqlRaw(
                "SELECT * FROM dbo.RII_FN_CARI({0}, {1})",
                string.IsNullOrWhiteSpace(customerCode) ? DBNull.Value : customerCode,
                branchCode.HasValue ? branchCode.Value : DBNull.Value)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return rows.Select(x => new CustomerFunctionDto
        {
            SubeKodu = x.SubeKodu,
            IsletmeKodu = x.IsletmeKodu,
            CariKod = x.CariKod,
            CariIsim = x.CariIsim,
            CariTel = x.CariTel,
            CariIl = x.CariIl,
            CariAdres = x.CariAdres,
            CariIlce = x.CariIlce,
            UlkeKodu = x.UlkeKodu,
            Email = x.Email,
            Web = x.Web,
            VergiNumarasi = x.VergiNumarasi,
            VergiDairesi = x.VergiDairesi,
            TcknNumber = x.TcknNumber
        }).ToList();
    }
}
