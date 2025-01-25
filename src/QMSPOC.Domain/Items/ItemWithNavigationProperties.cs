using QMSPOC.ItemCategories;

using System;
using System.Collections.Generic;

namespace QMSPOC.Items
{
    public  class ItemWithNavigationProperties
    {
        public Item Item { get; set; } = null!;

        public ItemCategory ItemCategory { get; set; } = null!;
        

        
    }
}