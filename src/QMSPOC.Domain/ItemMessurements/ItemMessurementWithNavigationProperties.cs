using QMSPOC.Items;

using System;
using System.Collections.Generic;

namespace QMSPOC.ItemMessurements
{
    public  class ItemMessurementWithNavigationProperties
    {
        public ItemMessurement ItemMessurement { get; set; } = null!;

        public Item Item { get; set; } = null!;
        

        
    }
}