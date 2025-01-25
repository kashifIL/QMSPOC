using QMSPOC.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using QMSPOC.EntityFrameworkCore;

namespace QMSPOC.ItemBoms
{
    public class EfCoreItemBomRepository : EfCoreRepository<QMSPOCDbContext, ItemBom, Guid>, IItemBomRepository
    {
        public EfCoreItemBomRepository(IDbContextProvider<QMSPOCDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        string? code = null,
            int? versionMin = null,
            int? versionMax = null,
            string? description = null,
            Guid? itemId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();

            query = ApplyFilter(query, filterText, code, versionMin, versionMax, description, itemId);

            var ids = query.Select(x => x.ItemBom.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<ItemBomWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(itemBom => new ItemBomWithNavigationProperties
                {
                    ItemBom = itemBom,
                    Item = dbContext.Set<Item>().FirstOrDefault(c => c.Id == itemBom.ItemId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<ItemBomWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? code = null,
            int? versionMin = null,
            int? versionMax = null,
            string? description = null,
            Guid? itemId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, code, versionMin, versionMax, description, itemId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemBomConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<ItemBomWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from itemBom in (await GetDbSetAsync())
                   join item in (await GetDbContextAsync()).Set<Item>() on itemBom.ItemId equals item.Id into items
                   from item in items.DefaultIfEmpty()
                   select new ItemBomWithNavigationProperties
                   {
                       ItemBom = itemBom,
                       Item = item
                   };
        }

        protected virtual IQueryable<ItemBomWithNavigationProperties> ApplyFilter(
            IQueryable<ItemBomWithNavigationProperties> query,
            string? filterText,
            string? code = null,
            int? versionMin = null,
            int? versionMax = null,
            string? description = null,
            Guid? itemId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.ItemBom.Code!.Contains(filterText!) || e.ItemBom.Description!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.ItemBom.Code.Contains(code))
                    .WhereIf(versionMin.HasValue, e => e.ItemBom.Version >= versionMin!.Value)
                    .WhereIf(versionMax.HasValue, e => e.ItemBom.Version <= versionMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.ItemBom.Description.Contains(description))
                    .WhereIf(itemId != null && itemId != Guid.Empty, e => e.Item != null && e.Item.Id == itemId);
        }

        public virtual async Task<List<ItemBom>> GetListAsync(
            string? filterText = null,
            string? code = null,
            int? versionMin = null,
            int? versionMax = null,
            string? description = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, code, versionMin, versionMax, description);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemBomConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? code = null,
            int? versionMin = null,
            int? versionMax = null,
            string? description = null,
            Guid? itemId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, code, versionMin, versionMax, description, itemId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<ItemBom> ApplyFilter(
            IQueryable<ItemBom> query,
            string? filterText = null,
            string? code = null,
            int? versionMin = null,
            int? versionMax = null,
            string? description = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Code!.Contains(filterText!) || e.Description!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.Code.Contains(code))
                    .WhereIf(versionMin.HasValue, e => e.Version >= versionMin!.Value)
                    .WhereIf(versionMax.HasValue, e => e.Version <= versionMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description));
        }
    }
}