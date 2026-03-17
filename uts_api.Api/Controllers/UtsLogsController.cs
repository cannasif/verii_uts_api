using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.UtsLogs;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/uts-logs")]
public sealed class UtsLogsController : BaseApiController
{
    private readonly IUtsLogService _utsLogService;

    public UtsLogsController(IUtsLogService utsLogService)
    {
        _utsLogService = utsLogService;
    }

    [PermissionAuthorize(PermissionConstants.UtsLogs.View)]
    [HttpGet]
    public async Task<ActionResult<PagedApiResponse<UtsLogListItemDto>>> GetAll([FromQuery] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _utsLogService.GetPagedAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.UtsLogs.View)]
    [HttpPost("search")]
    public async Task<ActionResult<PagedApiResponse<UtsLogListItemDto>>> Search([FromBody] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _utsLogService.GetPagedAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.UtsLogs.View)]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<UtsLogDetailDto>>> GetById(long id, CancellationToken cancellationToken)
    {
        return OkResponse(await _utsLogService.GetByIdAsync(id, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.UtsLogs.Create)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<UtsLogDetailDto>>> Create([FromBody] CreateUtsLogRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _utsLogService.CreateAsync(request, cancellationToken);
        return CreatedResponse(nameof(GetById), new { id = response.Id }, response, LocalizationKeys.Created);
    }

    [PermissionAuthorize(PermissionConstants.UtsLogs.Update)]
    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<UtsLogDetailDto>>> Update(long id, [FromBody] UpdateUtsLogRequestDto request, CancellationToken cancellationToken)
    {
        return OkResponse(await _utsLogService.UpdateAsync(id, request, cancellationToken), LocalizationKeys.Updated);
    }

    [PermissionAuthorize(PermissionConstants.UtsLogs.Delete)]
    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ApiResponse>> Delete(long id, CancellationToken cancellationToken)
    {
        await _utsLogService.DeleteAsync(id, cancellationToken);
        return OkMessage(LocalizationKeys.Deleted);
    }
}
