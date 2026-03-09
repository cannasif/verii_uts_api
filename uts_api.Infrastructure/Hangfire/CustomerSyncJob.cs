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
public sealed class CustomerSyncJob : ICustomerSyncJob
{
    private const string RecurringJobId = "rii-customer-sync-job";

    private readonly UtsDbContext _dbContext;
    private readonly ICustomerFunctionService _customerFunctionService;
    private readonly ILogger<CustomerSyncJob> _logger;

    public CustomerSyncJob(UtsDbContext dbContext, ICustomerFunctionService customerFunctionService, ILogger<CustomerSyncJob> logger)
    {
        _dbContext = dbContext;
        _customerFunctionService = customerFunctionService;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        _logger.LogInformation(AppLocalizer.Get(LocalizationKeys.CustomerSyncStarted, DateTime.UtcNow), DateTime.UtcNow);

        IReadOnlyList<Application.DTOs.Customers.CustomerFunctionDto> functionRows;
        try
        {
            functionRows = await _customerFunctionService.GetCustomersAsync();
        }
        catch (Exception ex)
        {
            await LogRecordFailureAsync("ERP_FETCH", ex);
            _logger.LogWarning(ex, AppLocalizer.Get(LocalizationKeys.CustomerSyncAborted));
            return;
        }

        if (functionRows.Count == 0)
        {
            _logger.LogInformation(AppLocalizer.Get(LocalizationKeys.CustomerSyncSkipped));
            return;
        }

        var createdCount = 0;
        var updatedCount = 0;
        var reactivatedCount = 0;
        var skippedCount = 0;
        var failedCount = 0;
        var duplicatePayloadCount = 0;
        var processedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var row in functionRows)
        {
            var code = row.CariKod?.Trim() ?? string.Empty;
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
                var customer = await _dbContext.Customers
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(x => x.CustomerCode == code);

                var customerName = string.IsNullOrWhiteSpace(row.CariIsim) ? code : row.CariIsim.Trim();
                var branchCode = row.SubeKodu;
                var businessUnitCode = row.IsletmeKodu;
                var taxOffice = row.VergiDairesi ?? string.Empty;
                var taxNumber = row.VergiNumarasi ?? string.Empty;
                var tcknNumber = row.TcknNumber ?? string.Empty;
                var email = row.Email ?? string.Empty;
                var website = row.Web ?? string.Empty;
                var phone = row.CariTel ?? string.Empty;
                var address = row.CariAdres ?? string.Empty;
                var city = row.CariIl ?? string.Empty;
                var district = row.CariIlce ?? string.Empty;
                var countryCode = row.UlkeKodu ?? string.Empty;

                if (customer is null)
                {
                    _dbContext.Customers.Add(new Customer
                    {
                        CustomerCode = code,
                        CustomerName = customerName,
                        TaxOffice = taxOffice,
                        TaxNumber = taxNumber,
                        TcknNumber = tcknNumber,
                        Email = email,
                        Website = website,
                        Phone = phone,
                        Address = address,
                        City = city,
                        District = district,
                        CountryCode = countryCode,
                        BranchCode = branchCode,
                        BusinessUnitCode = businessUnitCode,
                        IsErpIntegrated = true,
                        ErpIntegrationNumber = code,
                        LastSyncDateUtc = DateTime.UtcNow,
                        IsDeleted = false
                    });

                    await _dbContext.SaveChangesAsync();
                    createdCount++;
                    continue;
                }

                var updated = false;
                var reactivated = false;

                if (customer.CustomerName != customerName) { customer.CustomerName = customerName; updated = true; }
                if (customer.TaxOffice != taxOffice) { customer.TaxOffice = taxOffice; updated = true; }
                if (customer.TaxNumber != taxNumber) { customer.TaxNumber = taxNumber; updated = true; }
                if (customer.TcknNumber != tcknNumber) { customer.TcknNumber = tcknNumber; updated = true; }
                if (customer.Email != email) { customer.Email = email; updated = true; }
                if (customer.Website != website) { customer.Website = website; updated = true; }
                if (customer.Phone != phone) { customer.Phone = phone; updated = true; }
                if (customer.Address != address) { customer.Address = address; updated = true; }
                if (customer.City != city) { customer.City = city; updated = true; }
                if (customer.District != district) { customer.District = district; updated = true; }
                if (customer.CountryCode != countryCode) { customer.CountryCode = countryCode; updated = true; }
                if (customer.BranchCode != branchCode) { customer.BranchCode = branchCode; updated = true; }
                if (customer.BusinessUnitCode != businessUnitCode) { customer.BusinessUnitCode = businessUnitCode; updated = true; }
                if (customer.IsErpIntegrated != true) { customer.IsErpIntegrated = true; updated = true; }
                if (customer.ErpIntegrationNumber != code) { customer.ErpIntegrationNumber = code; updated = true; }

                if (customer.IsDeleted)
                {
                    customer.IsDeleted = false;
                    customer.DeletedAtUtc = null;
                    customer.DeleteUser = null;
                    updated = true;
                    reactivated = true;
                }

                if (!updated)
                {
                    continue;
                }

                customer.LastSyncDateUtc = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                if (reactivated)
                {
                    reactivatedCount++;
                }
                else
                {
                    updatedCount++;
                }
            }
            catch (Exception ex)
            {
                failedCount++;
                await LogRecordFailureAsync(code, ex);
                _dbContext.ChangeTracker.Clear();
            }
        }

        _logger.LogInformation(
            AppLocalizer.Get(LocalizationKeys.CustomerSyncCompleted, createdCount, updatedCount, reactivatedCount, failedCount, skippedCount, duplicatePayloadCount),
            createdCount,
            updatedCount,
            reactivatedCount,
            failedCount,
            skippedCount,
            duplicatePayloadCount);
    }

    private async Task LogRecordFailureAsync(string code, Exception exception)
    {
        _logger.LogError(exception, AppLocalizer.Get(LocalizationKeys.CustomerSyncRecordFailed, code), code);

        try
        {
            _dbContext.HangfireJobLogs.Add(new HangfireJobLog
            {
                JobId = $"{RecurringJobId}:{code}:{DateTime.UtcNow:yyyyMMddHHmmssfff}",
                JobName = $"{typeof(CustomerSyncJob).FullName}.ExecuteAsync",
                State = "Failed",
                OccurredAtUtc = DateTime.UtcNow,
                Reason = $"CustomerCode={code}",
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
            _logger.LogWarning(logException, AppLocalizer.Get(LocalizationKeys.CustomerSyncFailureLogWriteFailed, code), code);
        }
    }
}
