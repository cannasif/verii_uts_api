using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.UtsIhracatList;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/uts-ihracat-list")]
public sealed class UtsIhracatListController : BaseApiController
{
    private readonly IUtsIhracatListService _service;

    public UtsIhracatListController(IUtsIhracatListService service)
    {
        _service = service;
    }

    [PermissionAuthorize(PermissionConstants.UtsIhracatList.View)]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<UtsIhracatListItemDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return OkResponse(await _service.GetAllAsync(cancellationToken), LocalizationKeys.FetchSuccessful);
    }
}
