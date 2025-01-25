using QMSPOC.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using QMSPOC.Shared;

namespace QMSPOC.ItemMessurements
{
    public interface IItemMessurementsAppService : IApplicationService
    {

        Task<PagedResultDto<ItemMessurementWithNavigationPropertiesDto>> GetListAsync(GetItemMessurementsInput input);

        Task<ItemMessurementWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<ItemMessurementDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetItemLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<ItemMessurementDto> CreateAsync(ItemMessurementCreateDto input);

        Task<ItemMessurementDto> UpdateAsync(Guid id, ItemMessurementUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ItemMessurementExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> itemmessurementIds);

        Task DeleteAllAsync(GetItemMessurementsInput input);
        Task<QMSPOC.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}