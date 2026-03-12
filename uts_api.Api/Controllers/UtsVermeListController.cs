using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.UtsVermeList;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/uts-verme-list")]
public sealed class UtsVermeListController : BaseApiController
{
    private readonly IUtsVermeListService _utsVermeListService;

    public UtsVermeListController(IUtsVermeListService utsVermeListService)
    {
        _utsVermeListService = utsVermeListService;
    }

    [PermissionAuthorize(PermissionConstants.UtsVermeList.View)]
    [HttpGet]
    public async Task<ActionResult<PagedApiResponse<UtsVermeListItemDto>>> GetAll([FromQuery] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _utsVermeListService.GetPagedAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.UtsVermeList.View)]
    [HttpPost("search")]
    public async Task<ActionResult<PagedApiResponse<UtsVermeListItemDto>>> Search([FromBody] PagedRequest request, CancellationToken cancellationToken)
    {
        return OkPagedResponse(await _utsVermeListService.GetPagedAsync(request, cancellationToken), LocalizationKeys.FetchSuccessful);
    }
}
