using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using HarmonyLib;
using ObjectBased.RecipeMap;
using RoboPhredDev.PotionCraft.Pantry.PantryPackages;
using UnityEngine;
using Utils.BezierCurves;

namespace RoboPhredDev.PotionCraft.Pantry
{

    [BepInPlugin("net.robophreddev.PotionCraft.Pantry", "Custom Ingredients", "1.0.0.0")]
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
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                TestPackage();
            }
        }

        private void TestPackage()
        {
            Managers.Player.inventory.Clear();

            Debug.Log("Testing yml load");
            try
            {
                var pkg = PantryPackage.Load("Test");

                foreach (var pkgIngredient in pkg.Ingredients)
                {
                    var ingredient = PantryIngredientRegistry.RegisterIngredient(pkgIngredient);
                    Managers.Player.inventory.AddItem(ingredient, 5000, true, true);
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed: {ex.GetType().FullName} {ex.Message} {ex.StackTrace}");
            }
        }

        private void UnlockEverything()
        {
            Managers.RecipeMap.FogSetAll(0);

            Managers.Player.inventory.Clear();
            foreach (var ingredient in Managers.Ingredient.ingredients)
            {
                if (ingredient.name == "Default")
                {
                    continue;
                }
                Managers.Player.inventory.AddItem(ingredient, 5000, true, true);
            }

            foreach (var salt in Salt.allSalts)
            {
                Managers.Player.inventory.AddItem(salt, 5000, true, true);
            }

            var potionBaseSubManager = Managers.RecipeMap.potionBaseSubManager;
            foreach (var potionBase in Managers.RecipeMap.potionBasesSettings.potionBases)
            {
                potionBaseSubManager.UnlockPotionBase(potionBase, false);
            }

            foreach (var state in MapLoader.loadedMaps)
            {
                state.potionEffectsOnMap.ForEach(x => x.Status = PotionEffectStatus.Known);
            }
        }

        private void ApplyPatches()
        {
            var harmony = new Harmony("net.robophreddev.PotionCraft.Pantry");
            harmony.PatchAll();
        }
    }
}