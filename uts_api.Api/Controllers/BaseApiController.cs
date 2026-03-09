using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using uts_api.Api.Resources;
using uts_api.Application.Common.Models;

namespace uts_api.Api.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    private IStringLocalizer<SharedResource>? _localizer;
    protected IStringLocalizer<SharedResource> Localizer =>
        _localizer ??= HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();

    protected ActionResult<ApiResponse<T>> OkResponse<T>(T data, string messageKey) =>
        Ok(ApiResponse<T>.Ok(data, Localizer[messageKey]));

    protected ActionResult<PagedApiResponse<T>> OkPagedResponse<T>(PagedResult<T> result, string messageKey) =>
        Ok(PagedApiResponse<T>.Ok(result, Localizer[messageKey]));

    protected ActionResult<ApiResponse> OkMessage(string messageKey) =>
        Ok(ApiResponse.Ok(Localizer[messageKey]));

    protected ActionResult<ApiResponse<T>> CreatedResponse<T>(string actionName, object routeValues, T data, string messageKey) =>
        CreatedAtAction(actionName, routeValues, ApiResponse<T>.Ok(data, Localizer[messageKey]));
}
