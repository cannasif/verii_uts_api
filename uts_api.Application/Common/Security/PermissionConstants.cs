namespace uts_api.Application.Common.Security;

public static class PermissionConstants
{
    public static class Auth
    {
        public const string Me = "auth.me";
        public const string MyPermissions = "auth.my_permissions";
    }

    public static class Users
    {
        public const string View = "users.view";
        public const string Create = "users.create";
        public const string Update = "users.update";
        public const string Delete = "users.delete";
    }

    public static class Roles
    {
        public const string View = "roles.view";
    }

    public static class PermissionDefinitions
    {
        public const string View = "permission_definitions.view";
    }

    public static class PermissionGroups
    {
        public const string View = "permission_groups.view";
        public const string Create = "permission_groups.create";
        public const string Update = "permission_groups.update";
        public const string ManagePermissions = "permission_groups.manage_permissions";
    }

    public static class UserPermissionGroups
    {
        public const string View = "user_permission_groups.view";
        public const string Update = "user_permission_groups.update";
    }

    public static class Smtp
    {
        public const string View = "smtp.view";
        public const string Update = "smtp.update";
    }

    public static class HangfireMonitoring
    {
        public const string View = "hangfire_monitoring.view";
    }

    public static class Stocks
    {
        public const string View = "stocks.view";
        public const string Sync = "stocks.sync";
    }

    public static class Customers
    {
        public const string View = "customers.view";
        public const string Sync = "customers.sync";
    }

    public static IReadOnlyCollection<PermissionDefinitionSeedModel> All { get; } =
    [
        new("Auth", "My Profile", Auth.Me, "Read current user"),
        new("Auth", "My Permissions", Auth.MyPermissions, "Read current user permissions"),
        new("Users", "View Users", Users.View, "Read users"),
        new("Users", "Create User", Users.Create, "Create users"),
        new("Users", "Update User", Users.Update, "Update users"),
        new("Users", "Delete User", Users.Delete, "Delete users"),
        new("Roles", "View Roles", Roles.View, "Read roles"),
        new("PermissionDefinitions", "View Permission Definitions", PermissionDefinitions.View, "Read permission definitions"),
        new("PermissionGroups", "View Permission Groups", PermissionGroups.View, "Read permission groups"),
        new("PermissionGroups", "Create Permission Group", PermissionGroups.Create, "Create permission groups"),
        new("PermissionGroups", "Update Permission Group", PermissionGroups.Update, "Update permission groups"),
        new("PermissionGroups", "Manage Group Permissions", PermissionGroups.ManagePermissions, "Update permission group permissions"),
        new("UserPermissionGroups", "View User Permission Groups", UserPermissionGroups.View, "Read user permission groups"),
        new("UserPermissionGroups", "Update User Permission Groups", UserPermissionGroups.Update, "Update user permission groups"),
        new("Smtp", "View SMTP Settings", Smtp.View, "Read SMTP settings"),
        new("Smtp", "Update SMTP Settings", Smtp.Update, "Update SMTP settings"),
        new("HangfireMonitoring", "View Hangfire Monitoring", HangfireMonitoring.View, "Read Hangfire monitoring"),
        new("Stocks", "View Stocks", Stocks.View, "Read RII_STOCK records"),
        new("Stocks", "Sync Stocks", Stocks.Sync, "Trigger RII_STOCK sync job"),
        new("Customers", "View Customers", Customers.View, "Read RII_CUSTOMERS records"),
        new("Customers", "Sync Customers", Customers.Sync, "Trigger RII_CUSTOMERS sync job")
    ];
}
