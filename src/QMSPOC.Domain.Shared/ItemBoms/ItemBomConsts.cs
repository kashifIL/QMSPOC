namespace QMSPOC.ItemBoms
{
    public static class ItemBomConsts
    {
        private const string DefaultSorting = "{0}Code asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "ItemBom." : string.Empty);
        }

    }
}