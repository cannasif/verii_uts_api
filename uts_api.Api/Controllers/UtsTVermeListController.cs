using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.UtsTVermeList;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/uts-tverme-list")]
public sealed class UtsTVermeListController : BaseApiController
{
    private readonly IUtsTVermeListService _utsTVermeListService;

    public UtsTVermeListController(IUtsTVermeListService utsTVermeListService)
    {
        _utsTVermeListService = utsTVermeListService;
    }

    [PermissionAuthorize(PermissionConstants.UtsTVermeList.View)]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<UtsTVermeListItemDto>>>> GetAll(CancellationToken cancellationToken)
    {
        return OkResponse(await _utsTVermeListService.GetAllAsync(cancellationToken), LocalizationKeys.FetchSuccessful);
    }
}
