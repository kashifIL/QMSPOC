using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QMSPOC.ItemBomDetails
{
    public interface IItemBomDetailRepository : IRepository<ItemBomDetail, Guid>
    {
        Task<List<ItemBomDetail>> GetListByItemBomIdAsync(
    Guid itemBomId,
    string? sorting = null,
    int maxResultCount = int.MaxValue,
    int skipCount = 0,
    CancellationToken cancellationToken = default
);

        Task<long> GetCountByItemBomIdAsync(Guid itemBomId, CancellationToken cancellationToken = default);

        Task<List<ItemBomDetailWithNavigationProperties>> GetListWithNavigationPropertiesByItemBomIdAsync(
            Guid itemBomId,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<ItemBomDetailWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default
        );

        Task<List<ItemBomDetailWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            decimal? qtyMin = null,
            decimal? qtyMax = null,
            string? uom = null,
            Guid? itemId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<ItemBomDetail>> GetListAsync(
                    string? filterText = null,
                    decimal? qtyMin = null,
                    decimal? qtyMax = null,
                    string? uom = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            decimal? qtyMin = null,
            decimal? qtyMax = null,
            string? uom = null,
            Guid? itemId = null,
            CancellationToken cancellationToken = default);
    }
}