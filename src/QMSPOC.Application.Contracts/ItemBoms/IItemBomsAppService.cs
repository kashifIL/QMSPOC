using QMSPOC.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using QMSPOC.Shared;

namespace QMSPOC.ItemBoms
{
    public interface IItemBomsAppService : IApplicationService
    {

        Task<PagedResultDto<ItemBomWithNavigationPropertiesDto>> GetListAsync(GetItemBomsInput input);

        Task<ItemBomWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<ItemBomDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetItemLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<ItemBomDto> CreateAsync(ItemBomCreateDto input);

        Task<ItemBomDto> UpdateAsync(Guid id, ItemBomUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ItemBomExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> itembomIds);

        Task DeleteAllAsync(GetItemBomsInput input);
        Task<QMSPOC.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}