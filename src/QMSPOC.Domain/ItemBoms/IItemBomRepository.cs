using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QMSPOC.ItemBoms
{
    public interface IItemBomRepository : IRepository<ItemBom, Guid>
    {

        Task DeleteAllAsync(
            string? filterText = null,
            string? code = null,
            int? versionMin = null,
            int? versionMax = null,
            string? description = null,
            Guid? itemId = null,
            CancellationToken cancellationToken = default);
        Task<ItemBomWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default
        );

        Task<List<ItemBomWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? code = null,
            int? versionMin = null,
            int? versionMax = null,
            string? description = null,
            Guid? itemId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<ItemBom>> GetListAsync(
                    string? filterText = null,
                    string? code = null,
                    int? versionMin = null,
                    int? versionMax = null,
                    string? description = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? code = null,
            int? versionMin = null,
            int? versionMax = null,
            string? description = null,
            Guid? itemId = null,
            CancellationToken cancellationToken = default);
    }
}