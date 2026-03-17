using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsUretimList;
using uts_api.Application.Interfaces;
using uts_api.Domain.Entities;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class UtsUretimListService : IUtsUretimListService
{
    private static readonly IReadOnlyDictionary<string, string> AllowedColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["chk"] = "Chk",
        ["siraNo"] = "SiraNo",
        ["bno"] = "Bno",
        ["sira"] = "Sira",
        ["git"] = "Git",
        ["uno"] = "Uno",
        ["lsNo"] = "LsNo",
        ["adt"] = "Adt",
        ["sinif"] = "Sinif",
        ["seriMiLotMu"] = "SeriMiLotMu",
        ["stokKodu"] = "StokKodu",
        ["stokAdi"] = "StokAdi",
        ["urt"] = "Urt",
        ["skt"] = "Skt",
        ["utsDurum"] = "UtsDurum"
    };

    private readonly UtsDbContext _dbContext;

    public UtsUretimListService(UtsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<UtsUretimListItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<UtsUretimListItem>()
            .AsNoTracking()
            .Select(x => new UtsUretimListItemDto
            {
                Chk = x.Chk,
                SiraNo = x.SiraNo,
                Bno = x.Bno,
                Sira = x.Sira,
                Git = x.Git,
                Uno = x.Uno,
                LsNo = x.LsNo,
                Adt = x.Adt,
                Sinif = x.Sinif,
                SeriMiLotMu = x.SeriMiLotMu,
                StokKodu = x.StokKodu,
                StokAdi = x.StokAdi,
                Urt = x.Urt,
                Skt = x.Skt,
                UtsDurum = x.UtsDurum
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<UtsUretimListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<UtsUretimListItem>()
            .AsNoTracking()
            .ApplySearch(request.Search, "Bno", "Git", "Uno", "LsNo", "Sinif", "StokKodu", "StokAdi", "Urt", "Skt", "UtsDurum")
            .ApplyFilters(request.Filters, AllowedColumns, request.FilterLogic)
            .ApplySorting(request.SortBy, request.SortDirection, AllowedColumns)
            .Select(x => new UtsUretimListItemDto
            {
                Chk = x.Chk,
                SiraNo = x.SiraNo,
                Bno = x.Bno,
                Sira = x.Sira,
                Git = x.Git,
                Uno = x.Uno,
                LsNo = x.LsNo,
                Adt = x.Adt,
                Sinif = x.Sinif,
                SeriMiLotMu = x.SeriMiLotMu,
                StokKodu = x.StokKodu,
                StokAdi = x.StokAdi,
                Urt = x.Urt,
                Skt = x.Skt,
                UtsDurum = x.UtsDurum
            });

        return await query.ToPagedResultAsync(request, cancellationToken);
    }
}
