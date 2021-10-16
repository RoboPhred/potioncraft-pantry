using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace RoboPhredDev.PotionCraft.Pantry
{

    [BepInPlugin("net.robophreddev.PotionCraft.Pantry", "Custom Ingredients", "0.4.0.0")]
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
            Debug.Log($"[Pantry]: Loaded");

            PantryIngredientRegistry.Initialize();
            PantryIngredientAtlas.Initialize();

            ApplyPatches();

            // Delay ingredient load until the ingredients we base ours on have loaded.
            RecipeMapObjectAwakeEvent.OnRecipeMapObjectAwake += (sender, args) => LoadCustomIngredients();
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

        private void LoadCustomIngredients()
        {
            var packages = PantryPackageLoader.LoadAllPackages();
            packages.ForEach(x => x.Apply());
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