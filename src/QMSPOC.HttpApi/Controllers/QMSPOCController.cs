using QMSPOC.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace QMSPOC.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class QMSPOCController : AbpControllerBase
{
    protected QMSPOCController()
    {
        LocalizationResource = typeof(QMSPOCResource);
    }
}
