using QMSPOC.ItemMeasuremetnDetails;
using QMSPOC.ItemMessurements;
using QMSPOC.ItemBomDetails;
using QMSPOC.ItemBoms;
using QMSPOC.Items;
using QMSPOC.ItemCategories;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.FileManagement.EntityFrameworkCore;
using Volo.Chat.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Saas.EntityFrameworkCore;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;
using Volo.Abp.Gdpr;

namespace QMSPOC.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityProDbContext))]
[ReplaceDbContext(typeof(ISaasDbContext))]
[ConnectionStringName("Default")]
public class QMSPOCDbContext :
    AbpDbContext<QMSPOCDbContext>,
    ISaasDbContext,
    IIdentityProDbContext
{
    public DbSet<ItemMeasuremetnDetail> ItemMeasuremetnDetails { get; set; } = null!;
    public DbSet<ItemMessurement> ItemMessurements { get; set; } = null!;
    public DbSet<ItemBomDetail> ItemBomDetails { get; set; } = null!;
    public DbSet<ItemBom> ItemBoms { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<ItemCategory> ItemCategories { get; set; } = null!;
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // SaaS
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Edition> Editions { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public QMSPOCDbContext(DbContextOptions<QMSPOCDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentityPro();
        builder.ConfigureOpenIddictPro();
        builder.ConfigureLanguageManagement();
        builder.ConfigureFileManagement();
        builder.ConfigureSaas();
        builder.ConfigureChat();
        builder.ConfigureTextTemplateManagement();
        builder.ConfigureGdpr();
        builder.ConfigureBlobStoring();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(QMSPOCConsts.DbTablePrefix + "YourEntities", QMSPOCConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        builder.Entity<ItemCategory>(b =>
                {
                    b.ToTable(QMSPOCConsts.DbTablePrefix + "ItemCategories", QMSPOCConsts.DbSchema);
                    b.ConfigureByConvention();
                    b.Property(x => x.TenantId).HasColumnName(nameof(ItemCategory.TenantId));
                    b.Property(x => x.Code).HasColumnName(nameof(ItemCategory.Code)).IsRequired();
                    b.Property(x => x.Name).HasColumnName(nameof(ItemCategory.Name)).IsRequired();
                });

        builder.Entity<Item>(b =>
                {
                    b.ToTable(QMSPOCConsts.DbTablePrefix + "Items", QMSPOCConsts.DbSchema);
                    b.ConfigureByConvention();
                    b.Property(x => x.TenantId).HasColumnName(nameof(Item.TenantId));
                    b.Property(x => x.Code).HasColumnName(nameof(Item.Code)).IsRequired();
                    b.Property(x => x.Description).HasColumnName(nameof(Item.Description)).IsRequired();
                    b.HasOne<ItemCategory>().WithMany().IsRequired().HasForeignKey(x => x.ItemCategoryId).OnDelete(DeleteBehavior.NoAction);
                });

        builder.Entity<ItemBom>(b =>
                {
                    b.ToTable(QMSPOCConsts.DbTablePrefix + "ItemBoms", QMSPOCConsts.DbSchema);
                    b.ConfigureByConvention();
                    b.Property(x => x.TenantId).HasColumnName(nameof(ItemBom.TenantId));
                    b.Property(x => x.Code).HasColumnName(nameof(ItemBom.Code)).IsRequired();
                    b.Property(x => x.Version).HasColumnName(nameof(ItemBom.Version));
                    b.Property(x => x.Description).HasColumnName(nameof(ItemBom.Description));
                    b.HasOne<Item>().WithMany().IsRequired().HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.NoAction);
                    b.HasMany(x => x.ItemBomDetails).WithOne().HasForeignKey(x => x.ItemBomId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                });
        builder.Entity<ItemBomDetail>(b =>
                {
                    b.ToTable(QMSPOCConsts.DbTablePrefix + "ItemBomDetails", QMSPOCConsts.DbSchema);
                    b.ConfigureByConvention();
                    b.Property(x => x.TenantId).HasColumnName(nameof(ItemBomDetail.TenantId));
                    b.Property(x => x.Qty).HasColumnName(nameof(ItemBomDetail.Qty));
                    b.Property(x => x.Uom).HasColumnName(nameof(ItemBomDetail.Uom));
                    b.HasOne<Item>().WithMany().IsRequired().HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.NoAction);
                    b.HasOne<ItemBom>().WithMany(x => x.ItemBomDetails).HasForeignKey(x => x.ItemBomId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                });

        builder.Entity<ItemMessurement>(b =>
                {
                    b.ToTable(QMSPOCConsts.DbTablePrefix + "ItemMessurements", QMSPOCConsts.DbSchema);
                    b.ConfigureByConvention();
                    b.Property(x => x.TenantId).HasColumnName(nameof(ItemMessurement.TenantId));
                    b.Property(x => x.Code).HasColumnName(nameof(ItemMessurement.Code)).IsRequired();
                    b.Property(x => x.Version).HasColumnName(nameof(ItemMessurement.Version));
                    b.HasOne<Item>().WithMany().IsRequired().HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.NoAction);
                    b.HasMany(x => x.ItemMeasuremetnDetails).WithOne().HasForeignKey(x => x.ItemMessurementId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                });
        builder.Entity<ItemMeasuremetnDetail>(b =>
                {
                    b.ToTable(QMSPOCConsts.DbTablePrefix + "ItemMeasuremetnDetails", QMSPOCConsts.DbSchema);
                    b.ConfigureByConvention();
                    b.Property(x => x.TenantId).HasColumnName(nameof(ItemMeasuremetnDetail.TenantId));
                    b.Property(x => x.Type).HasColumnName(nameof(ItemMeasuremetnDetail.Type));
                    b.Property(x => x.Value).HasColumnName(nameof(ItemMeasuremetnDetail.Value));
                    b.Property(x => x.Uom).HasColumnName(nameof(ItemMeasuremetnDetail.Uom));
                    b.HasOne<ItemMessurement>().WithMany(x => x.ItemMeasuremetnDetails).HasForeignKey(x => x.ItemMessurementId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                });
    }
}