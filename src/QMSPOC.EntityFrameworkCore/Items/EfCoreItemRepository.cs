using QMSPOC.ItemCategories;
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

namespace QMSPOC.Items
{
    public class EfCoreItemRepository : EfCoreRepository<QMSPOCDbContext, Item, Guid>, IItemRepository
    {
        public EfCoreItemRepository(IDbContextProvider<QMSPOCDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        string? code = null,
            string? description = null,
            Guid? itemCategoryId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();

            query = ApplyFilter(query, filterText, code, description, itemCategoryId);

            var ids = query.Select(x => x.Item.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<ItemWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(item => new ItemWithNavigationProperties
                {
                    Item = item,
                    ItemCategory = dbContext.Set<ItemCategory>().FirstOrDefault(c => c.Id == item.ItemCategoryId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<ItemWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? code = null,
            string? description = null,
            Guid? itemCategoryId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, code, description, itemCategoryId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<ItemWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from item in (await GetDbSetAsync())
                   join itemCategory in (await GetDbContextAsync()).Set<ItemCategory>() on item.ItemCategoryId equals itemCategory.Id into itemCategories
                   from itemCategory in itemCategories.DefaultIfEmpty()
                   select new ItemWithNavigationProperties
                   {
                       Item = item,
                       ItemCategory = itemCategory
                   };
        }

        protected virtual IQueryable<ItemWithNavigationProperties> ApplyFilter(
            IQueryable<ItemWithNavigationProperties> query,
            string? filterText,
            string? code = null,
            string? description = null,
            Guid? itemCategoryId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Item.Code!.Contains(filterText!) || e.Item.Description!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.Item.Code.Contains(code))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Item.Description.Contains(description))
                    .WhereIf(itemCategoryId != null && itemCategoryId != Guid.Empty, e => e.ItemCategory != null && e.ItemCategory.Id == itemCategoryId);
        }

        public virtual async Task<List<Item>> GetListAsync(
            string? filterText = null,
            string? code = null,
            string? description = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, code, description);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? code = null,
            string? description = null,
            Guid? itemCategoryId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, code, description, itemCategoryId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Item> ApplyFilter(
            IQueryable<Item> query,
            string? filterText = null,
            string? code = null,
            string? description = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Code!.Contains(filterText!) || e.Description!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.Code.Contains(code))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description));
        }
    }
}