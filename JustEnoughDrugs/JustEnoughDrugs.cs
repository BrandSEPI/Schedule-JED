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
        public const string Version = "1.2.0";
        public const string DownloadLink = null;
    }

    public class MainMod : MelonMod
    {
        private bool isInitialized = false;
        private bool wasFocusedLastFrame = false;
        private bool isShortcutDisabled = false;
        private InputField inputField = null;
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
                isInitialized = initSearchBar() && initDrugList();
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
            rect.sizeDelta = new Vector2(300, 60);

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
            button.onClick.AddListener(ClearSearchText);

            inputField.onValueChanged.AddListener(OnSearchTextChanged);
            return inputField;
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
                            string drugName = productEntry.Definition.ToString();
                            bool shouldShow = drugName.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;

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
           

            MelonLogger.Msg("All actions except allowed keys have been disabled.");
        }

        private void RestoreShortcuts()
        {

            {
                foreach (var action in disabledActions)
                {
                    action.Enable();
                }
                disabledActions.Clear();
                MelonLogger.Msg("All previously disabled inputs restored.");
            }

        }
    }

}
