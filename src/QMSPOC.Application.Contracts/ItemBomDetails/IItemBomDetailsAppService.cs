using QMSPOC.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QMSPOC.ItemBomDetails
{
    public interface IItemBomDetailsAppService : IApplicationService
    {

        Task<PagedResultDto<ItemBomDetailDto>> GetListByItemBomIdAsync(GetItemBomDetailListInput input);
        Task<PagedResultDto<ItemBomDetailWithNavigationPropertiesDto>> GetListWithNavigationPropertiesByItemBomIdAsync(GetItemBomDetailListInput input);

        Task<PagedResultDto<ItemBomDetailWithNavigationPropertiesDto>> GetListAsync(GetItemBomDetailsInput input);

        Task<ItemBomDetailWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<ItemBomDetailDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetItemLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<ItemBomDetailDto> CreateAsync(ItemBomDetailCreateDto input);

        Task<ItemBomDetailDto> UpdateAsync(Guid id, ItemBomDetailUpdateDto input);
    }
}