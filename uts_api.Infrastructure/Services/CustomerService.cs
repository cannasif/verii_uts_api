using Microsoft.EntityFrameworkCore;
using uts_api.Application.Common.Interfaces;
using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.Customers;
using uts_api.Application.Interfaces;
using uts_api.Infrastructure.Persistence;

namespace uts_api.Infrastructure.Services;

public sealed class CustomerService : ICustomerService
{
    private static readonly IReadOnlyDictionary<string, string> AllowedColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["id"] = "Id",
        ["customerCode"] = "CustomerCode",
        ["customerName"] = "CustomerName",
        ["taxNumber"] = "TaxNumber",
        ["email"] = "Email",
        ["phone"] = "Phone",
        ["city"] = "City",
        ["branchCode"] = "BranchCode",
        ["isErpIntegrated"] = "IsErpIntegrated",
        ["lastSyncDateUtc"] = "LastSyncDateUtc",
        ["createdAtUtc"] = "CreatedAtUtc"
    };

    private readonly IApplicationDbContext _dbContext;

    public CustomerService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<CustomerListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Customers
            .AsNoTracking()
            .ApplySearch(request.Search, "CustomerCode", "CustomerName", "TaxNumber", "TcknNumber", "Email", "Phone", "City")
            .ApplyFilters(request.Filters, AllowedColumns, request.FilterLogic)
            .ApplySorting(request.SortBy, request.SortDirection, AllowedColumns)
            .Select(x => new CustomerListItemDto
            {
                Id = x.Id,
                CustomerCode = x.CustomerCode,
                CustomerName = x.CustomerName,
                TaxNumber = x.TaxNumber,
                TcknNumber = x.TcknNumber,
                Email = x.Email,
                Phone = x.Phone,
                City = x.City,
                BranchCode = x.BranchCode,
                IsErpIntegrated = x.IsErpIntegrated,
                LastSyncDateUtc = x.LastSyncDateUtc,
                CreatedAtUtc = x.CreatedAtUtc
            });

        return await query.ToPagedResultAsync(request, cancellationToken);
    }
}
