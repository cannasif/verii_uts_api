using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsTVermeList;
using uts_api.Application.Interfaces;
using uts_api.Domain.Entities;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class UtsTVermeListService : IUtsTVermeListService
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
        ["vkn"] = "Vkn",
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
        ["depoKod"] = "DepoKod",
        ["olcuBr"] = "OlcuBr",
        ["stharGcMik"] = "StharGcMik",
        ["straInc"] = "StraInc",
        ["imalIthal"] = "ImalIthal",
        ["uretimBildirimi"] = "UretimBildirimi"
    };

    private readonly UtsDbContext _dbContext;

    public UtsTVermeListService(UtsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<UtsTVermeListItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<UtsTVermeListItem>()
            .AsNoTracking()
            .Select(x => new UtsTVermeListItemDto
            {
                Chk = x.Chk,
                SiraNo = x.SiraNo,
                Bno = x.Bno,
                Sira = x.Sira,
                Git = x.Git,
                Kun = x.Kun,
                Uno = x.Uno,
                Vkn = x.Vkn,
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
                DepoKod = x.DepoKod,
                OlcuBr = x.OlcuBr,
                StharGcMik = x.StharGcMik,
                StraInc = x.StraInc,
                ImalIthal = x.ImalIthal,
                UretimBildirimi = x.UretimBildirimi
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<UtsTVermeListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<UtsTVermeListItem>()
            .AsNoTracking()
            .ApplySearch(
                request.Search,
                "Chk",
                "Bno",
                "Git",
                "Kun",
                "Uno",
                "Vkn",
                "LsNo",
                "Sinif",
                "CariKodu",
                "CariIsim",
                "StokKodu",
                "StokAdi",
                "UtsDurum",
                "UretimLsNo",
                "ImalIthal",
                "UretimBildirimi")
            .ApplyFilters(request.Filters, AllowedColumns, request.FilterLogic)
            .ApplySorting(request.SortBy, request.SortDirection, AllowedColumns)
            .Select(x => new UtsTVermeListItemDto
            {
                Chk = x.Chk,
                SiraNo = x.SiraNo,
                Bno = x.Bno,
                Sira = x.Sira,
                Git = x.Git,
                Kun = x.Kun,
                Uno = x.Uno,
                Vkn = x.Vkn,
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
