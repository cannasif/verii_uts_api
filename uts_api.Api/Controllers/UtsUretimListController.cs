using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.UtsUretimList;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/uts-uretim-list")]
public sealed class UtsUretimListController : BaseApiController
{
    private readonly IUtsUretimListService _utsUretimListService;

    public UtsUretimListController(IUtsUretimListService utsUretimListService)
    {
        _utsUretimListService = utsUretimListService;
    }

    [PermissionAuthorize(PermissionConstants.UtsUretimList.View)]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<UtsUretimListItemDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return OkResponse(await _utsUretimListService.GetAllAsync(cancellationToken), LocalizationKeys.FetchSuccessful);
    }
}
