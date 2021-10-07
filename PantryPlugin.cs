using System;
using System.Linq;
using BepInEx;
using HarmonyLib;
using Npc.Parts;
using Npc.Parts.Settings;
using QuestSystem;
using TMPro;
using UnityEngine;

namespace RoboPhredDev.PotionCraft.Pantry
{

    [BepInPlugin("net.robophreddev.PotionCraft.Pantry", "Custom Ingredients", "0.3.0.0")]
    public class PantryPlugin : BaseUnityPlugin
    {
        public static string AssemblyDirectory
        {
            get
            {
                var assemblyLocation = typeof(PantryPlugin).Assembly.Location;
                return System.IO.Path.GetDirectoryName(assemblyLocation);
            }
        }

        void Awake()
        {
            UnityEngine.Debug.Log($"[Pantry]: Loaded");

            PantryIngredientRegistry.Initialize();
            InjectAtlases();

            this.ApplyPatches();

            // Delay ingredient load until the ingredients we base ours on have loaded.
            RecipeMapObjectAwakeEvent.OnRecipeMapObjectAwake += (sender, args) =>
            {
                LoadCustomIngredients();
            };
        }

        void InjectAtlases()
        {
            var old = Reflection.GetPrivateStaticField<TMP_Text, Func<int, string, TMP_SpriteAsset>>("OnSpriteAssetRequest");
            var assetHashCode = TMP_TextUtilities.GetSimpleHashCode(PantryIngredientAtlas.AtlasName);
            TextMeshPro.OnSpriteAssetRequest += (hashCode, assetName) =>
            {
                if (hashCode == assetHashCode)
                {
                    return PantryIngredientAtlas.Atlas;
                }

                if (old != null)
                {
                    return old(hashCode, assetName);
                }

                return null;
            };
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                AddCustomIngredients();
            }

            if (Input.GetKeyDown(KeyCode.F11))
            {
                LoadCustomIngredients();
            }
        }

        public void OnGUI()
        {
            ParseErrorsGUI.OnGUI();
        }

        private void DebugTest()
        {
            foreach (var template in NpcTemplate.allNpcTemplates)
            {
                Debug.Log($"NPC Template: {template.name}");

                NpcTemplate.UsedSubTemplatesOnGetParts.Clear();
                var parts = template.GetListOfPartsToApply();

                foreach (var traderSettings in parts.Item1.OfType<TraderSettings>())
                {
                    DumpTraderSettings(traderSettings);
                }

                foreach (var quest in parts.Item1.OfType<Quest>())
                {
                    DumpQuest(quest);
                }
            }
        }

        private void DumpTraderSettings(TraderSettings traderSettings)
        {
            Debug.Log($" > Trader Settings: {traderSettings.name}");
            foreach (var category in traderSettings.deliveriesCategories)
            {
                Debug.Log($" > > Category: {category.name}");
                foreach (var delivery in category.deliveries)
                {
                    Debug.Log($" > > > Delivery: {delivery.name} item {delivery.item.name} price {delivery.item.GetPrice()} chance {delivery.appearingChance} count {delivery.minCount} to {delivery.maxCount}");
                }
            }
        }

        private void DumpQuest(Quest quest)
        {
            Debug.Log($" > Quest: {quest.name}");
            foreach (var effect in quest.desiredEffects)
            {
                Debug.Log($"> > Desired Effect: {effect.name} Base price {effect.price}");
            }
        }

        private void LoadCustomIngredients()
        {
            var packages = PantryPackageLoader.LoadAllPackages();
            packages.ForEach(x => x.Apply());
            PantryIngredientAtlas.RebuildAtlas();
            ParseErrorsGUI.ShowIfErrors();
        }

        private void AddCustomIngredients()
        {
            foreach (var ingredient in PantryIngredientRegistry.ResolvedIngredients)
            {
                Managers.Player.inventory.AddItem(ingredient, 10);
            }
        }

        private void ApplyPatches()
        {
            var harmony = new Harmony("net.robophreddev.PotionCraft.Pantry");
            harmony.PatchAll();
        }
    }
}