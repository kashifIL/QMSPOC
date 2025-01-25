using QMSPOC.Items;

using System;
using System.Collections.Generic;

namespace QMSPOC.ItemBomDetails
{
    public  class ItemBomDetailWithNavigationProperties
    {
        public ItemBomDetail ItemBomDetail { get; set; } = null!;

        public Item Item { get; set; } = null!;
        

        
    }
}