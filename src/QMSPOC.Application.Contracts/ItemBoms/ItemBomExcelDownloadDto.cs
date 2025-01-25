using Volo.Abp.Application.Dtos;
using System;

namespace QMSPOC.ItemBoms
{
    public class ItemBomExcelDownloadDto
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? Code { get; set; }
        public int? VersionMin { get; set; }
        public int? VersionMax { get; set; }
        public string? Description { get; set; }
        public Guid? ItemId { get; set; }

        public ItemBomExcelDownloadDto()
        {

        }
    }
}