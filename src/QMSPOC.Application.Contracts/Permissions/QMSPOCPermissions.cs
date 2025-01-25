namespace QMSPOC.Permissions;

public static class QMSPOCPermissions
{
    public const string GroupName = "QMSPOC";

    public static class Dashboard
    {
        public const string DashboardGroup = GroupName + ".Dashboard";
        public const string Host = DashboardGroup + ".Host";
        public const string Tenant = DashboardGroup + ".Tenant";
    }

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public static class ItemCategories
    {
        public const string Default = GroupName + ".ItemCategories";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class Items
    {
        public const string Default = GroupName + ".Items";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class ItemBoms
    {
        public const string Default = GroupName + ".ItemBoms";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class ItemBomDetails
    {
        public const string Default = GroupName + ".ItemBomDetails";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class ItemMessurements
    {
        public const string Default = GroupName + ".ItemMessurements";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public static class ItemMeasuremetnDetails
    {
        public const string Default = GroupName + ".ItemMeasuremetnDetails";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}