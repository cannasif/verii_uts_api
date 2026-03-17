using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsAlmaList;
using uts_api.Application.Interfaces;
using uts_api.Domain.Entities;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class UtsAlmaListService : IUtsAlmaListService
{
    private static readonly IReadOnlyDictionary<string, string> AllowedColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["chk"] = "Chk",
        ["siraNo"] = "SiraNo",
        ["bno"] = "Bno",
        ["sira"] = "Sira",
        ["git"] = "Git",
        ["kun"] = "Kun",
        ["uno"] = "Uno",
        ["lsNo"] = "LsNo",
        ["adt"] = "Adt",
        ["sinif"] = "Sinif",
        ["seriMiLotMu"] = "SeriMiLotMu",
        ["cariKodu"] = "CariKodu",
        ["cariIsim"] = "CariIsim",
        ["stokKodu"] = "StokKodu",
        ["stokAdi"] = "StokAdi",
        ["acik16"] = "Acik16",
        ["utsDurum"] = "UtsDurum",
    };

    private readonly UtsDbContext _dbContext;

    public UtsAlmaListService(UtsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<UtsAlmaListItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<UtsAlmaListItem>()
            .AsNoTracking()
            .Select(x => new UtsAlmaListItemDto
            {
                Chk = x.Chk,
                SiraNo = x.SiraNo,
                Bno = x.Bno,
                Sira = x.Sira,
                Git = x.Git,
                Kun = x.Kun,
                Uno = x.Uno,
                LsNo = x.LsNo,
                Adt = x.Adt,
                Sinif = x.Sinif,
                SeriMiLotMu = x.SeriMiLotMu,
                CariKodu = x.CariKodu,
                CariIsim = x.CariIsim,
                StokKodu = x.StokKodu,
                StokAdi = x.StokAdi,
                Acik16 = x.Acik16,
                UtsDurum = x.UtsDurum
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<UtsAlmaListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var baseQuery = _dbContext.Set<UtsAlmaListItem>()
            .AsNoTracking()
            .ApplySearch(
                request.Search,
                "Chk",
                "Bno",
                "Git",
                "Kun",
                "Uno",
                "LsNo",
                "Sinif",
                "CariKodu",
                "CariIsim",
                "StokKodu",
                "StokAdi",
                "Acik16",
                "UtsDurum")
            .ApplyFilters(request.Filters, AllowedColumns, request.FilterLogic)
            .ApplySorting(request.SortBy, request.SortDirection, AllowedColumns);

        return await baseQuery
            .Select(x => new UtsAlmaListItemDto
            {
                Chk = x.Chk,
                SiraNo = x.SiraNo,
                Bno = x.Bno,
                Sira = x.Sira,
                Git = x.Git,
                Kun = x.Kun,
                Uno = x.Uno,
                LsNo = x.LsNo,
                Adt = x.Adt,
                Sinif = x.Sinif,
                SeriMiLotMu = x.SeriMiLotMu,
                CariKodu = x.CariKodu,
                CariIsim = x.CariIsim,
                StokKodu = x.StokKodu,
                StokAdi = x.StokAdi,
                Acik16 = x.Acik16,
                UtsDurum = x.UtsDurum
            })
            .ToPagedResultAsync(request, cancellationToken);
    }
}
