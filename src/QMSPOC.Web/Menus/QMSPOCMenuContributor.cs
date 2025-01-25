using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using QMSPOC.Localization;
using QMSPOC.Permissions;
using QMSPOC.MultiTenancy;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.AuditLogging.Web.Navigation;
using Volo.Abp.LanguageManagement.Navigation;
using Volo.Abp.TextTemplateManagement.Web.Navigation;
using Volo.Abp.OpenIddict.Pro.Web.Menus;
using Volo.Saas.Host.Navigation;

namespace QMSPOC.Web.Menus;

public class QMSPOCMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<QMSPOCResource>();

        //Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                QMSPOCMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fa fa-home",
                order: 1
            )
        );

        //HostDashboard
        context.Menu.AddItem(
            new ApplicationMenuItem(
                QMSPOCMenus.HostDashboard,
                l["Menu:Dashboard"],
                "~/HostDashboard",
                icon: "fa fa-line-chart",
                order: 2
            ).RequirePermissions(QMSPOCPermissions.Dashboard.Host)
        );

        //TenantDashboard
        context.Menu.AddItem(
            new ApplicationMenuItem(
                QMSPOCMenus.TenantDashboard,
                l["Menu:Dashboard"],
                "~/Dashboard",
                icon: "fa fa-line-chart",
                order: 2
            ).RequirePermissions(QMSPOCPermissions.Dashboard.Tenant)
        );

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 5;

        //Administration->Saas
        administration.SetSubItemOrder(SaasHostMenuNames.GroupName, 1);

        //Administration->Identity
        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);

        //Administration->OpenIddict
        administration.SetSubItemOrder(OpenIddictProMenus.GroupName, 3);

        //Administration->Language Management
        administration.SetSubItemOrder(LanguageManagementMenuNames.GroupName, 4);

        //Administration->Text Template Management
        administration.SetSubItemOrder(TextTemplateManagementMainMenuNames.GroupName, 5);

        //Administration->Audit Logs
        administration.SetSubItemOrder(AbpAuditLoggingMainMenuNames.GroupName, 6);

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 7);

        context.Menu.AddItem(
            new ApplicationMenuItem(
                QMSPOCMenus.ItemCategories,
                l["Menu:ItemCategories"],
                url: "/ItemCategories",
                icon: "fa fa-file-alt",
                requiredPermissionName: QMSPOCPermissions.ItemCategories.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                QMSPOCMenus.Items,
                l["Menu:Items"],
                url: "/Items",
icon: "fa fa-file-alt",
                requiredPermissionName: QMSPOCPermissions.Items.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                QMSPOCMenus.ItemBoms,
                l["Menu:ItemBoms"],
                url: "/ItemBoms",
                icon: "fa fa-file-alt",
                requiredPermissionName: QMSPOCPermissions.ItemBoms.Default)
        );
        return Task.CompletedTask;
    }
}