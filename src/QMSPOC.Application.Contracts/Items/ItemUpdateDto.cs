using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace QMSPOC.Items
{
    public class ItemUpdateDto
    {
        [Required]
        public string Code { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        public Guid ItemCategoryId { get; set; }

    }
}