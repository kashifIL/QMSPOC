using QMSPOC.Localization;
using Volo.Abp.Application.Services;

namespace QMSPOC;

/* Inherit your application services from this class.
 */
public abstract class QMSPOCAppService : ApplicationService
{
    protected QMSPOCAppService()
    {
        LocalizationResource = typeof(QMSPOCResource);
    }
}
