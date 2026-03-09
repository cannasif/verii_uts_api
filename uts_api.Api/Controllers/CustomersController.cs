using Hangfire;
using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.Customers;
using uts_api.Application.Interfaces;
using uts_api.Infrastructure.Hangfire;

namespace uts_api.Api.Controllers;

[Route("api/customers")]
public sealed class CustomersController : BaseApiController
{
    private readonly ICustomerService _customerService;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public CustomersController(ICustomerService customerService, IBackgroundJobClient backgroundJobClient)
    {
        _customerService = customerService;
        _backgroundJobClient = backgroundJobClient;
    }

    [PermissionAuthorize(PermissionConstants.Customers.View)]
    [HttpGet]
    public async Task<ActionResult<PagedApiResponse<CustomerListItemDto>>> GetAll([FromQuery] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _customerService.GetPagedAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.Customers.View)]
    [HttpPost("search")]
    public async Task<ActionResult<PagedApiResponse<CustomerListItemDto>>> Search([FromBody] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _customerService.GetPagedAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.Customers.Sync)]
    [HttpPost("sync")]
    public ActionResult<ApiResponse<CustomerSyncTriggerResponseDto>> TriggerSync()
    {
        var jobId = _backgroundJobClient.Enqueue<ICustomerSyncJob>(x => x.ExecuteAsync());
        return OkResponse(new CustomerSyncTriggerResponseDto
        {
            JobId = jobId,
            Queue = "default",
            EnqueuedAtUtc = DateTime.UtcNow
        }, LocalizationKeys.Success);
    }
}
