using QMSPOC.ItemCategories;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace QMSPOC.Items
{
    public class ItemWithNavigationPropertiesDto
    {
        public ItemDto Item { get; set; } = null!;

        public ItemCategoryDto ItemCategory { get; set; } = null!;

    }
}