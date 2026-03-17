using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Exceptions;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.UtsLogs;
using uts_api.Application.Interfaces;
using uts_api.Domain.Entities;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class UtsLogService : IUtsLogService
{
    private static readonly IReadOnlyDictionary<string, string> AllowedColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["id"] = "Id",
        ["bno"] = "Bno",
        ["sira"] = "Sira",
        ["seriTraInc"] = "SeriTraInc",
        ["stokKodu"] = "StokKodu",
        ["seriNo"] = "SeriNo",
        ["miktar"] = "Miktar",
        ["gonderimTarihi"] = "GonderimTarihi",
        ["gonderenKisi"] = "GonderenKisi",
        ["gonderimTipi"] = "GonderimTipi",
        ["durum"] = "Durum",
        ["createdAtUtc"] = "CreatedAtUtc"
    };

    private readonly IApplicationDbContext _dbContext;

    public UtsLogService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<UtsLogListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.UtsLogs
            .AsNoTracking()
            .ApplySearch(request.Search, "Bno", "StokKodu", "SeriNo", "GonderenKisi", "GonderimTipi", "Durum", "Sonuc")
            .ApplyFilters(request.Filters, AllowedColumns, request.FilterLogic)
            .ApplySorting(request.SortBy, request.SortDirection, AllowedColumns)
            .Select(x => new UtsLogListItemDto
            {
                Id = x.Id,
                Bno = x.Bno,
                Sira = x.Sira,
                SeriTraInc = x.SeriTraInc,
                StokKodu = x.StokKodu,
                SeriNo = x.SeriNo,
                Miktar = x.Miktar,
                GonderimTarihi = x.GonderimTarihi,
                GonderenKisi = x.GonderenKisi,
                GonderimTipi = x.GonderimTipi,
                Sonuc = x.Sonuc,
                Durum = x.Durum,
                CreatedAtUtc = x.CreatedAtUtc
            });

        return await query.ToPagedResultAsync(request, cancellationToken);
    }

    public async Task<UtsLogDetailDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UtsLogs
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new UtsLogDetailDto
            {
                Id = x.Id,
                Bno = x.Bno,
                Sira = x.Sira,
                SeriTraInc = x.SeriTraInc,
                StokKodu = x.StokKodu,
                SeriNo = x.SeriNo,
                Miktar = x.Miktar,
                GonderimTarihi = x.GonderimTarihi,
                GonderenKisi = x.GonderenKisi,
                GonderimTipi = x.GonderimTipi,
                Sonuc = x.Sonuc,
                Durum = x.Durum,
                CreateUser = x.CreateUser,
                CreatedAtUtc = x.CreatedAtUtc,
                UpdateUser = x.UpdateUser,
                UpdatedAtUtc = x.UpdatedAtUtc
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new AppException(LocalizationKeys.UtsLogNotFound, 404);
    }

    public async Task<UtsLogDetailDto> CreateAsync(CreateUtsLogRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = new UtsLog
        {
            Bno = request.Bno.Trim(),
            Sira = request.Sira,
            SeriTraInc = request.SeriTraInc,
            StokKodu = request.StokKodu?.Trim(),
            SeriNo = request.SeriNo?.Trim(),
            Miktar = request.Miktar,
            GonderimTarihi = request.GonderimTarihi,
            GonderenKisi = request.GonderenKisi?.Trim(),
            GonderimTipi = request.GonderimTipi?.Trim(),
            Sonuc = request.Sonuc,
            Durum = request.Durum?.Trim()
        };

        _dbContext.UtsLogs.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(entity.Id, cancellationToken);
    }

    public async Task<UtsLogDetailDto> UpdateAsync(long id, UpdateUtsLogRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.UtsLogs.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new AppException(LocalizationKeys.UtsLogNotFound, 404);

        entity.Bno = request.Bno.Trim();
        entity.Sira = request.Sira;
        entity.SeriTraInc = request.SeriTraInc;
        entity.StokKodu = request.StokKodu?.Trim();
        entity.SeriNo = request.SeriNo?.Trim();
        entity.Miktar = request.Miktar;
        entity.GonderimTarihi = request.GonderimTarihi;
        entity.GonderenKisi = request.GonderenKisi?.Trim();
        entity.GonderimTipi = request.GonderimTipi?.Trim();
        entity.Sonuc = request.Sonuc;
        entity.Durum = request.Durum?.Trim();

        await _dbContext.SaveChangesAsync(cancellationToken);
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.UtsLogs.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new AppException(LocalizationKeys.UtsLogNotFound, 404);

        _dbContext.UtsLogs.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
