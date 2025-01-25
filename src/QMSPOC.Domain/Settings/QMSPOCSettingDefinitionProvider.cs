using Volo.Abp.Settings;

namespace QMSPOC.Settings;

public class QMSPOCSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(QMSPOCSettings.MySetting1));
    }
}
