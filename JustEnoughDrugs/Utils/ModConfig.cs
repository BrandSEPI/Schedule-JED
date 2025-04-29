using MelonLoader;

namespace JustEnoughDrugs
{
    public static class ModConfig
    {
        private static MelonPreferences_Category category;

        private static MelonPreferences_Entry<bool> EnableDrugSortingEntry;
        private static MelonPreferences_Entry<bool> ShowFullRecipeEntry;

        public static void Setup()
        {
            category = MelonPreferences.CreateCategory("JustEnoughDrugs", "Just Enough Drugs Settings");

            EnableDrugSortingEntry = category.CreateEntry(
                "EnableDrugSorting", true,
                "Enable Drug Sorting",
                "Enables the sorting functionality for drugs in the product manager."
            );

            ShowFullRecipeEntry = category.CreateEntry(
                "ShowFullRecipe", true,
                "Show Full Recipe",
                "Displays the full recursive recipe for products when selected."
            );



            MelonPreferences.Save();

        }

        public static bool EnableDrugSorting => EnableDrugSortingEntry.Value;
        public static bool ShowFullRecipe => ShowFullRecipeEntry.Value;
    }
}
