using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QMSPOC.ItemMessurements
{
    public interface IItemMessurementRepository : IRepository<ItemMessurement, Guid>
    {

        Task DeleteAllAsync(
            string? filterText = null,
            string? code = null,
            string? version = null,
            Guid? itemId = null,
            CancellationToken cancellationToken = default);
        Task<ItemMessurementWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default
        );

        Task<List<ItemMessurementWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? code = null,
            string? version = null,
            Guid? itemId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<ItemMessurement>> GetListAsync(
                    string? filterText = null,
                    string? code = null,
                    string? version = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? code = null,
            string? version = null,
            Guid? itemId = null,
            CancellationToken cancellationToken = default);
    }
}