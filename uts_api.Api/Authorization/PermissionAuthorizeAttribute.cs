using Microsoft.AspNetCore.Authorization;
using uts_api.Application.Common.Security;

namespace uts_api.Api.Authorization;

public sealed class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    public PermissionAuthorizeAttribute(string permission)
    {
        Policy = PermissionPolicy.Build(permission);
    }
}
