using Hangfire;
using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.Stocks;
using uts_api.Application.Interfaces;
using uts_api.Infrastructure.Hangfire;

namespace uts_api.Api.Controllers;

[Route("api/stocks")]
public sealed class StocksController : BaseApiController
{
    private readonly IStockService _stockService;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public StocksController(IStockService stockService, IBackgroundJobClient backgroundJobClient)
    {
        _stockService = stockService;
        _backgroundJobClient = backgroundJobClient;
    }

    [PermissionAuthorize(PermissionConstants.Stocks.View)]
    [HttpGet]
    public async Task<ActionResult<PagedApiResponse<StockListItemDto>>> GetAll([FromQuery] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _stockService.GetPagedAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.Stocks.View)]
    [HttpPost("search")]
    public async Task<ActionResult<PagedApiResponse<StockListItemDto>>> Search([FromBody] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _stockService.GetPagedAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.Stocks.Sync)]
    [HttpPost("sync")]
    public ActionResult<ApiResponse<StockSyncTriggerResponseDto>> TriggerSync()
    {
        var jobId = _backgroundJobClient.Enqueue<IStockSyncJob>(x => x.ExecuteAsync());
        return OkResponse(new StockSyncTriggerResponseDto
        {
            JobId = jobId,
            Queue = "default",
            EnqueuedAtUtc = DateTime.UtcNow
        }, LocalizationKeys.Success);
    }
}
