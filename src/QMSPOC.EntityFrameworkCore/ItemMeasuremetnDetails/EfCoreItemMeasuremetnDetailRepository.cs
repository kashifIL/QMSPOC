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

namespace QMSPOC.ItemMeasuremetnDetails
{
    public class EfCoreItemMeasuremetnDetailRepository : EfCoreRepository<QMSPOCDbContext, ItemMeasuremetnDetail, Guid>, IItemMeasuremetnDetailRepository
    {
        public EfCoreItemMeasuremetnDetailRepository(IDbContextProvider<QMSPOCDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<List<ItemMeasuremetnDetail>> GetListByItemMessurementIdAsync(
           Guid itemMessurementId,
           string? sorting = null,
           int maxResultCount = int.MaxValue,
           int skipCount = 0,
           CancellationToken cancellationToken = default)
        {
            var query = (await GetQueryableAsync()).Where(x => x.ItemMessurementId == itemMessurementId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemMeasuremetnDetailConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountByItemMessurementIdAsync(Guid itemMessurementId, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync()).Where(x => x.ItemMessurementId == itemMessurementId).CountAsync(cancellationToken);
        }

        public virtual async Task<List<ItemMeasuremetnDetail>> GetListAsync(
            string? filterText = null,
            string? type = null,
            decimal? valueMin = null,
            decimal? valueMax = null,
            string? uom = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, type, valueMin, valueMax, uom);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ItemMeasuremetnDetailConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? type = null,
            decimal? valueMin = null,
            decimal? valueMax = null,
            string? uom = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, type, valueMin, valueMax, uom);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<ItemMeasuremetnDetail> ApplyFilter(
            IQueryable<ItemMeasuremetnDetail> query,
            string? filterText = null,
            string? type = null,
            decimal? valueMin = null,
            decimal? valueMax = null,
            string? uom = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Type!.Contains(filterText!) || e.Uom!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(type), e => e.Type.Contains(type))
                    .WhereIf(valueMin.HasValue, e => e.Value >= valueMin!.Value)
                    .WhereIf(valueMax.HasValue, e => e.Value <= valueMax!.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(uom), e => e.Uom.Contains(uom));
        }
    }
}