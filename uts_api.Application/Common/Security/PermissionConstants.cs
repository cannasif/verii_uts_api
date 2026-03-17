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

    public static class UtsVermeList
    {
        public const string View = "uts_verme_list.view";
    }

    public static class UtsUretimList
    {
        public const string View = "uts_uretim_list.view";
    }

    public static class UtsTVermeList
    {
        public const string View = "uts_tverme_list.view";
    }

    public static class UtsTuketiciVermeList
    {
        public const string View = "uts_tuketici_verme_list.view";
    }

    public static class UtsIthalatList
    {
        public const string View = "uts_ithalat_list.view";
    }

    public static class UtsImhaList
    {
        public const string View = "uts_imha_list.view";
    }

    public static class UtsIhracatList
    {
        public const string View = "uts_ihracat_list.view";
    }

    public static class UtsAlmaList
    {
        public const string View = "uts_alma_list.view";
    }

    public static class UtsLogs
    {
        public const string View = "uts_logs.view";
        public const string Create = "uts_logs.create";
        public const string Update = "uts_logs.update";
        public const string Delete = "uts_logs.delete";
    }

    public static class UretimTarihi
    {
        public const string View = "uretim_tarihi.view";
        public const string Create = "uretim_tarihi.create";
        public const string Update = "uretim_tarihi.update";
        public const string Delete = "uretim_tarihi.delete";
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
        new("Customers", "Sync Customers", Customers.Sync, "Trigger RII_CUSTOMERS sync job"),
        new("UtsVermeList", "View UTS Verme List", UtsVermeList.View, "Read VWLEGO_UTS_VERME_LIST records"),
        new("UtsUretimList", "View UTS Production List", UtsUretimList.View, "Read VWLEGO_UTS_URETIM_LIST records"),
        new("UtsTVermeList", "View UTS Undefined Delivery List", UtsTVermeList.View, "Read VWLEGO_UTS_TVERME_LIST records"),
        new("UtsTuketiciVermeList", "View UTS Consumer Delivery List", UtsTuketiciVermeList.View, "Read VWLEGO_UTS_TUKETICI_VERME_LIST records"),
        new("UtsIthalatList", "View UTS Import List", UtsIthalatList.View, "Read VWLEGO_UTS_ITHALAT_LIST records"),
        new("UtsImhaList", "View UTS Disposal List", UtsImhaList.View, "Read VWLEGO_UTS_IMHA_LIST records"),
        new("UtsIhracatList", "View UTS Export List", UtsIhracatList.View, "Read VWLEGO_UTS_IHRACAT_LIST records"),
        new("UtsAlmaList", "View UTS Receipt List", UtsAlmaList.View, "Read VWLEGO_UTS_ALMA_LIST records"),
        new("UtsLogs", "View UTS Logs", UtsLogs.View, "Read UTS log records"),
        new("UtsLogs", "Create UTS Log", UtsLogs.Create, "Create UTS log records"),
        new("UtsLogs", "Update UTS Log", UtsLogs.Update, "Update UTS log records"),
        new("UtsLogs", "Delete UTS Log", UtsLogs.Delete, "Delete UTS log records"),
        new("UretimTarihi", "View Production Date Records", UretimTarihi.View, "Read production date records"),
        new("UretimTarihi", "Create Production Date Record", UretimTarihi.Create, "Create production date records"),
        new("UretimTarihi", "Update Production Date Record", UretimTarihi.Update, "Update production date records"),
        new("UretimTarihi", "Delete Production Date Record", UretimTarihi.Delete, "Delete production date records")
    ];
}
