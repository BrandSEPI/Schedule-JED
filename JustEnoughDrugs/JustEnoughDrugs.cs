using MelonLoader;
using HarmonyLib;
using UnityEngine;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using ScheduleOne.DevUtilities;
using System.Collections.Generic;
using ScheduleOne;
using System.ComponentModel;
using System.Reflection;
using ScheduleOne.Property;
[assembly: MelonInfo(typeof(JustEnoughDrugs.MainMod), JustEnoughDrugs.BuildInfo.Name, JustEnoughDrugs.BuildInfo.Version, JustEnoughDrugs.BuildInfo.Author)]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace JustEnoughDrugs
{
    public static class BuildInfo
    {
        public const string Name = "JustEnoughDrugs";
        public const string Description = "Add a searchBar to the drugs tab";
        public const string Author = "BrandSEPI";
        public const string Company = null;
        public const string Version = "1.3.1";
        public const string DownloadLink = null;
    }

    public class MainMod : MelonMod
    {
        private bool isInitialized = false;
        private bool wasFocusedLastFrame = false;
        private bool isShortcutDisabled = false;
        private InputField inputField = null;
        private Dropdown dropdownFilter = null;
        private Transform drugItems = null;
        private List<UnityEngine.InputSystem.InputAction> disabledActions = new List<UnityEngine.InputSystem.InputAction>();

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            isInitialized = false;
            if (sceneName == "Main")
            {
                MelonCoroutines.Start(InitAfterSceneLoaded());
            }
        }

        private IEnumerator InitAfterSceneLoaded()
        {
            MelonLogger.Msg("Initialising search bar...");
            while (!isInitialized)
            {
                isInitialized = initSearchBar() && initDrugList() && initDropdown();
                yield return null;
            }
        }
        public override void OnUpdate()
        {

            if (inputField == null) return;

            if (inputField.isFocused && !wasFocusedLastFrame)
            {

                MelonLogger.Msg("Input field focussed.");
                if (!isShortcutDisabled)
                {
                    isShortcutDisabled = true;
                    DisableShortcuts();
                }
            }
            else if (!inputField.isFocused && wasFocusedLastFrame)
            {
                MelonLogger.Msg("Input field lost focus.");
                isShortcutDisabled = false;
                RestoreShortcuts();
            }

            wasFocusedLastFrame = inputField.isFocused;
        }


        private Boolean initSearchBar()
        {
            try
            {
                if (isInitialized) return true;
                Transform topbar = null;
                var productManagerApp = GameObject.Find("ProductManagerApp");
                if (productManagerApp != null && productManagerApp.activeInHierarchy)
                {
                    MelonLogger.Msg("ProductManagerApp detected, injecting search bar into Topbar...");
                    topbar = FindChildByPath(productManagerApp.transform, "Container/Topbar");
                    if (topbar != null)
                    {
                        MelonLogger.Msg("Topbar found!");
                        inputField = setupSearchBar(topbar);
                        MelonLogger.Msg("Search bar successfully injected.");
                    }
                }
                return productManagerApp && topbar;
            }

            catch (Exception e)
            {
                MelonLogger.Error($"Error during search bar initialisation: {e}");
                return false;
            }
        }
        private Boolean initDropdown()
        {
            try
            {
                if (isInitialized) return true;
                Transform topbar = null;
                var productManagerApp = GameObject.Find("ProductManagerApp");
                if (productManagerApp != null && productManagerApp.activeInHierarchy)
                {
                    MelonLogger.Msg("ProductManagerApp detected, injecting search bar into Topbar...");
                    topbar = FindChildByPath(productManagerApp.transform, "Container/Topbar");
                    if (topbar != null)
                    {
                        MelonLogger.Msg("Topbar found!");
                        dropdownFilter = SetupDropdown(topbar);

                        MelonLogger.Msg("Dropdown successfully injected.");
                    }
                }
                return productManagerApp && topbar;
            }

            catch (Exception e)
            {
                MelonLogger.Error($"Error during search bar initialisation: {e}");
                return false;
            }
        }



        private Boolean initDrugList()
        {
            var productManagerApp = GameObject.Find("ProductManagerApp");
            if (!productManagerApp) return false;
            var productList = FindChildByPath(productManagerApp.transform, "Container/Scroll View/Viewport/Content");
            drugItems = productList;
            MelonLogger.Msg("DrugsList getted");

            return productList != null;


        }

        private Transform FindChildByPath(Transform parent, string path)
        {
            var current = parent;
            foreach (var part in path.Split('/'))
            {
                current = current.Find(part);
                if (current == null) return null;
            }
            return current;
        }



        private InputField setupSearchBar(Transform parent)
        {
            GameObject inputGO = new GameObject("DrugSearchInput");
            inputGO.transform.SetParent(parent, false);
            inputGO.AddComponent<CanvasRenderer>();

            var rect = inputGO.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(1, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(1, 1);
            rect.sizeDelta = new Vector2(415, 60);

            var bg = inputGO.AddComponent<Image>();
            bg.color = new Color(1f, 1f, 1f, 0.9f);

            var inputField = inputGO.AddComponent<InputField>();

            // Placeholder
            GameObject placeholderGO = new GameObject("Placeholder");
            placeholderGO.transform.SetParent(inputGO.transform, false);
            var placeholder = placeholderGO.AddComponent<Text>();
            placeholder.text = "Search";
            placeholder.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            placeholder.fontSize = 30;
            placeholder.color = Color.gray;
            placeholder.alignment = TextAnchor.MiddleLeft;
            var placeholderRect = placeholder.GetComponent<RectTransform>();
            placeholderRect.anchorMin = Vector2.zero;
            placeholderRect.anchorMax = Vector2.one;
            placeholderRect.offsetMin = new Vector2(10, 5);
            placeholderRect.offsetMax = new Vector2(-10, -5);
            inputField.placeholder = placeholder;

            // Text
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(inputGO.transform, false);
            var text = textGO.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 22;
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleLeft;
            var textRect = text.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(10, 5);
            textRect.offsetMax = new Vector2(-10, -5);
            inputField.textComponent = text;
            inputField.text = "";
            inputField.ForceLabelUpdate();

            // Button
            GameObject clearBtnGO = new GameObject("ClearButton");
            clearBtnGO.transform.SetParent(inputGO.transform, false);
            var buttonText = clearBtnGO.AddComponent<Text>();
            buttonText.text = "X";
            buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonText.fontSize = 30;
            buttonText.color = Color.red;
            buttonText.alignment = TextAnchor.MiddleCenter;
            var buttonRect = clearBtnGO.GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(40, 40);
            buttonRect.anchorMin = new Vector2(1, 1);
            buttonRect.anchorMax = new Vector2(1, 1);
            buttonRect.anchoredPosition = new Vector2(-15, -30);

            var button = clearBtnGO.AddComponent<Button>();
            button.onClick.AddListener(() => ClearSearchText());

            inputField.onValueChanged.AddListener((value) => OnSearchTextChanged(value));
            return inputField;
        }


        private Dropdown SetupDropdown(Transform parent)
        {

            var deliveryApp = GameObject.Find("DeliveryApp");
            if (deliveryApp == null)
            {
                MelonLogger.Error("DeliveryApp not found.");
                throw new Exception("DeliveryApp not found.");
            }
            var originalDropdown = FindChildByPath(deliveryApp.transform, "Container/Scroll View/Viewport/Content/Dan's Hardware/Contents/Panel/Destination/Dropdown (Legacy)").GetComponent<Dropdown>();
            if (originalDropdown == null)
            {
                MelonLogger.Error("Original dropdown not found.");
                throw new Exception("Original dropdown not found.");
            }
            var clonedDropdownGO = GameObject.Instantiate(originalDropdown.gameObject);
            if (clonedDropdownGO == null)
            {
                MelonLogger.Error("Failed to clone dropdown.");
                throw new Exception("Failed to clone dropdown.");
            }
            clonedDropdownGO.name = "SearchDropdown";
            clonedDropdownGO.transform.SetParent(parent, false);
            var clonedDropdown = clonedDropdownGO.GetComponent<Dropdown>();
            var clonedDropdowRect = clonedDropdownGO.GetComponent<RectTransform>();
            clonedDropdowRect.sizeDelta = new Vector2(-1030, 60);
            clonedDropdowRect.anchoredPosition = new Vector2(100, 30);
            clonedDropdown.ClearOptions();
            clonedDropdown.options.Add(new Dropdown.OptionData("Any"));
            clonedDropdown.options.Add(new Dropdown.OptionData("Ingredients"));
            clonedDropdown.options.Add(new Dropdown.OptionData("Effects"));
            clonedDropdown.options.Add(new Dropdown.OptionData("Name"));

            clonedDropdown.onValueChanged.AddListener((index) => OnDropDownValueChanged(index));

            return clonedDropdown;
        }


        private void ClearSearchText()
        {
            MelonLogger.Msg("Clearing search text.");
            inputField.text = "";
        }

        private void OnSearchTextChanged(string value)
        {
            if (drugItems == null) return;
            HideOutline();

            UpdateDrugDisplay(value);

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)drugItems);
        }

        private void OnDropDownValueChanged(int index)
        {
            if (drugItems == null) return;
            HideOutline();

            UpdateDrugDisplay(inputField.text);

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)drugItems);
        }

        private void HideOutline()
        {
            drugItems.Find("Outline").gameObject.SetActive(false);
        }

        private void UpdateDrugDisplay(string value)
        {
            foreach (Transform Category in drugItems)
            {
                bool drugDisplayed = false;
                foreach (Transform Entries in Category)
                {
                    foreach (Transform drugItem in Entries)
                    {
                        ProductEntry productEntry = drugItem.GetComponent<ProductEntry>();

                        if (productEntry != null && productEntry.Definition != null)
                        {

                            bool shouldShow = ShouldShowDrug(productEntry, value, dropdownFilter.options[dropdownFilter.value].text);

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

                var noneObj = Category.Find("None");
                if (noneObj != null)
                {
                    noneObj.gameObject.SetActive(!drugDisplayed);
                }
            }
        }

        private bool ShouldShowDrug(ProductEntry productEntry, string searchText, string filterKey)
        {

            if (string.IsNullOrEmpty(searchText)) return true;

            bool matchesEffect = false;
            bool matchesName = false;
            bool matchesIngredient = false;

            if (filterKey == "Effects" || filterKey == "Any")
            {
                var effectList = new List<string>();
                productEntry.Definition.Properties.ForEach(p => effectList.Add(p.ToString()));
                matchesEffect = effectList.Any(effect => effect.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);

            }
            if (filterKey == "Ingredients" || filterKey == "Any")
            {
                var ingredientList = new List<string>();
                productEntry.Definition.Recipes.ForEach(r => r.Ingredients.ForEach(i => ingredientList.Add(i.Item.ToString())));
                matchesIngredient = ingredientList.Any(ingredient => ingredient.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            if (filterKey == "Name" || filterKey == "Any")
            {
                string drugName = productEntry.Definition.ToString();
                matchesName = drugName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return matchesName || matchesEffect || matchesIngredient;
        }
        private void DisableShortcuts()
        {
            var input = Singleton<GameInput>.Instance;
            var inputAsset = input.PlayerInput;


            foreach (var action in inputAsset.actions)
            {
                string bindingString = string.Join(",", action.bindings.Select(b => b.path));

                if (bindingString.Contains("/keyboard/tab") ||
                    bindingString.Contains("/keyboard/escape") ||
                    bindingString.Contains("/mouse/rightButton"))
                {
                    continue;
                }

                if (action.enabled)
                {
                    action.Disable();
                    disabledActions.Add(action);
                }
            }


        }

        private void RestoreShortcuts()
        {

            {
                foreach (var action in disabledActions)
                {
                    action.Enable();
                }
                disabledActions.Clear();
            }

        }
    }

}
