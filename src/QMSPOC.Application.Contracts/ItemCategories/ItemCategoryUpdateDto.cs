using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace QMSPOC.ItemCategories
{
    public class ItemCategoryUpdateDto
    {
        [Required]
        public string Code { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;

    }
}