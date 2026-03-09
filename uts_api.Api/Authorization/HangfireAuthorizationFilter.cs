using Hangfire.Dashboard;
using System.Security.Claims;

namespace uts_api.Api.Authorization;

public sealed class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
        {
            return false;
        }

        return httpContext.User.Claims.Any(claim =>
            (claim.Type == ClaimTypes.Role || claim.Type == "role") &&
            string.Equals(claim.Value, "Admin", StringComparison.OrdinalIgnoreCase));
    }
}
