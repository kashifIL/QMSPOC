using Volo.Abp.Application.Dtos;
using System;

namespace QMSPOC.ItemMeasuremetnDetails
{
    public class GetItemMeasuremetnDetailListInput : PagedAndSortedResultRequestDto
    {
        public Guid ItemMessurementId { get; set; }
    }
}