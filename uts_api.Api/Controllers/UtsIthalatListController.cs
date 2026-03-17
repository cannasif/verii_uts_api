using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.UtsIthalatList;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/uts-ithalat-list")]
public sealed class UtsIthalatListController : BaseApiController
{
    private readonly IUtsIthalatListService _service;

    public UtsIthalatListController(IUtsIthalatListService service)
    {
        _service = service;
    }

    [PermissionAuthorize(PermissionConstants.UtsIthalatList.View)]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<UtsIthalatListItemDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return OkResponse(await _service.GetAllAsync(cancellationToken), LocalizationKeys.FetchSuccessful);
    }
}
