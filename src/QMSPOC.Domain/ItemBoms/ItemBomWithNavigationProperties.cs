using QMSPOC.Items;

using System;
using System.Collections.Generic;

namespace QMSPOC.ItemBoms
{
    public  class ItemBomWithNavigationProperties
    {
        public ItemBom ItemBom { get; set; } = null!;

        public Item Item { get; set; } = null!;
        

        
    }
}