using Volo.Abp.Application.Dtos;
using System;

namespace QMSPOC.ItemMessurements
{
    public class ItemMessurementExcelDownloadDto
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? Code { get; set; }
        public string? Version { get; set; }
        public Guid? ItemId { get; set; }

        public ItemMessurementExcelDownloadDto()
        {

        }
    }
}