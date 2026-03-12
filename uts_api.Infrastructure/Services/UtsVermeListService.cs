using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsVermeList;
using uts_api.Application.Interfaces;
using uts_api.Domain.Entities;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class UtsVermeListService : IUtsVermeListService
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
        ["utsDurum"] = "UtsDurum",
        ["uretimLsNo"] = "UretimLsNo",
        ["utrh"] = "Utrh",
        ["strh"] = "Strh",
        ["depoKod"] = "DepoKod",
        ["olcuBr"] = "OlcuBr",
        ["stharGcMik"] = "StharGcMik",
        ["straInc"] = "StraInc",
        ["imalIthal"] = "ImalIthal",
        ["uretimBildirimi"] = "UretimBildirimi"
    };

    private readonly UtsDbContext _dbContext;

    public UtsVermeListService(UtsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<UtsVermeListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<UtsVermeListItem>()
            .AsNoTracking()
            .ApplySearch(
                request.Search,
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
                "UtsDurum",
                "UretimLsNo",
                "Utrh",
                "Strh",
                "ImalIthal",
                "UretimBildirimi")
            .ApplyFilters(request.Filters, AllowedColumns, request.FilterLogic)
            .ApplySorting(request.SortBy, request.SortDirection, AllowedColumns)
            .Select(x => new UtsVermeListItemDto
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
                UtsDurum = x.UtsDurum,
                UretimLsNo = x.UretimLsNo,
                Utrh = x.Utrh,
                Strh = x.Strh,
                DepoKod = x.DepoKod,
                OlcuBr = x.OlcuBr,
                StharGcMik = x.StharGcMik,
                StraInc = x.StraInc,
                ImalIthal = x.ImalIthal,
                UretimBildirimi = x.UretimBildirimi
            });

        return await query.ToPagedResultAsync(request, cancellationToken);
    }
}
