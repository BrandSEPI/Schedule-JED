using UnityEngine;
using UnityEngine.UI;
using MelonLoader;
using System;
using ScheduleOne.Product;
using System.Collections.Generic;
using JustEnoughDrugs.Utils;
using JustEnoughDrugs.Models;

namespace JustEnoughDrugs.UI
{
    public class DrugListUI
    {
        private Transform drugItems;
        private DrugSearcher searcher;

        private DrugSorter sorter;

        public bool Initialize(Transform productManagerAppTransform)
        {
            try
            {
                drugItems = GetDrugListTransform(productManagerAppTransform);
                searcher = new DrugSearcher();
                sorter = new DrugSorter();
                if (drugItems != null)
                {
                    MelonLogger.Msg("Drug list found and initialized.");
                    return true;
                }

                MelonLogger.Error("Failed to find drug list transform.");
                return false;
            }
            catch (Exception e)
            {
                MelonLogger.Error($"Error initializing drug list: {e}");
                return false;
            }
        }

        private Transform GetDrugListTransform(Transform productManagerAppTransform)
        {
            if (productManagerAppTransform == null)
            {
                MelonLogger.Error("Product Manager App transform is null.");
                return null;
            }

            var productList = TransformExtensions.FindChildByPath(
                productManagerAppTransform,
                "Container/Scroll View/Viewport/Content");

            if (productList == null)
            {
                MelonLogger.Error("Could not find drug list in hierarchy.");
                return null;
            }

            MelonLogger.Msg("Drug list component accessed successfully.");
            return productList;
        }

        public void UpdateDrugDisplay(string searchText, string filterKey)
        {
            if (drugItems == null)
            {
                MelonLogger.Error("Cannot update drug display: drug items is null.");
                return;
            }

            HideOutline();

            foreach (Transform category in drugItems)
            {
                bool drugDisplayed = false;
                foreach (Transform entries in category)
                {
                    foreach (Transform drugItem in entries)
                    {
                        ProductEntry productEntry = drugItem.GetComponent<ProductEntry>();

                        if (productEntry != null && productEntry.Definition != null)
                        {
                            bool shouldShow = searcher.ShouldShowDrug(productEntry, searchText, filterKey);

                            if (drugItem.gameObject.activeSelf != shouldShow)
                            {
                                drugItem.gameObject.SetActive(shouldShow);
                            }

                            if (!drugDisplayed)
                            {
                                drugDisplayed = shouldShow;
                            }
                        }
                        else
                        {
                            drugItem.gameObject.SetActive(false);
                        }
                    }
                }

                var noneObj = category.Find("None");
                if (noneObj != null)
                {
                    noneObj.gameObject.SetActive(!drugDisplayed);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)drugItems);
        }

        public void ReorderDrugs(string sorterType, string sortOrder)
        {
            if (drugItems == null || sorterType == "None")
                return;

            MelonLogger.Msg($"Reordering drugs by {sorterType} in {sortOrder} order");

            DrugSorter.SorterType type = (DrugSorter.SorterType)Enum.Parse(typeof(DrugSorter.SorterType), sorterType);
            DrugSorter.SortOrder order = (DrugSorter.SortOrder)Enum.Parse(typeof(DrugSorter.SortOrder), sortOrder);

            foreach (Transform category in drugItems)
            {
                foreach (Transform entries in category)
                {
                    var drugEntries = new List<ProductEntry>();
                    var drugTransforms = new Dictionary<ProductEntry, Transform>();

                    foreach (Transform drugItem in entries)
                    {
                        if (drugItem.gameObject.activeSelf)
                        {
                            var productEntry = drugItem.GetComponent<ProductEntry>();
                            if (productEntry != null && productEntry.Definition != null)
                            {
                                drugEntries.Add(productEntry);
                                drugTransforms.Add(productEntry, drugItem);
                            }
                        }
                    }

                    var sortedEntries = DrugSorter.SortDrugs(drugEntries, type, order);

                    for (int i = 0; i < sortedEntries.Count; i++)
                    {
                        Transform drugItem = drugTransforms[sortedEntries[i]];
                        drugItem.SetSiblingIndex(i);
                    }
                }
            }

            // Force rebuild layout
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)drugItems);
        }
        private void HideOutline()
        {
            var outline = drugItems.Find("Outline");
            if (outline != null)
            {
                outline.gameObject.SetActive(false);
            }
        }
    }
}