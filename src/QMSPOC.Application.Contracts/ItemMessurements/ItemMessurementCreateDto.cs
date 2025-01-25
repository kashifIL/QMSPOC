using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace QMSPOC.ItemMessurements
{
    public class ItemMessurementCreateDto
    {
        [Required]
        public string Code { get; set; } = null!;
        public string? Version { get; set; }
        public Guid ItemId { get; set; }
    }
}