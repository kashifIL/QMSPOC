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
using QMSPOC.ItemCategories;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using QMSPOC.Shared;

namespace QMSPOC.ItemCategories
{

    [Authorize(QMSPOCPermissions.ItemCategories.Default)]
    public class ItemCategoriesAppService : QMSPOCAppService, IItemCategoriesAppService
    {
        protected IDistributedCache<ItemCategoryDownloadTokenCacheItem, string> _downloadTokenCache;
        protected IItemCategoryRepository _itemCategoryRepository;
        protected ItemCategoryManager _itemCategoryManager;

        public ItemCategoriesAppService(IItemCategoryRepository itemCategoryRepository, ItemCategoryManager itemCategoryManager, IDistributedCache<ItemCategoryDownloadTokenCacheItem, string> downloadTokenCache)
        {
            _downloadTokenCache = downloadTokenCache;
            _itemCategoryRepository = itemCategoryRepository;
            _itemCategoryManager = itemCategoryManager;

        }

        public virtual async Task<PagedResultDto<ItemCategoryDto>> GetListAsync(GetItemCategoriesInput input)
        {
            var totalCount = await _itemCategoryRepository.GetCountAsync(input.FilterText, input.Code, input.Name);
            var items = await _itemCategoryRepository.GetListAsync(input.FilterText, input.Code, input.Name, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ItemCategoryDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ItemCategory>, List<ItemCategoryDto>>(items)
            };
        }

        public virtual async Task<ItemCategoryDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<ItemCategory, ItemCategoryDto>(await _itemCategoryRepository.GetAsync(id));
        }

        [Authorize(QMSPOCPermissions.ItemCategories.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _itemCategoryRepository.DeleteAsync(id);
        }

        [Authorize(QMSPOCPermissions.ItemCategories.Create)]
        public virtual async Task<ItemCategoryDto> CreateAsync(ItemCategoryCreateDto input)
        {

            var itemCategory = await _itemCategoryManager.CreateAsync(
            input.Code, input.Name
            );

            return ObjectMapper.Map<ItemCategory, ItemCategoryDto>(itemCategory);
        }

        [Authorize(QMSPOCPermissions.ItemCategories.Edit)]
        public virtual async Task<ItemCategoryDto> UpdateAsync(Guid id, ItemCategoryUpdateDto input)
        {

            var itemCategory = await _itemCategoryManager.UpdateAsync(
            id,
            input.Code, input.Name
            );

            return ObjectMapper.Map<ItemCategory, ItemCategoryDto>(itemCategory);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ItemCategoryExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _itemCategoryRepository.GetListAsync(input.FilterText, input.Code, input.Name);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<ItemCategory>, List<ItemCategoryExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "ItemCategories.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(QMSPOCPermissions.ItemCategories.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> itemcategoryIds)
        {
            await _itemCategoryRepository.DeleteManyAsync(itemcategoryIds);
        }

        [Authorize(QMSPOCPermissions.ItemCategories.Delete)]
        public virtual async Task DeleteAllAsync(GetItemCategoriesInput input)
        {
            await _itemCategoryRepository.DeleteAllAsync(input.FilterText, input.Code, input.Name);
        }
        public virtual async Task<QMSPOC.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new ItemCategoryDownloadTokenCacheItem { Token = token },
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