using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.UtsAlmaList;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/uts-alma-list")]
public sealed class UtsAlmaListController : BaseApiController
{
    private readonly IUtsAlmaListService _service;

    public UtsAlmaListController(IUtsAlmaListService service)
    {
        _service = service;
    }

    [PermissionAuthorize(PermissionConstants.UtsAlmaList.View)]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<UtsAlmaListItemDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return OkResponse(await _service.GetAllAsync(cancellationToken), LocalizationKeys.FetchSuccessful);
    }
}
