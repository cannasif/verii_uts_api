using Microsoft.AspNetCore.Mvc;
using uts_api.Api.Authorization;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;
using uts_api.Application.Common.Security;
using uts_api.Application.DTOs.Smtp;
using uts_api.Application.Interfaces;

namespace uts_api.Api.Controllers;

[Route("api/smtp-settings")]
public sealed class SmtpSettingsController : BaseApiController
{
    private readonly ISmtpSettingService _smtpSettingService;

    public SmtpSettingsController(ISmtpSettingService smtpSettingService)
    {
        _smtpSettingService = smtpSettingService;
    }

    [PermissionAuthorize(PermissionConstants.Smtp.View)]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<SmtpSettingDto?>>> Get(CancellationToken cancellationToken)
    {
        return OkResponse(await _smtpSettingService.GetActiveAsync(cancellationToken), LocalizationKeys.FetchSuccessful);
    }

    [PermissionAuthorize(PermissionConstants.Smtp.Update)]
    [HttpPut]
    public async Task<ActionResult<ApiResponse<SmtpSettingDto>>> Upsert([FromBody] SmtpSettingUpsertRequestDto request, CancellationToken cancellationToken)
    {
        return OkResponse(await _smtpSettingService.UpsertAsync(request, cancellationToken), LocalizationKeys.SmtpUpdated);
    }
}
