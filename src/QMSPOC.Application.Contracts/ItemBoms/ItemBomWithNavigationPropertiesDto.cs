using QMSPOC.Items;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace QMSPOC.ItemBoms
{
    public class ItemBomWithNavigationPropertiesDto
    {
        public ItemBomDto ItemBom { get; set; } = null!;

        public ItemDto Item { get; set; } = null!;

    }
}