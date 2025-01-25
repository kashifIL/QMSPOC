namespace QMSPOC.Items
{
    public static class ItemConsts
    {
        private const string DefaultSorting = "{0}Code asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Item." : string.Empty);
        }

    }
}