namespace QMSPOC.ItemBomDetails
{
    public static class ItemBomDetailConsts
    {
        private const string DefaultSorting = "{0}Qty asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "ItemBomDetail." : string.Empty);
        }

    }
}