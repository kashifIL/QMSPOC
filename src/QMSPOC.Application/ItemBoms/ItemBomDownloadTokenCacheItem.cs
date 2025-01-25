using System;

namespace QMSPOC.ItemBoms;

[Serializable]
public class ItemBomDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}