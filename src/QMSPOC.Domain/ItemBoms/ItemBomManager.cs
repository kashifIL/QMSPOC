using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace QMSPOC.ItemBoms
{
    public class ItemBomManager : DomainService
    {
        protected IItemBomRepository _itemBomRepository;

        public ItemBomManager(IItemBomRepository itemBomRepository)
        {
            _itemBomRepository = itemBomRepository;
        }

        public virtual async Task<ItemBom> CreateAsync(
        Guid itemId, string code, int version, string? description = null)
        {
            Check.NotNull(itemId, nameof(itemId));
            Check.NotNullOrWhiteSpace(code, nameof(code));

            var itemBom = new ItemBom(
             GuidGenerator.Create(),
             itemId, code, version, description
             );

            return await _itemBomRepository.InsertAsync(itemBom);
        }

        public virtual async Task<ItemBom> UpdateAsync(
            Guid id,
            Guid itemId, string code, int version, string? description = null
        )
        {
            Check.NotNull(itemId, nameof(itemId));
            Check.NotNullOrWhiteSpace(code, nameof(code));

            var itemBom = await _itemBomRepository.GetAsync(id);

            itemBom.ItemId = itemId;
            itemBom.Code = code;
            itemBom.Version = version;
            itemBom.Description = description;

            return await _itemBomRepository.UpdateAsync(itemBom);
        }

    }
}