using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QMSPOC.ItemMeasuremetnDetails
{
    public interface IItemMeasuremetnDetailsAppService : IApplicationService
    {

        Task<PagedResultDto<ItemMeasuremetnDetailDto>> GetListByItemMessurementIdAsync(GetItemMeasuremetnDetailListInput input);

        Task<PagedResultDto<ItemMeasuremetnDetailDto>> GetListAsync(GetItemMeasuremetnDetailsInput input);

        Task<ItemMeasuremetnDetailDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<ItemMeasuremetnDetailDto> CreateAsync(ItemMeasuremetnDetailCreateDto input);

        Task<ItemMeasuremetnDetailDto> UpdateAsync(Guid id, ItemMeasuremetnDetailUpdateDto input);
    }
}