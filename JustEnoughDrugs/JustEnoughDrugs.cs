using MelonLoader;
using HarmonyLib;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.Product;
using ScheduleOne.Property;
using JustEnoughDrugs.UI;
using JustEnoughDrugs.Models;
[assembly: MelonInfo(typeof(JustEnoughDrugs.MainMod), JustEnoughDrugs.BuildInfo.Name, JustEnoughDrugs.BuildInfo.Version, JustEnoughDrugs.BuildInfo.Author)]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace JustEnoughDrugs
{
    public static class BuildInfo
    {
        public const string Name = "JustEnoughDrugs";
        public const string Description = "Filter drugs by name, ingredients or effects and sort them !";
        public const string Author = "BrandSEPI";
        public const string Company = null;
        public const string Version = "2.0.0";
        public const string DownloadLink = null;
    }

    public class MainMod : MelonMod
    {
        private bool isInitialized = false;
        private static UIManager uiManager;

        // Keep these public static dictionaries for now
        public static Dictionary<ProductDefinition, float> ProductCosts = new();
        public static Dictionary<ProductDefinition, List<PropertyItemDefinition>> ExtendedRecipes = new();

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
            MelonLogger.Msg("Initializing JustEnoughDrugs...");

            uiManager = new UIManager();

            while (!isInitialized)
            {
                isInitialized = uiManager.Initialize();
                yield return null;
            }
        }

        public override void OnUpdate()
        {
            if (isInitialized)
            {
                uiManager.Update();
            }
        }

        // Harmony patches
        [HarmonyPatch(typeof(ProductDefinition), nameof(ProductDefinition.AddRecipe))]
        public class ProductDefinition_AddRecipe_Patch
        {
            static void Postfix(ProductDefinition __instance)
            {
                RecipeManager.ProcessNewRecipe(__instance);
            }
        }

        [HarmonyPatch(typeof(ProductEntry), "Clicked")]
        public class ProductEntry_Clicked_Patch
        {
            static void Postfix(ProductEntry __instance)
            {
                uiManager.DetailClicked(__instance);
            }
        }
    }
}