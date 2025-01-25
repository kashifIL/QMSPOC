using QMSPOC.Shared;
using QMSPOC.Items;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using QMSPOC.Permissions;
using QMSPOC.ItemBomDetails;

namespace QMSPOC.ItemBomDetails
{

    [Authorize(QMSPOCPermissions.ItemBomDetails.Default)]
    public class ItemBomDetailsAppService : QMSPOCAppService, IItemBomDetailsAppService
    {

        protected IItemBomDetailRepository _itemBomDetailRepository;
        protected ItemBomDetailManager _itemBomDetailManager;

        protected IRepository<QMSPOC.Items.Item, Guid> _itemRepository;

        public ItemBomDetailsAppService(IItemBomDetailRepository itemBomDetailRepository, ItemBomDetailManager itemBomDetailManager, IRepository<QMSPOC.Items.Item, Guid> itemRepository)
        {

            _itemBomDetailRepository = itemBomDetailRepository;
            _itemBomDetailManager = itemBomDetailManager; _itemRepository = itemRepository;

        }

        public virtual async Task<PagedResultDto<ItemBomDetailDto>> GetListByItemBomIdAsync(GetItemBomDetailListInput input)
        {
            var itemBomDetails = await _itemBomDetailRepository.GetListByItemBomIdAsync(
                input.ItemBomId,
                input.Sorting,
                input.MaxResultCount,
                input.SkipCount);

            return new PagedResultDto<ItemBomDetailDto>
            {
                TotalCount = await _itemBomDetailRepository.GetCountByItemBomIdAsync(input.ItemBomId),
                Items = ObjectMapper.Map<List<ItemBomDetail>, List<ItemBomDetailDto>>(itemBomDetails)
            };
        }
        public virtual async Task<PagedResultDto<ItemBomDetailWithNavigationPropertiesDto>> GetListWithNavigationPropertiesByItemBomIdAsync(GetItemBomDetailListInput input)
        {
            var itemBomDetails = await _itemBomDetailRepository.GetListWithNavigationPropertiesByItemBomIdAsync(
                input.ItemBomId,
                input.Sorting,
                input.MaxResultCount,
                input.SkipCount);

            return new PagedResultDto<ItemBomDetailWithNavigationPropertiesDto>
            {
                TotalCount = await _itemBomDetailRepository.GetCountByItemBomIdAsync(input.ItemBomId),
                Items = ObjectMapper.Map<List<ItemBomDetailWithNavigationProperties>, List<ItemBomDetailWithNavigationPropertiesDto>>(itemBomDetails)
            };
        }

        public virtual async Task<PagedResultDto<ItemBomDetailWithNavigationPropertiesDto>> GetListAsync(GetItemBomDetailsInput input)
        {
            var totalCount = await _itemBomDetailRepository.GetCountAsync(input.FilterText, input.QtyMin, input.QtyMax, input.Uom, input.ItemId);
            var items = await _itemBomDetailRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.QtyMin, input.QtyMax, input.Uom, input.ItemId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ItemBomDetailWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ItemBomDetailWithNavigationProperties>, List<ItemBomDetailWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<ItemBomDetailWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<ItemBomDetailWithNavigationProperties, ItemBomDetailWithNavigationPropertiesDto>
                (await _itemBomDetailRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<ItemBomDetailDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<ItemBomDetail, ItemBomDetailDto>(await _itemBomDetailRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetItemLookupAsync(LookupRequestDto input)
        {
            var query = (await _itemRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Code != null &&
                         x.Code.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<QMSPOC.Items.Item>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<QMSPOC.Items.Item>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [Authorize(QMSPOCPermissions.ItemBomDetails.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _itemBomDetailRepository.DeleteAsync(id);
        }

        [Authorize(QMSPOCPermissions.ItemBomDetails.Create)]
        public virtual async Task<ItemBomDetailDto> CreateAsync(ItemBomDetailCreateDto input)
        {
            if (input.ItemId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Item"]]);
            }

            var itemBomDetail = await _itemBomDetailManager.CreateAsync(input.ItemBomId
            , input.ItemId, input.Qty, input.Uom
            );

            return ObjectMapper.Map<ItemBomDetail, ItemBomDetailDto>(itemBomDetail);
        }

        [Authorize(QMSPOCPermissions.ItemBomDetails.Edit)]
        public virtual async Task<ItemBomDetailDto> UpdateAsync(Guid id, ItemBomDetailUpdateDto input)
        {
            if (input.ItemId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Item"]]);
            }

            var itemBomDetail = await _itemBomDetailManager.UpdateAsync(
            id, input.ItemBomId
            , input.ItemId, input.Qty, input.Uom
            );

            return ObjectMapper.Map<ItemBomDetail, ItemBomDetailDto>(itemBomDetail);
        }
    }
}