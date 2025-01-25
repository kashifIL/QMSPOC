using Volo.Abp.Application.Dtos;
using System;

namespace QMSPOC.ItemMeasuremetnDetails
{
    public class GetItemMeasuremetnDetailsInput : PagedAndSortedResultRequestDto
    {
        public Guid? ItemMessurementId { get; set; }

        public string? FilterText { get; set; }

        public string? Type { get; set; }
        public decimal? ValueMin { get; set; }
        public decimal? ValueMax { get; set; }
        public string? Uom { get; set; }

        public GetItemMeasuremetnDetailsInput()
        {

        }
    }
}