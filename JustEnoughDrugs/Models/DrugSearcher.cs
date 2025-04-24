using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Product;

namespace JustEnoughDrugs.Models
{
    public class DrugSearcher
    {
        public enum SearchFilter { Any, Ingredients, Effects, Name }

        public bool ShouldShowDrug(ProductEntry productEntry, string searchText, string filterKey)
        {
            if (string.IsNullOrEmpty(searchText)) return true;

            bool matchesEffect = false;
            bool matchesName = false;
            bool matchesIngredient = false;

            if (filterKey == "Effects" || filterKey == "Any")
            {
                matchesEffect = MatchesEffects(productEntry, searchText);
            }

            if (filterKey == "Ingredients" || filterKey == "Any")
            {
                matchesIngredient = MatchesIngredients(productEntry, searchText);
            }

            if (filterKey == "Name" || filterKey == "Any")
            {
                matchesName = MatchesName(productEntry, searchText);
            }

            return matchesName || matchesEffect || matchesIngredient;
        }

        private bool MatchesName(ProductEntry entry, string searchText)
        {
            string drugName = entry.Definition.ToString();
            return MatchesSearch(drugName, searchText);
        }

        private bool MatchesEffects(ProductEntry entry, string searchText)
        {
            var effectList = new List<string>();
            entry.Definition.Properties.ForEach(p => effectList.Add(p.ToString()));
            return effectList.Any(effect => MatchesSearch(effect, searchText));
        }

        private bool MatchesIngredients(ProductEntry entry, string searchText)
        {
            var ingredientList = new List<string>();
            entry.Definition.Recipes.ForEach(r => r.Ingredients.ForEach(i =>
                ingredientList.Add(i.Item.ToString())));

            return ingredientList.Any(ingredient => MatchesSearch(ingredient, searchText));
        }

        private bool MatchesSearch(string target, string searchText)
        {
            target = target.ToLowerInvariant();
            searchText = searchText.ToLowerInvariant();

            if (searchText.Contains(","))
            {
                var terms = searchText.Split(',');
                return terms.All(term => target.Contains(term.Trim()));
            }
            else if (searchText.Contains("|"))
            {
                var terms = searchText.Split('|');
                return terms.Any(term => target.Contains(term.Trim()));
            }
            else
            {
                return target.Contains(searchText);
            }
        }
    }
}