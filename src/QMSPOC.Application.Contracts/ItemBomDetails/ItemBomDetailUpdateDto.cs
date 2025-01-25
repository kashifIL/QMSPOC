using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace QMSPOC.ItemBomDetails
{
    public class ItemBomDetailUpdateDto
    {
        public Guid ItemBomId { get; set; }
        public decimal Qty { get; set; }
        public string? Uom { get; set; }
        public Guid ItemId { get; set; }

    }
}