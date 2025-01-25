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

namespace QMSPOC.ItemBomDetails
{
    public class EfCoreItemBomDetailRepository : EfCoreRepository<QMSPOCDbContext, ItemBomDetail, Guid>, IItemBomDetailRepository
    {
        public EfCoreItemBomDetailRepository(IDbContextProvider<QMSPOCDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<List<ItemBomDetail>> GetListByItemBomIdAsync(
           Guid itemBomId,
           string? sorting = null,
           int maxResultCount = int.MaxValue,
           int skipCount = 0,
           CancellationToken cancellationToken = default)
        {
            var query = (await GetQueryableAsync()).Where(x => x.ItemBomId == itemBomId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemBomDetailConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountByItemBomIdAsync(Guid itemBomId, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync()).Where(x => x.ItemBomId == itemBomId).CountAsync(cancellationToken);
        }

        public virtual async Task<List<ItemBomDetailWithNavigationProperties>> GetListWithNavigationPropertiesByItemBomIdAsync(
    Guid itemBomId,
    string? sorting = null,
    int maxResultCount = int.MaxValue,
    int skipCount = 0,
    CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = query.Where(x => x.ItemBomDetail.ItemBomId == itemBomId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemBomDetailConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<ItemBomDetailWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(itemBomDetail => new ItemBomDetailWithNavigationProperties
                {
                    ItemBomDetail = itemBomDetail,
                    Item = dbContext.Set<Item>().FirstOrDefault(c => c.Id == itemBomDetail.ItemId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<ItemBomDetailWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            decimal? qtyMin = null,
            decimal? qtyMax = null,
            string? uom = null,
            Guid? itemId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, qtyMin, qtyMax, uom, itemId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemBomDetailConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<ItemBomDetailWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from itemBomDetail in (await GetDbSetAsync())
                   join item in (await GetDbContextAsync()).Set<Item>() on itemBomDetail.ItemId equals item.Id into items
                   from item in items.DefaultIfEmpty()
                   select new ItemBomDetailWithNavigationProperties
                   {
                       ItemBomDetail = itemBomDetail,
                       Item = item
                   };
        }

        protected virtual IQueryable<ItemBomDetailWithNavigationProperties> ApplyFilter(
            IQueryable<ItemBomDetailWithNavigationProperties> query,
            string? filterText,
            decimal? qtyMin = null,
            decimal? qtyMax = null,
            string? uom = null,
            Guid? itemId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.ItemBomDetail.Uom!.Contains(filterText!))
                    .WhereIf(qtyMin.HasValue, e => e.ItemBomDetail.Qty >= qtyMin!.Value)
                    .WhereIf(qtyMax.HasValue, e => e.ItemBomDetail.Qty <= qtyMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(uom), e => e.ItemBomDetail.Uom.Contains(uom))
                    .WhereIf(itemId != null && itemId != Guid.Empty, e => e.Item != null && e.Item.Id == itemId);
        }

        public virtual async Task<List<ItemBomDetail>> GetListAsync(
            string? filterText = null,
            decimal? qtyMin = null,
            decimal? qtyMax = null,
            string? uom = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, qtyMin, qtyMax, uom);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemBomDetailConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            decimal? qtyMin = null,
            decimal? qtyMax = null,
            string? uom = null,
            Guid? itemId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, qtyMin, qtyMax, uom, itemId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<ItemBomDetail> ApplyFilter(
            IQueryable<ItemBomDetail> query,
            string? filterText = null,
            decimal? qtyMin = null,
            decimal? qtyMax = null,
            string? uom = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Uom!.Contains(filterText!))
                    .WhereIf(qtyMin.HasValue, e => e.Qty >= qtyMin!.Value)
                    .WhereIf(qtyMax.HasValue, e => e.Qty <= qtyMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(uom), e => e.Uom.Contains(uom));
        }
    }
}