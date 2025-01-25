using QMSPOC.Shared;
using QMSPOC.ItemCategories;
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
using QMSPOC.Items;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using QMSPOC.Shared;

namespace QMSPOC.Items
{

    [Authorize(QMSPOCPermissions.Items.Default)]
    public class ItemsAppService : QMSPOCAppService, IItemsAppService
    {
        protected IDistributedCache<ItemDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IItemRepository _itemRepository;
        protected ItemManager _itemManager;

        protected IRepository<QMSPOC.ItemCategories.ItemCategory, Guid> _itemCategoryRepository;

        public ItemsAppService(IItemRepository itemRepository, ItemManager itemManager, IDistributedCache<ItemDownloadTokenCacheItem, string> downloadTokenCache, IRepository<QMSPOC.ItemCategories.ItemCategory, Guid> itemCategoryRepository)
        {
            _downloadTokenCache = downloadTokenCache;
            _itemRepository = itemRepository;
            _itemManager = itemManager; _itemCategoryRepository = itemCategoryRepository;

        }

        public virtual async Task<PagedResultDto<ItemWithNavigationPropertiesDto>> GetListAsync(GetItemsInput input)
        {
            var totalCount = await _itemRepository.GetCountAsync(input.FilterText, input.Code, input.Description, input.ItemCategoryId);
            var items = await _itemRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Code, input.Description, input.ItemCategoryId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ItemWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ItemWithNavigationProperties>, List<ItemWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<ItemWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<ItemWithNavigationProperties, ItemWithNavigationPropertiesDto>
                (await _itemRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<ItemDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Item, ItemDto>(await _itemRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetItemCategoryLookupAsync(LookupRequestDto input)
        {
            var query = (await _itemCategoryRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.Code != null &&
                         x.Code.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<QMSPOC.ItemCategories.ItemCategory>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<QMSPOC.ItemCategories.ItemCategory>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [Authorize(QMSPOCPermissions.Items.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _itemRepository.DeleteAsync(id);
        }

        [Authorize(QMSPOCPermissions.Items.Create)]
        public virtual async Task<ItemDto> CreateAsync(ItemCreateDto input)
        {
            if (input.ItemCategoryId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["ItemCategory"]]);
            }

            var item = await _itemManager.CreateAsync(
            input.ItemCategoryId, input.Code, input.Description
            );

            return ObjectMapper.Map<Item, ItemDto>(item);
        }

        [Authorize(QMSPOCPermissions.Items.Edit)]
        public virtual async Task<ItemDto> UpdateAsync(Guid id, ItemUpdateDto input)
        {
            if (input.ItemCategoryId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["ItemCategory"]]);
            }

            var item = await _itemManager.UpdateAsync(
            id,
            input.ItemCategoryId, input.Code, input.Description
            );

            return ObjectMapper.Map<Item, ItemDto>(item);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ItemExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _itemRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Code, input.Description, input.ItemCategoryId);
            var item = items.Select(item => new
            {
                Code = item.Item.Code,
                Description = item.Item.Description,

                ItemCategory = item.ItemCategory?.Code,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Items.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(QMSPOCPermissions.Items.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> itemIds)
        {
            await _itemRepository.DeleteManyAsync(itemIds);
        }

        [Authorize(QMSPOCPermissions.Items.Delete)]
        public virtual async Task DeleteAllAsync(GetItemsInput input)
        {
            await _itemRepository.DeleteAllAsync(input.FilterText, input.Code, input.Description, input.ItemCategoryId);
        }
        public virtual async Task<QMSPOC.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new ItemDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new QMSPOC.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}