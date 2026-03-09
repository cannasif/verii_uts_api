using System.Globalization;

namespace uts_api.Api.Middleware;

public sealed class LocalizationMiddleware
{
    private static readonly HashSet<string> SupportedCultures = new(StringComparer.OrdinalIgnoreCase) { "tr", "en" };
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestedCulture = context.Request.Headers["x-language"].ToString();
        if (string.IsNullOrWhiteSpace(requestedCulture))
        {
            requestedCulture = context.Request.Headers.AcceptLanguage
                .ToString()
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => x.Split(';')[0])
                .FirstOrDefault();
        }

        var cultureCode = requestedCulture?.StartsWith("tr", StringComparison.OrdinalIgnoreCase) == true ? "tr" : "en";
        if (!SupportedCultures.Contains(cultureCode))
        {
            cultureCode = "tr";
        }

        var culture = new CultureInfo(cultureCode);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await _next(context);
    }
}
