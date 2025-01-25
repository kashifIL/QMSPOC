using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QMSPOC.ItemMeasuremetnDetails
{
    public interface IItemMeasuremetnDetailRepository : IRepository<ItemMeasuremetnDetail, Guid>
    {
        Task<List<ItemMeasuremetnDetail>> GetListByItemMessurementIdAsync(
    Guid itemMessurementId,
    string? sorting = null,
    int maxResultCount = int.MaxValue,
    int skipCount = 0,
    CancellationToken cancellationToken = default
);

        Task<long> GetCountByItemMessurementIdAsync(Guid itemMessurementId, CancellationToken cancellationToken = default);

        Task<List<ItemMeasuremetnDetail>> GetListAsync(
                    string? filterText = null,
                    string? type = null,
                    decimal? valueMin = null,
                    decimal? valueMax = null,
                    string? uom = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? type = null,
            decimal? valueMin = null,
            decimal? valueMax = null,
            string? uom = null,
            CancellationToken cancellationToken = default);
    }
}