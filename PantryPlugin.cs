using System;
using BepInEx;
using HarmonyLib;
using ObjectBased.RecipeMap;
using UnityEngine;

namespace RoboPhredDev.PotionCraft.Pantry
{

    [BepInPlugin("net.robophreddev.PotionCraft.Pantry", "Custom Ingredients", "0.1.0.0")]
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

            this.ApplyPatches();


            // Delay ingredient load until the ingredients we base ours on have loaded.
            RecipeMapObjectAwakeEvent.OnRecipeMapObjectAwake += (sender, args) => LoadCustomIngredients();
        }

        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.F1))
            // {
            //     UnlockEverything();
            // }
        }

        public void OnGUI()
        {
            ParseErrorsGUI.OnGUI();
        }

        private void LoadCustomIngredients()
        {
            var packages = PantryPackageLoader.LoadAllPackages();
            ParseErrorsGUI.ShowIfErrors();
            Debug.Log($"Loaded {packages.Count} packages");
            foreach (var pkg in packages)
            {
                Debug.Log($"Found {pkg.Ingredients.Count} ingredients in {pkg.Name}");
                foreach (var pkgIngredient in pkg.Ingredients)
                {
                    try
                    {
                        PantryIngredientRegistry.RegisterIngredient(pkgIngredient);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log($"[Pantry]: Failed to load ingredient {pkgIngredient.Name} from package {pkg.Name}: {ex.Message}\n{ex.StackTrace}\n\n");
                    }
                }
            }
        }

        // private void UnlockEverything()
        // {
        //     Managers.RecipeMap.FogSetAll(0);

        //     Managers.Player.inventory.Clear();
        //     foreach (var ingredient in Managers.Ingredient.ingredients)
        //     {
        //         if (ingredient.name == "Default")
        //         {
        //             continue;
        //         }
        //         Debug.Log($"Adding {ingredient.name}");
        //         Managers.Player.inventory.AddItem(ingredient, 5000, true, true);
        //     }

        //     foreach (var salt in Salt.allSalts)
        //     {
        //         Managers.Player.inventory.AddItem(salt, 5000, true, true);
        //     }

        //     var potionBaseSubManager = Managers.RecipeMap.potionBaseSubManager;
        //     foreach (var potionBase in Managers.RecipeMap.potionBasesSettings.potionBases)
        //     {
        //         potionBaseSubManager.UnlockPotionBase(potionBase, false);
        //     }

        //     foreach (var state in MapLoader.loadedMaps)
        //     {
        //         state.potionEffectsOnMap.ForEach(x => x.Status = PotionEffectStatus.Known);
        //     }
        // }

        private void ApplyPatches()
        {
            var harmony = new Harmony("net.robophreddev.PotionCraft.Pantry");
            harmony.PatchAll();
        }
    }
}