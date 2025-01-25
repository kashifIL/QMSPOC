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
using QMSPOC.ItemMessurements;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using QMSPOC.Shared;

namespace QMSPOC.ItemMessurements
{

    [Authorize(QMSPOCPermissions.ItemMessurements.Default)]
    public class ItemMessurementsAppService : QMSPOCAppService, IItemMessurementsAppService
    {
        protected IDistributedCache<ItemMessurementDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IItemMessurementRepository _itemMessurementRepository;
        protected ItemMessurementManager _itemMessurementManager;

        protected IRepository<QMSPOC.Items.Item, Guid> _itemRepository;

        public ItemMessurementsAppService(IItemMessurementRepository itemMessurementRepository, ItemMessurementManager itemMessurementManager, IDistributedCache<ItemMessurementDownloadTokenCacheItem, string> downloadTokenCache, IRepository<QMSPOC.Items.Item, Guid> itemRepository)
        {
            _downloadTokenCache = downloadTokenCache;
            _itemMessurementRepository = itemMessurementRepository;
            _itemMessurementManager = itemMessurementManager; _itemRepository = itemRepository;

        }

        public virtual async Task<PagedResultDto<ItemMessurementWithNavigationPropertiesDto>> GetListAsync(GetItemMessurementsInput input)
        {
            var totalCount = await _itemMessurementRepository.GetCountAsync(input.FilterText, input.Code, input.Version, input.ItemId);
            var items = await _itemMessurementRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Code, input.Version, input.ItemId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ItemMessurementWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ItemMessurementWithNavigationProperties>, List<ItemMessurementWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<ItemMessurementWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<ItemMessurementWithNavigationProperties, ItemMessurementWithNavigationPropertiesDto>
                (await _itemMessurementRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<ItemMessurementDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<ItemMessurement, ItemMessurementDto>(await _itemMessurementRepository.GetAsync(id));
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

        [Authorize(QMSPOCPermissions.ItemMessurements.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _itemMessurementRepository.DeleteAsync(id);
        }

        [Authorize(QMSPOCPermissions.ItemMessurements.Create)]
        public virtual async Task<ItemMessurementDto> CreateAsync(ItemMessurementCreateDto input)
        {
            if (input.ItemId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Item"]]);
            }

            var itemMessurement = await _itemMessurementManager.CreateAsync(
            input.ItemId, input.Code, input.Version
            );

            return ObjectMapper.Map<ItemMessurement, ItemMessurementDto>(itemMessurement);
        }

        [Authorize(QMSPOCPermissions.ItemMessurements.Edit)]
        public virtual async Task<ItemMessurementDto> UpdateAsync(Guid id, ItemMessurementUpdateDto input)
        {
            if (input.ItemId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Item"]]);
            }

            var itemMessurement = await _itemMessurementManager.UpdateAsync(
            id,
            input.ItemId, input.Code, input.Version
            );

            return ObjectMapper.Map<ItemMessurement, ItemMessurementDto>(itemMessurement);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ItemMessurementExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var itemMessurements = await _itemMessurementRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Code, input.Version, input.ItemId);
            var items = itemMessurements.Select(item => new
            {
                Code = item.ItemMessurement.Code,
                Version = item.ItemMessurement.Version,

                Item = item.Item?.Code,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "ItemMessurements.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(QMSPOCPermissions.ItemMessurements.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> itemmessurementIds)
        {
            await _itemMessurementRepository.DeleteManyAsync(itemmessurementIds);
        }

        [Authorize(QMSPOCPermissions.ItemMessurements.Delete)]
        public virtual async Task DeleteAllAsync(GetItemMessurementsInput input)
        {
            await _itemMessurementRepository.DeleteAllAsync(input.FilterText, input.Code, input.Version, input.ItemId);
        }
        public virtual async Task<QMSPOC.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new ItemMessurementDownloadTokenCacheItem { Token = token },
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