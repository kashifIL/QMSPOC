using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QMSPOC.Data;
using Volo.Abp.DependencyInjection;

namespace QMSPOC.EntityFrameworkCore;

public class EntityFrameworkCoreQMSPOCDbSchemaMigrator
    : IQMSPOCDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreQMSPOCDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the QMSPOCDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<QMSPOCDbContext>()
            .Database
            .MigrateAsync();
    }
}
