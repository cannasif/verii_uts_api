using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.UtsImhaList;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/uts-imha-list")]
public sealed class UtsImhaListController : BaseApiController
{
    private readonly IUtsImhaListService _service;

    public UtsImhaListController(IUtsImhaListService service)
    {
        _service = service;
    }

    [PermissionAuthorize(PermissionConstants.UtsImhaList.View)]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<UtsImhaListItemDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return OkResponse(await _service.GetAllAsync(cancellationToken), LocalizationKeys.FetchSuccessful);
    }
}
