using QMSPOC.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace QMSPOC.Permissions;

public class QMSPOCPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(QMSPOCPermissions.GroupName);

        myGroup.AddPermission(QMSPOCPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(QMSPOCPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(QMSPOCPermissions.MyPermission1, L("Permission:MyPermission1"));

        var itemCategoryPermission = myGroup.AddPermission(QMSPOCPermissions.ItemCategories.Default, L("Permission:ItemCategories"));
        itemCategoryPermission.AddChild(QMSPOCPermissions.ItemCategories.Create, L("Permission:Create"));
        itemCategoryPermission.AddChild(QMSPOCPermissions.ItemCategories.Edit, L("Permission:Edit"));
        itemCategoryPermission.AddChild(QMSPOCPermissions.ItemCategories.Delete, L("Permission:Delete"));

        var itemPermission = myGroup.AddPermission(QMSPOCPermissions.Items.Default, L("Permission:Items"));
        itemPermission.AddChild(QMSPOCPermissions.Items.Create, L("Permission:Create"));
        itemPermission.AddChild(QMSPOCPermissions.Items.Edit, L("Permission:Edit"));
        itemPermission.AddChild(QMSPOCPermissions.Items.Delete, L("Permission:Delete"));

        var itemBomPermission = myGroup.AddPermission(QMSPOCPermissions.ItemBoms.Default, L("Permission:ItemBoms"));
        itemBomPermission.AddChild(QMSPOCPermissions.ItemBoms.Create, L("Permission:Create"));
        itemBomPermission.AddChild(QMSPOCPermissions.ItemBoms.Edit, L("Permission:Edit"));
        itemBomPermission.AddChild(QMSPOCPermissions.ItemBoms.Delete, L("Permission:Delete"));

        var itemBomDetailPermission = myGroup.AddPermission(QMSPOCPermissions.ItemBomDetails.Default, L("Permission:ItemBomDetails"));
        itemBomDetailPermission.AddChild(QMSPOCPermissions.ItemBomDetails.Create, L("Permission:Create"));
        itemBomDetailPermission.AddChild(QMSPOCPermissions.ItemBomDetails.Edit, L("Permission:Edit"));
        itemBomDetailPermission.AddChild(QMSPOCPermissions.ItemBomDetails.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<QMSPOCResource>(name);
    }
}