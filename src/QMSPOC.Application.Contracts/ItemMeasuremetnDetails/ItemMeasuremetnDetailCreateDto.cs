using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace QMSPOC.ItemMeasuremetnDetails
{
    public class ItemMeasuremetnDetailCreateDto
    {
        public Guid ItemMessurementId { get; set; }
        public string? Type { get; set; }
        public decimal Value { get; set; }
        public string? Uom { get; set; }
    }
}