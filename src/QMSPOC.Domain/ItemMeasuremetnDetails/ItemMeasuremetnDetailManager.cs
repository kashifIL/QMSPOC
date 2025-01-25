using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace QMSPOC.ItemMeasuremetnDetails
{
    public class ItemMeasuremetnDetailManager : DomainService
    {
        protected IItemMeasuremetnDetailRepository _itemMeasuremetnDetailRepository;

        public ItemMeasuremetnDetailManager(IItemMeasuremetnDetailRepository itemMeasuremetnDetailRepository)
        {
            _itemMeasuremetnDetailRepository = itemMeasuremetnDetailRepository;
        }

        public virtual async Task<ItemMeasuremetnDetail> CreateAsync(
        Guid itemMessurementId, decimal value, string? type = null, string? uom = null)
        {

            var itemMeasuremetnDetail = new ItemMeasuremetnDetail(
             GuidGenerator.Create(),
             itemMessurementId, value, type, uom
             );

            return await _itemMeasuremetnDetailRepository.InsertAsync(itemMeasuremetnDetail);
        }

        public virtual async Task<ItemMeasuremetnDetail> UpdateAsync(
            Guid id,
            Guid itemMessurementId, decimal value, string? type = null, string? uom = null
        )
        {

            var itemMeasuremetnDetail = await _itemMeasuremetnDetailRepository.GetAsync(id);

            itemMeasuremetnDetail.ItemMessurementId = itemMessurementId;
            itemMeasuremetnDetail.Value = value;
            itemMeasuremetnDetail.Type = type;
            itemMeasuremetnDetail.Uom = uom;

            return await _itemMeasuremetnDetailRepository.UpdateAsync(itemMeasuremetnDetail);
        }

    }
}