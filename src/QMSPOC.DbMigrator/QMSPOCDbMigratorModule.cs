using QMSPOC.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace QMSPOC.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(QMSPOCEntityFrameworkCoreModule),
    typeof(QMSPOCApplicationContractsModule)
)]
public class QMSPOCDbMigratorModule : AbpModule
{
}
