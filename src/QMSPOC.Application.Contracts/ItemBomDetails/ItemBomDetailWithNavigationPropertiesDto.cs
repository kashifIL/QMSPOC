using QMSPOC.Items;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace QMSPOC.ItemBomDetails
{
    public class ItemBomDetailWithNavigationPropertiesDto
    {
        public ItemBomDetailDto ItemBomDetail { get; set; } = null!;

        public ItemDto Item { get; set; } = null!;

    }
}