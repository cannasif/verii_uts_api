using System.Text.Json;
using FluentValidation;
using Microsoft.Extensions.Localization;
using uts_api.Api.Resources;
using uts_api.Application.Common.Exceptions;
using uts_api.Application.Common.Localization;
using uts_api.Application.Common.Models;

namespace uts_api.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(
                ApiResponse.Fail(
                    localizer[LocalizationKeys.ValidationFailed],
                    ex.Errors.Select(x => x.ErrorMessage).ToArray())));
        }
        catch (AppException ex)
        {
            var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(
                ApiResponse.Fail(localizer[ex.MessageKey])));
        }
        catch (Exception)
        {
            var localizer = context.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(
                ApiResponse.Fail(localizer[LocalizationKeys.UnexpectedError])));
        }
    }
}
