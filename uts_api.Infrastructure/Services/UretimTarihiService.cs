using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Exceptions;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UretimTarihi;
using uts_api.Application.Interfaces;
using uts_api.Domain.Entities;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class UretimTarihiService : IUretimTarihiService
{
    private static readonly IReadOnlyDictionary<string, string> AllowedColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["id"] = "Id",
        ["wfState"] = "WfState",
        ["stokKodu"] = "StokKodu",
        ["seriLotNo"] = "SeriLotNo",
        ["tarih"] = "Tarih",
        ["lotNo"] = "LotNo",
        ["sonKulTarih"] = "SonKulTarih",
        ["sYedek1"] = "SYedek1",
        ["sYedek2"] = "SYedek2",
        ["dYedek1"] = "DYedek1",
        ["iYedek1"] = "IYedek1",
        ["iYedek2"] = "IYedek2",
        ["fYedek1"] = "FYedek1",
        ["fYedek2"] = "FYedek2",
        ["createdAtUtc"] = "CreatedAtUtc"
    };

    private readonly IApplicationDbContext _dbContext;

    public UretimTarihiService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<UretimTarihiListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.UretimTarihleri
            .AsNoTracking()
            .ApplySearch(request.Search, "StokKodu", "SeriLotNo", "LotNo", "SYedek1", "SYedek2")
            .ApplyFilters(request.Filters, AllowedColumns, request.FilterLogic)
            .ApplySorting(request.SortBy, request.SortDirection, AllowedColumns)
            .Select(x => new UretimTarihiListItemDto
            {
                Id = x.Id,
                WfState = x.WfState,
                StokKodu = x.StokKodu,
                SeriLotNo = x.SeriLotNo,
                Tarih = x.Tarih,
                LotNo = x.LotNo,
                SonKulTarih = x.SonKulTarih,
                SYedek1 = x.SYedek1,
                SYedek2 = x.SYedek2,
                DYedek1 = x.DYedek1,
                IYedek1 = x.IYedek1,
                IYedek2 = x.IYedek2,
                FYedek1 = x.FYedek1,
                FYedek2 = x.FYedek2,
                CreatedAtUtc = x.CreatedAtUtc
            });

        return await query.ToPagedResultAsync(request, cancellationToken);
    }

    public async Task<UretimTarihiDetailDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UretimTarihleri
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new UretimTarihiDetailDto
            {
                Id = x.Id,
                WfState = x.WfState,
                StokKodu = x.StokKodu,
                SeriLotNo = x.SeriLotNo,
                Tarih = x.Tarih,
                LotNo = x.LotNo,
                SonKulTarih = x.SonKulTarih,
                SYedek1 = x.SYedek1,
                SYedek2 = x.SYedek2,
                DYedek1 = x.DYedek1,
                IYedek1 = x.IYedek1,
                IYedek2 = x.IYedek2,
                FYedek1 = x.FYedek1,
                FYedek2 = x.FYedek2,
                CreateUser = x.CreateUser,
                CreatedAtUtc = x.CreatedAtUtc,
                UpdateUser = x.UpdateUser,
                UpdatedAtUtc = x.UpdatedAtUtc
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new AppException(LocalizationKeys.UretimTarihiNotFound, 404);
    }

    public async Task<UretimTarihiDetailDto> CreateAsync(CreateUretimTarihiRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = new UretimTarihi
        {
            WfState = request.WfState,
            StokKodu = request.StokKodu?.Trim(),
            SeriLotNo = request.SeriLotNo?.Trim(),
            Tarih = request.Tarih,
            LotNo = request.LotNo?.Trim(),
            SonKulTarih = request.SonKulTarih,
            SYedek1 = request.SYedek1?.Trim(),
            SYedek2 = request.SYedek2?.Trim(),
            DYedek1 = request.DYedek1,
            IYedek1 = request.IYedek1,
            IYedek2 = request.IYedek2,
            FYedek1 = request.FYedek1,
            FYedek2 = request.FYedek2
        };

        _dbContext.UretimTarihleri.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(entity.Id, cancellationToken);
    }

    public async Task<UretimTarihiDetailDto> UpdateAsync(long id, UpdateUretimTarihiRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.UretimTarihleri.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new AppException(LocalizationKeys.UretimTarihiNotFound, 404);

        entity.WfState = request.WfState;
        entity.StokKodu = request.StokKodu?.Trim();
        entity.SeriLotNo = request.SeriLotNo?.Trim();
        entity.Tarih = request.Tarih;
        entity.LotNo = request.LotNo?.Trim();
        entity.SonKulTarih = request.SonKulTarih;
        entity.SYedek1 = request.SYedek1?.Trim();
        entity.SYedek2 = request.SYedek2?.Trim();
        entity.DYedek1 = request.DYedek1;
        entity.IYedek1 = request.IYedek1;
        entity.IYedek2 = request.IYedek2;
        entity.FYedek1 = request.FYedek1;
        entity.FYedek2 = request.FYedek2;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.UretimTarihleri.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new AppException(LocalizationKeys.UretimTarihiNotFound, 404);

        _dbContext.UretimTarihleri.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
