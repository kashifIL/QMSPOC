using System;
using System.Collections.Generic;
using QMSPOC.ItemMeasuremetnDetails;
using Volo.Abp.Application.Dtos;

namespace QMSPOC.ItemMessurements
{
    public class ItemMessurementDto : EntityDto<Guid>
    {
        public string Code { get; set; } = null!;
        public string? Version { get; set; }
        public Guid ItemId { get; set; }

        public List<ItemMeasuremetnDetailDto> ItemMeasuremetnDetails { get; set; } = new();
    }
}