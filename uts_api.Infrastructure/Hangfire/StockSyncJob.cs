using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using uts_api.Application.Common.Localization;
using uts_api.Application.Interfaces;
using uts_api.Domain.Entities;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Hangfire;

[DisableConcurrentExecution(timeoutInSeconds: 300)]
[AutomaticRetry(Attempts = 3, DelaysInSeconds = [60, 120, 300])]
public sealed class StockSyncJob : IStockSyncJob
{
    private const string RecurringJobId = "rii-stock-sync-job";

    private readonly UtsDbContext _dbContext;
    private readonly IStockFunctionService _stockFunctionService;
    private readonly ILogger<StockSyncJob> _logger;

    public StockSyncJob(UtsDbContext dbContext, IStockFunctionService stockFunctionService, ILogger<StockSyncJob> logger)
    {
        _dbContext = dbContext;
        _stockFunctionService = stockFunctionService;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        _logger.LogInformation(AppLocalizer.Get(LocalizationKeys.StockSyncStarted, DateTime.UtcNow), DateTime.UtcNow);

        IReadOnlyList<Application.DTOs.Stocks.StockFunctionDto> functionRows;
        try
        {
            functionRows = await _stockFunctionService.GetStocksAsync();
        }
        catch (Exception ex)
        {
            await LogRecordFailureAsync("ERP_FETCH", ex);
            _logger.LogWarning(ex, AppLocalizer.Get(LocalizationKeys.StockSyncAborted));
            return;
        }

        if (functionRows.Count == 0)
        {
            _logger.LogInformation(AppLocalizer.Get(LocalizationKeys.StockSyncSkipped));
            return;
        }

        var createdCount = 0;
        var updatedCount = 0;
        var skippedCount = 0;
        var failedCount = 0;
        var duplicatePayloadCount = 0;
        var processedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in functionRows)
        {
            var code = row.StokKodu?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(code))
            {
                skippedCount++;
                continue;
            }

            if (!processedCodes.Add(code))
            {
                duplicatePayloadCount++;
                continue;
            }

            try
            {
                var stock = await _dbContext.Stocks
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(x => x.ErpStockCode == code);

                var stockName = string.IsNullOrWhiteSpace(row.StokAdi) ? code : row.StokAdi.Trim();
                var branchCode = (int)row.SubeKodu;

                if (stock is null)
                {
                    _dbContext.Stocks.Add(new Stock
                    {
                        ErpStockCode = code,
                        StockName = stockName,
                        Unit = row.OlcuBr1 ?? string.Empty,
                        UreticiKodu = row.UreticiKodu ?? string.Empty,
                        GrupKodu = row.GrupKodu ?? string.Empty,
                        GrupAdi = row.GrupIsim ?? string.Empty,
                        Kod1 = row.Kod1 ?? string.Empty,
                        Kod1Adi = row.Kod1Adi ?? string.Empty,
                        Kod2 = row.Kod2 ?? string.Empty,
                        Kod2Adi = row.Kod2Adi ?? string.Empty,
                        Kod3 = row.Kod3 ?? string.Empty,
                        Kod3Adi = row.Kod3Adi ?? string.Empty,
                        Kod4 = row.Kod4 ?? string.Empty,
                        Kod4Adi = row.Kod4Adi ?? string.Empty,
                        Kod5 = row.Kod5 ?? string.Empty,
                        Kod5Adi = row.Kod5Adi ?? string.Empty,
                        BranchCode = branchCode,
                        IsDeleted = false
                    });

                    await _dbContext.SaveChangesAsync();
                    createdCount++;
                    continue;
                }

                var updated = false;

                if (stock.StockName != stockName) { stock.StockName = stockName; updated = true; }
                if (stock.Unit != (row.OlcuBr1 ?? string.Empty)) { stock.Unit = row.OlcuBr1 ?? string.Empty; updated = true; }
                if (stock.UreticiKodu != (row.UreticiKodu ?? string.Empty)) { stock.UreticiKodu = row.UreticiKodu ?? string.Empty; updated = true; }
                if (stock.GrupKodu != (row.GrupKodu ?? string.Empty)) { stock.GrupKodu = row.GrupKodu ?? string.Empty; updated = true; }
                if (stock.GrupAdi != (row.GrupIsim ?? string.Empty)) { stock.GrupAdi = row.GrupIsim ?? string.Empty; updated = true; }
                if (stock.Kod1 != (row.Kod1 ?? string.Empty)) { stock.Kod1 = row.Kod1 ?? string.Empty; updated = true; }
                if (stock.Kod1Adi != (row.Kod1Adi ?? string.Empty)) { stock.Kod1Adi = row.Kod1Adi ?? string.Empty; updated = true; }
                if (stock.Kod2 != (row.Kod2 ?? string.Empty)) { stock.Kod2 = row.Kod2 ?? string.Empty; updated = true; }
                if (stock.Kod2Adi != (row.Kod2Adi ?? string.Empty)) { stock.Kod2Adi = row.Kod2Adi ?? string.Empty; updated = true; }
                if (stock.Kod3 != (row.Kod3 ?? string.Empty)) { stock.Kod3 = row.Kod3 ?? string.Empty; updated = true; }
                if (stock.Kod3Adi != (row.Kod3Adi ?? string.Empty)) { stock.Kod3Adi = row.Kod3Adi ?? string.Empty; updated = true; }
                if (stock.Kod4 != (row.Kod4 ?? string.Empty)) { stock.Kod4 = row.Kod4 ?? string.Empty; updated = true; }
                if (stock.Kod4Adi != (row.Kod4Adi ?? string.Empty)) { stock.Kod4Adi = row.Kod4Adi ?? string.Empty; updated = true; }
                if (stock.Kod5 != (row.Kod5 ?? string.Empty)) { stock.Kod5 = row.Kod5 ?? string.Empty; updated = true; }
                if (stock.Kod5Adi != (row.Kod5Adi ?? string.Empty)) { stock.Kod5Adi = row.Kod5Adi ?? string.Empty; updated = true; }

                if (stock.BranchCode != branchCode)
                {
                    stock.BranchCode = branchCode;
                    updated = true;
                }

                if (!updated)
                {
                    continue;
                }

                await _dbContext.SaveChangesAsync();
                updatedCount++;
            }
            catch (Exception ex)
            {
                failedCount++;
                await LogRecordFailureAsync(code, ex);
                _dbContext.ChangeTracker.Clear();
            }
        }

        _logger.LogInformation(
            AppLocalizer.Get(LocalizationKeys.StockSyncCompleted, createdCount, updatedCount, failedCount, skippedCount, duplicatePayloadCount),
            createdCount,
            updatedCount,
            failedCount,
            skippedCount,
            duplicatePayloadCount);
    }

    private async Task LogRecordFailureAsync(string code, Exception exception)
    {
        _logger.LogError(exception, AppLocalizer.Get(LocalizationKeys.StockSyncRecordFailed, code), code);

        try
        {
            _dbContext.HangfireJobLogs.Add(new HangfireJobLog
            {
                JobId = $"{RecurringJobId}:{code}:{DateTime.UtcNow:yyyyMMddHHmmssfff}",
                JobName = $"{typeof(StockSyncJob).FullName}.ExecuteAsync",
                State = "Failed",
                OccurredAtUtc = DateTime.UtcNow,
                Reason = $"StockCode={code}",
                ExceptionType = exception.GetType().FullName,
                ExceptionMessage = exception.Message,
                StackTrace = exception.StackTrace?.Length > 8000 ? exception.StackTrace[..8000] : exception.StackTrace,
                Queue = "default",
                RetryCount = 0
            });

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception logException)
        {
            _logger.LogWarning(logException, AppLocalizer.Get(LocalizationKeys.StockSyncFailureLogWriteFailed, code), code);
        }
    }

}
