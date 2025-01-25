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
using QMSPOC.ItemBoms;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using QMSPOC.Shared;

namespace QMSPOC.ItemBoms
{

    [Authorize(QMSPOCPermissions.ItemBoms.Default)]
    public class ItemBomsAppService : QMSPOCAppService, IItemBomsAppService
    {
        protected IDistributedCache<ItemBomDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IItemBomRepository _itemBomRepository;
        protected ItemBomManager _itemBomManager;

        protected IRepository<QMSPOC.Items.Item, Guid> _itemRepository;

        public ItemBomsAppService(IItemBomRepository itemBomRepository, ItemBomManager itemBomManager, IDistributedCache<ItemBomDownloadTokenCacheItem, string> downloadTokenCache, IRepository<QMSPOC.Items.Item, Guid> itemRepository)
        {
            _downloadTokenCache = downloadTokenCache;
            _itemBomRepository = itemBomRepository;
            _itemBomManager = itemBomManager; _itemRepository = itemRepository;

        }

        public virtual async Task<PagedResultDto<ItemBomWithNavigationPropertiesDto>> GetListAsync(GetItemBomsInput input)
        {
            var totalCount = await _itemBomRepository.GetCountAsync(input.FilterText, input.Code, input.VersionMin, input.VersionMax, input.Description, input.ItemId);
            var items = await _itemBomRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Code, input.VersionMin, input.VersionMax, input.Description, input.ItemId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ItemBomWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ItemBomWithNavigationProperties>, List<ItemBomWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<ItemBomWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<ItemBomWithNavigationProperties, ItemBomWithNavigationPropertiesDto>
                (await _itemBomRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<ItemBomDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<ItemBom, ItemBomDto>(await _itemBomRepository.GetAsync(id));
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

        [Authorize(QMSPOCPermissions.ItemBoms.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _itemBomRepository.DeleteAsync(id);
        }

        [Authorize(QMSPOCPermissions.ItemBoms.Create)]
        public virtual async Task<ItemBomDto> CreateAsync(ItemBomCreateDto input)
        {
            if (input.ItemId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Item"]]);
            }

            var itemBom = await _itemBomManager.CreateAsync(
            input.ItemId, input.Code, input.Version, input.Description
            );

            return ObjectMapper.Map<ItemBom, ItemBomDto>(itemBom);
        }

        [Authorize(QMSPOCPermissions.ItemBoms.Edit)]
        public virtual async Task<ItemBomDto> UpdateAsync(Guid id, ItemBomUpdateDto input)
        {
            if (input.ItemId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Item"]]);
            }

            var itemBom = await _itemBomManager.UpdateAsync(
            id,
            input.ItemId, input.Code, input.Version, input.Description
            );

            return ObjectMapper.Map<ItemBom, ItemBomDto>(itemBom);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ItemBomExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var itemBoms = await _itemBomRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Code, input.VersionMin, input.VersionMax, input.Description, input.ItemId);
            var items = itemBoms.Select(item => new
            {
                Code = item.ItemBom.Code,
                Version = item.ItemBom.Version,
                Description = item.ItemBom.Description,

                Item = item.Item?.Code,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "ItemBoms.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(QMSPOCPermissions.ItemBoms.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> itembomIds)
        {
            await _itemBomRepository.DeleteManyAsync(itembomIds);
        }

        [Authorize(QMSPOCPermissions.ItemBoms.Delete)]
        public virtual async Task DeleteAllAsync(GetItemBomsInput input)
        {
            await _itemBomRepository.DeleteAllAsync(input.FilterText, input.Code, input.VersionMin, input.VersionMax, input.Description, input.ItemId);
        }
        public virtual async Task<QMSPOC.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new ItemBomDownloadTokenCacheItem { Token = token },
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