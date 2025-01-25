using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;

namespace QMSPOC.ItemMeasuremetnDetails
{
    public class ItemMeasuremetnDetailDto : EntityDto<Guid>
    {
        public Guid ItemMessurementId { get; set; }
        public string? Type { get; set; }
        public decimal Value { get; set; }
        public string? Uom { get; set; }

    }
}