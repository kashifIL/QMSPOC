using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using QMSPOC.Localization;

namespace QMSPOC.Web;

[Dependency(ReplaceServices = true)]
public class QMSPOCBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<QMSPOCResource> _localizer;

    public QMSPOCBrandingProvider(IStringLocalizer<QMSPOCResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
