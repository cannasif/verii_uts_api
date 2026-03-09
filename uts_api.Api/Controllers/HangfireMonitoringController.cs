using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.HangfireMonitoring;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/hangfire")]
public sealed class HangfireMonitoringController : BaseApiController
{
    private readonly IHangfireMonitoringService _hangfireMonitoringService;

    public HangfireMonitoringController(IHangfireMonitoringService hangfireMonitoringService)
    {
        _hangfireMonitoringService = hangfireMonitoringService;
    }

    [PermissionAuthorize(PermissionConstants.HangfireMonitoring.View)]
    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<HangfireStatsDto>>> GetStats(CancellationToken cancellationToken)
    {
        return OkResponse(await _hangfireMonitoringService.GetStatsAsync(cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.HangfireMonitoring.View)]
    [HttpGet("failed")]
    public async Task<ActionResult<ApiResponse<HangfireFailedJobsResponseDto>>> GetFailed([FromQuery] int from = 0, [FromQuery] int count = 20, CancellationToken cancellationToken = default)
    {
        return OkResponse(await _hangfireMonitoringService.GetFailuresFromDbAsync(from, count, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.HangfireMonitoring.View)]
    [HttpGet("failures-from-db")]
    public async Task<ActionResult<ApiResponse<HangfireFailedJobsResponseDto>>> GetFailuresFromDb([FromQuery] int from = 0, [FromQuery] int count = 50, CancellationToken cancellationToken = default)
    {
        return OkResponse(await _hangfireMonitoringService.GetFailuresFromDbAsync(from, count, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.HangfireMonitoring.View)]
    [HttpGet("dead-letter")]
    public async Task<ActionResult<ApiResponse<HangfireDeadLetterResponseDto>>> GetDeadLetter([FromQuery] int from = 0, [FromQuery] int count = 20, CancellationToken cancellationToken = default)
    {
        return OkResponse(await _hangfireMonitoringService.GetDeadLetterAsync(from, count, cancellationToken), LocalizationKeys.FetchSuccessful);
    }
}
