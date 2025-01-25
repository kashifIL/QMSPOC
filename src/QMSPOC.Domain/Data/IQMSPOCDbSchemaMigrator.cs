using System.Threading.Tasks;

namespace QMSPOC.Data;

public interface IQMSPOCDbSchemaMigrator
{
    Task MigrateAsync();
}
