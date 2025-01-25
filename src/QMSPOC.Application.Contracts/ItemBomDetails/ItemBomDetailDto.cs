using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;

namespace QMSPOC.ItemBomDetails
{
    public class ItemBomDetailDto : EntityDto<Guid>
    {
        public Guid ItemBomId { get; set; }
        public decimal Qty { get; set; }
        public string? Uom { get; set; }
        public Guid ItemId { get; set; }

    }
}