using System;

namespace QMSPOC.ItemBoms
{
    public class ItemBomExcelDto
    {
        public string Code { get; set; } = null!;
        public int Version { get; set; }
        public string? Description { get; set; }
    }
}