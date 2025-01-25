using System;
using System.Collections.Generic;
using QMSPOC.ItemBomDetails;
using Volo.Abp.Application.Dtos;

namespace QMSPOC.ItemBoms
{
    public class ItemBomDto : EntityDto<Guid>
    {
        public string Code { get; set; } = null!;
        public int Version { get; set; }
        public string? Description { get; set; }
        public Guid ItemId { get; set; }

        public List<ItemBomDetailWithNavigationPropertiesDto> ItemBomDetails { get; set; } = new();
    }
}