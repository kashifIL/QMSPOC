using Volo.Abp.Application.Dtos;
using System;

namespace QMSPOC.Items
{
    public class ItemExcelDownloadDto
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? Code { get; set; }
        public string? Description { get; set; }
        public Guid? ItemCategoryId { get; set; }

        public ItemExcelDownloadDto()
        {

        }
    }
}