namespace QMSPOC.ItemMessurements
{
    public static class ItemMessurementConsts
    {
        private const string DefaultSorting = "{0}Code asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "ItemMessurement." : string.Empty);
        }

    }
}