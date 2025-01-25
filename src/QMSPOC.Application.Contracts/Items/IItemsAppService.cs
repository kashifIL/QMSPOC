using QMSPOC.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using QMSPOC.Shared;

namespace QMSPOC.Items
{
    public interface IItemsAppService : IApplicationService
    {

        Task<PagedResultDto<ItemWithNavigationPropertiesDto>> GetListAsync(GetItemsInput input);

        Task<ItemWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<ItemDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetItemCategoryLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<ItemDto> CreateAsync(ItemCreateDto input);

        Task<ItemDto> UpdateAsync(Guid id, ItemUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ItemExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> itemIds);

        Task DeleteAllAsync(GetItemsInput input);
        Task<QMSPOC.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}