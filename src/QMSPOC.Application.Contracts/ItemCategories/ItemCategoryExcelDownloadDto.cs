using Volo.Abp.Application.Dtos;
using System;

namespace QMSPOC.ItemCategories
{
    public class ItemCategoryExcelDownloadDto
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? Code { get; set; }
        public string? Name { get; set; }

        public ItemCategoryExcelDownloadDto()
        {

        }
    }
}