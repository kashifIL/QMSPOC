using System;

namespace QMSPOC.Items;

[Serializable]
public class ItemDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}