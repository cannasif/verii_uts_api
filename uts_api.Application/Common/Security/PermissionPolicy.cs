namespace uts_api.Application.Common.Security;

public static class PermissionPolicy
{
    public const string Prefix = "Permission:";

    public static string Build(string permission) => $"{Prefix}{permission}";
}
