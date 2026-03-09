using uts_api.Application.DTOs.Customers;

namespace uts_api.Application.Interfaces;

public interface ICustomerFunctionService
{
    Task<IReadOnlyList<CustomerFunctionDto>> GetCustomersAsync(string? customerCode = null, short? branchCode = null, CancellationToken cancellationToken = default);
}
