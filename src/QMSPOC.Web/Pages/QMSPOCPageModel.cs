using QMSPOC.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace QMSPOC.Web.Pages;

public abstract class QMSPOCPageModel : AbpPageModel
{
    protected QMSPOCPageModel()
    {
        LocalizationResourceType = typeof(QMSPOCResource);
    }
}
