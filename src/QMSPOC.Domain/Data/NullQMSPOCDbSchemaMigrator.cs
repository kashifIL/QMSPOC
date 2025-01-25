using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace QMSPOC.Data;

/* This is used if database provider does't define
 * IQMSPOCDbSchemaMigrator implementation.
 */
public class NullQMSPOCDbSchemaMigrator : IQMSPOCDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
