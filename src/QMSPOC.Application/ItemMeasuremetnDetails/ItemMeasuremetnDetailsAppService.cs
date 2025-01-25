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
using QMSPOC.ItemMeasuremetnDetails;

namespace QMSPOC.ItemMeasuremetnDetails
{

    [Authorize(QMSPOCPermissions.ItemMeasuremetnDetails.Default)]
    public class ItemMeasuremetnDetailsAppService : QMSPOCAppService, IItemMeasuremetnDetailsAppService
    {

        protected IItemMeasuremetnDetailRepository _itemMeasuremetnDetailRepository;
        protected ItemMeasuremetnDetailManager _itemMeasuremetnDetailManager;

        public ItemMeasuremetnDetailsAppService(IItemMeasuremetnDetailRepository itemMeasuremetnDetailRepository, ItemMeasuremetnDetailManager itemMeasuremetnDetailManager)
        {

            _itemMeasuremetnDetailRepository = itemMeasuremetnDetailRepository;
            _itemMeasuremetnDetailManager = itemMeasuremetnDetailManager;

        }

        public virtual async Task<PagedResultDto<ItemMeasuremetnDetailDto>> GetListByItemMessurementIdAsync(GetItemMeasuremetnDetailListInput input)
        {
            var itemMeasuremetnDetails = await _itemMeasuremetnDetailRepository.GetListByItemMessurementIdAsync(
                input.ItemMessurementId,
                input.Sorting,
                input.MaxResultCount,
                input.SkipCount);

            return new PagedResultDto<ItemMeasuremetnDetailDto>
            {
                TotalCount = await _itemMeasuremetnDetailRepository.GetCountByItemMessurementIdAsync(input.ItemMessurementId),
                Items = ObjectMapper.Map<List<ItemMeasuremetnDetail>, List<ItemMeasuremetnDetailDto>>(itemMeasuremetnDetails)
            };
        }

        public virtual async Task<PagedResultDto<ItemMeasuremetnDetailDto>> GetListAsync(GetItemMeasuremetnDetailsInput input)
        {
            var totalCount = await _itemMeasuremetnDetailRepository.GetCountAsync(input.FilterText, input.Type, input.ValueMin, input.ValueMax, input.Uom);
            var items = await _itemMeasuremetnDetailRepository.GetListAsync(input.FilterText, input.Type, input.ValueMin, input.ValueMax, input.Uom, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ItemMeasuremetnDetailDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ItemMeasuremetnDetail>, List<ItemMeasuremetnDetailDto>>(items)
            };
        }

        public virtual async Task<ItemMeasuremetnDetailDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<ItemMeasuremetnDetail, ItemMeasuremetnDetailDto>(await _itemMeasuremetnDetailRepository.GetAsync(id));
        }

        [Authorize(QMSPOCPermissions.ItemMeasuremetnDetails.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _itemMeasuremetnDetailRepository.DeleteAsync(id);
        }

        [Authorize(QMSPOCPermissions.ItemMeasuremetnDetails.Create)]
        public virtual async Task<ItemMeasuremetnDetailDto> CreateAsync(ItemMeasuremetnDetailCreateDto input)
        {

            var itemMeasuremetnDetail = await _itemMeasuremetnDetailManager.CreateAsync(input.ItemMessurementId
            , input.Value, input.Type, input.Uom
            );

            return ObjectMapper.Map<ItemMeasuremetnDetail, ItemMeasuremetnDetailDto>(itemMeasuremetnDetail);
        }

        [Authorize(QMSPOCPermissions.ItemMeasuremetnDetails.Edit)]
        public virtual async Task<ItemMeasuremetnDetailDto> UpdateAsync(Guid id, ItemMeasuremetnDetailUpdateDto input)
        {

            var itemMeasuremetnDetail = await _itemMeasuremetnDetailManager.UpdateAsync(
            id, input.ItemMessurementId
            , input.Value, input.Type, input.Uom
            );

            return ObjectMapper.Map<ItemMeasuremetnDetail, ItemMeasuremetnDetailDto>(itemMeasuremetnDetail);
        }
    }
}