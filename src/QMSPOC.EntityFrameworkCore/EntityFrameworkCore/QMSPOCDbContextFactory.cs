using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace QMSPOC.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class QMSPOCDbContextFactory : IDesignTimeDbContextFactory<QMSPOCDbContext>
{
    public QMSPOCDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        QMSPOCEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<QMSPOCDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new QMSPOCDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../QMSPOC.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
