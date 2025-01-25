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

namespace QMSPOC.ItemMessurements
{
    public class EfCoreItemMessurementRepository : EfCoreRepository<QMSPOCDbContext, ItemMessurement, Guid>, IItemMessurementRepository
    {
        public EfCoreItemMessurementRepository(IDbContextProvider<QMSPOCDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        string? code = null,
            string? version = null,
            Guid? itemId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();

            query = ApplyFilter(query, filterText, code, version, itemId);

            var ids = query.Select(x => x.ItemMessurement.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<ItemMessurementWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(itemMessurement => new ItemMessurementWithNavigationProperties
                {
                    ItemMessurement = itemMessurement,
                    Item = dbContext.Set<Item>().FirstOrDefault(c => c.Id == itemMessurement.ItemId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<ItemMessurementWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? code = null,
            string? version = null,
            Guid? itemId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, code, version, itemId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemMessurementConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<ItemMessurementWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from itemMessurement in (await GetDbSetAsync())
                   join item in (await GetDbContextAsync()).Set<Item>() on itemMessurement.ItemId equals item.Id into items
                   from item in items.DefaultIfEmpty()
                   select new ItemMessurementWithNavigationProperties
                   {
                       ItemMessurement = itemMessurement,
                       Item = item
                   };
        }

        protected virtual IQueryable<ItemMessurementWithNavigationProperties> ApplyFilter(
            IQueryable<ItemMessurementWithNavigationProperties> query,
            string? filterText,
            string? code = null,
            string? version = null,
            Guid? itemId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.ItemMessurement.Code!.Contains(filterText!) || e.ItemMessurement.Version!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.ItemMessurement.Code.Contains(code))
                    .WhereIf(!string.IsNullOrWhiteSpace(version), e => e.ItemMessurement.Version.Contains(version))
                    .WhereIf(itemId != null && itemId != Guid.Empty, e => e.Item != null && e.Item.Id == itemId);
        }

        public virtual async Task<List<ItemMessurement>> GetListAsync(
            string? filterText = null,
            string? code = null,
            string? version = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, code, version);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemMessurementConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? code = null,
            string? version = null,
            Guid? itemId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, code, version, itemId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<ItemMessurement> ApplyFilter(
            IQueryable<ItemMessurement> query,
            string? filterText = null,
            string? code = null,
            string? version = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Code!.Contains(filterText!) || e.Version!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.Code.Contains(code))
                    .WhereIf(!string.IsNullOrWhiteSpace(version), e => e.Version.Contains(version));
        }
    }
}