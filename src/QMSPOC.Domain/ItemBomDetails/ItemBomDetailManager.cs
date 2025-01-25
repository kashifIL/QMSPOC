using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace QMSPOC.ItemBomDetails
{
    public class ItemBomDetailManager : DomainService
    {
        protected IItemBomDetailRepository _itemBomDetailRepository;

        public ItemBomDetailManager(IItemBomDetailRepository itemBomDetailRepository)
        {
            _itemBomDetailRepository = itemBomDetailRepository;
        }

        public virtual async Task<ItemBomDetail> CreateAsync(
        Guid itemBomId, Guid itemId, decimal qty, string? uom = null)
        {
            Check.NotNull(itemId, nameof(itemId));

            var itemBomDetail = new ItemBomDetail(
             GuidGenerator.Create(),
             itemBomId, itemId, qty, uom
             );

            return await _itemBomDetailRepository.InsertAsync(itemBomDetail);
        }

        public virtual async Task<ItemBomDetail> UpdateAsync(
            Guid id,
            Guid itemBomId, Guid itemId, decimal qty, string? uom = null
        )
        {
            Check.NotNull(itemId, nameof(itemId));

            var itemBomDetail = await _itemBomDetailRepository.GetAsync(id);

            itemBomDetail.ItemBomId = itemBomId;
            itemBomDetail.ItemId = itemId;
            itemBomDetail.Qty = qty;
            itemBomDetail.Uom = uom;

            return await _itemBomDetailRepository.UpdateAsync(itemBomDetail);
        }

    }
}