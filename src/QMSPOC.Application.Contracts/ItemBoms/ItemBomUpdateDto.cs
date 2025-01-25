using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace QMSPOC.ItemBoms
{
    public class ItemBomUpdateDto
    {
        [Required]
        public string Code { get; set; } = null!;
        public int Version { get; set; }
        public string? Description { get; set; }
        public Guid ItemId { get; set; }

    }
}