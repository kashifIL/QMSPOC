using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace QMSPOC.ItemCategories
{
    public class ItemCategoryManager : DomainService
    {
        protected IItemCategoryRepository _itemCategoryRepository;

        public ItemCategoryManager(IItemCategoryRepository itemCategoryRepository)
        {
            _itemCategoryRepository = itemCategoryRepository;
        }

        public virtual async Task<ItemCategory> CreateAsync(
        string code, string name)
        {
            Check.NotNullOrWhiteSpace(code, nameof(code));
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var itemCategory = new ItemCategory(
             GuidGenerator.Create(),
             code, name
             );

            return await _itemCategoryRepository.InsertAsync(itemCategory);
        }

        public virtual async Task<ItemCategory> UpdateAsync(
            Guid id,
            string code, string name
        )
        {
            Check.NotNullOrWhiteSpace(code, nameof(code));
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var itemCategory = await _itemCategoryRepository.GetAsync(id);

            itemCategory.Code = code;
            itemCategory.Name = name;

            return await _itemCategoryRepository.UpdateAsync(itemCategory);
        }

    }
}