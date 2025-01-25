using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using QMSPOC.Shared;

namespace QMSPOC.ItemCategories
{
    public interface IItemCategoriesAppService : IApplicationService
    {

        Task<PagedResultDto<ItemCategoryDto>> GetListAsync(GetItemCategoriesInput input);

        Task<ItemCategoryDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<ItemCategoryDto> CreateAsync(ItemCategoryCreateDto input);

        Task<ItemCategoryDto> UpdateAsync(Guid id, ItemCategoryUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ItemCategoryExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> itemcategoryIds);

        Task DeleteAllAsync(GetItemCategoriesInput input);
        Task<QMSPOC.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    }
}