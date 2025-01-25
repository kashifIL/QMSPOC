using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QMSPOC.Items
{
    public interface IItemRepository : IRepository<Item, Guid>
    {

        Task DeleteAllAsync(
            string? filterText = null,
            string? code = null,
            string? description = null,
            Guid? itemCategoryId = null,
            CancellationToken cancellationToken = default);
        Task<ItemWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default
        );

        Task<List<ItemWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? code = null,
            string? description = null,
            Guid? itemCategoryId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<Item>> GetListAsync(
                    string? filterText = null,
                    string? code = null,
                    string? description = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? code = null,
            string? description = null,
            Guid? itemCategoryId = null,
            CancellationToken cancellationToken = default);
    }
}