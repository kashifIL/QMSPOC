namespace QMSPOC.ItemMeasuremetnDetails
{
    public static class ItemMeasuremetnDetailConsts
    {
        private const string DefaultSorting = "{0}Type asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "ItemMeasuremetnDetail." : string.Empty);
        }

    }
}