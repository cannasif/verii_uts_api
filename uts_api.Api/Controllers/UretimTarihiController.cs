using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.UretimTarihi;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/uretim-tarihi")]
public sealed class UretimTarihiController : BaseApiController
{
    private readonly IUretimTarihiService _uretimTarihiService;

    public UretimTarihiController(IUretimTarihiService uretimTarihiService)
    {
        _uretimTarihiService = uretimTarihiService;
    }

    [PermissionAuthorize(PermissionConstants.UretimTarihi.View)]
    [HttpGet]
    public async Task<ActionResult<PagedApiResponse<UretimTarihiListItemDto>>> GetAll([FromQuery] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _uretimTarihiService.GetPagedAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.UretimTarihi.View)]
    [HttpPost("search")]
    public async Task<ActionResult<PagedApiResponse<UretimTarihiListItemDto>>> Search([FromBody] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _uretimTarihiService.GetPagedAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.UretimTarihi.View)]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<UretimTarihiDetailDto>>> GetById(long id, CancellationToken cancellationToken)
    {
        return OkResponse(await _uretimTarihiService.GetByIdAsync(id, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.UretimTarihi.Create)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<UretimTarihiDetailDto>>> Create([FromBody] CreateUretimTarihiRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _uretimTarihiService.CreateAsync(request, cancellationToken);
        return CreatedResponse(nameof(GetById), new { id = response.Id }, response, LocalizationKeys.Created);
    }

    [PermissionAuthorize(PermissionConstants.UretimTarihi.Update)]
    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<UretimTarihiDetailDto>>> Update(long id, [FromBody] UpdateUretimTarihiRequestDto request, CancellationToken cancellationToken)
    {
        return OkResponse(await _uretimTarihiService.UpdateAsync(id, request, cancellationToken), LocalizationKeys.Updated);
    }

    [PermissionAuthorize(PermissionConstants.UretimTarihi.Delete)]
    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ApiResponse>> Delete(long id, CancellationToken cancellationToken)
    {
        await _uretimTarihiService.DeleteAsync(id, cancellationToken);
        return OkMessage(LocalizationKeys.Deleted);
    }
}
