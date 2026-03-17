using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.UtsTuketiciVermeList;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/uts-tuketici-verme-list")]
public sealed class UtsTuketiciVermeListController : BaseApiController
{
    private readonly IUtsTuketiciVermeListService _service;

    public UtsTuketiciVermeListController(IUtsTuketiciVermeListService service)
    {
        _service = service;
    }

    [PermissionAuthorize(PermissionConstants.UtsTuketiciVermeList.View)]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<UtsTuketiciVermeListItemDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return OkResponse(await _service.GetAllAsync(cancellationToken), LocalizationKeys.FetchSuccessful);
    }
}
