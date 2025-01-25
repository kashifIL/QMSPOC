using QMSPOC.Items;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace QMSPOC.ItemMessurements
{
    public class ItemMessurementWithNavigationPropertiesDto
    {
        public ItemMessurementDto ItemMessurement { get; set; } = null!;

        public ItemDto Item { get; set; } = null!;

    }
}