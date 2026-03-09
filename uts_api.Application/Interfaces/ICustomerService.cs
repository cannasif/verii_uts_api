using uts_api.Application.Common.Models;
using uts_api.Application.DTOs.Customers;

namespace uts_api.Application.Interfaces;

public interface ICustomerService
{
    Task<PagedResult<CustomerListItemDto>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
}
