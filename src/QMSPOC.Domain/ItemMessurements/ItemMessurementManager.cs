using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace QMSPOC.ItemMessurements
{
    public class ItemMessurementManager : DomainService
    {
        protected IItemMessurementRepository _itemMessurementRepository;

        public ItemMessurementManager(IItemMessurementRepository itemMessurementRepository)
        {
            _itemMessurementRepository = itemMessurementRepository;
        }

        public virtual async Task<ItemMessurement> CreateAsync(
        Guid itemId, string code, string? version = null)
        {
            Check.NotNull(itemId, nameof(itemId));
            Check.NotNullOrWhiteSpace(code, nameof(code));

            var itemMessurement = new ItemMessurement(
             GuidGenerator.Create(),
             itemId, code, version
             );

            return await _itemMessurementRepository.InsertAsync(itemMessurement);
        }

        public virtual async Task<ItemMessurement> UpdateAsync(
            Guid id,
            Guid itemId, string code, string? version = null
        )
        {
            Check.NotNull(itemId, nameof(itemId));
            Check.NotNullOrWhiteSpace(code, nameof(code));

            var itemMessurement = await _itemMessurementRepository.GetAsync(id);

            itemMessurement.ItemId = itemId;
            itemMessurement.Code = code;
            itemMessurement.Version = version;

            return await _itemMessurementRepository.UpdateAsync(itemMessurement);
        }

    }
}