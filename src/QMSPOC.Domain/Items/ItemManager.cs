using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace QMSPOC.Items
{
    public class ItemManager : DomainService
    {
        protected IItemRepository _itemRepository;

        public ItemManager(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public virtual async Task<Item> CreateAsync(
        Guid itemCategoryId, string code, string description)
        {
            Check.NotNull(itemCategoryId, nameof(itemCategoryId));
            Check.NotNullOrWhiteSpace(code, nameof(code));
            Check.NotNullOrWhiteSpace(description, nameof(description));

            var item = new Item(
             GuidGenerator.Create(),
             itemCategoryId, code, description
             );

            return await _itemRepository.InsertAsync(item);
        }

        public virtual async Task<Item> UpdateAsync(
            Guid id,
            Guid itemCategoryId, string code, string description
        )
        {
            Check.NotNull(itemCategoryId, nameof(itemCategoryId));
            Check.NotNullOrWhiteSpace(code, nameof(code));
            Check.NotNullOrWhiteSpace(description, nameof(description));

            var item = await _itemRepository.GetAsync(id);

            item.ItemCategoryId = itemCategoryId;
            item.Code = code;
            item.Description = description;

            return await _itemRepository.UpdateAsync(item);
        }

    }
}