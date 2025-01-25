namespace QMSPOC.ItemCategories
{
    public static class ItemCategoryConsts
    {
        private const string DefaultSorting = "{0}Code asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "ItemCategory." : string.Empty);
        }

    }
}