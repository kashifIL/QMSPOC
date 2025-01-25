using System;

namespace QMSPOC.ItemCategories;

[Serializable]
public class ItemCategoryDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}