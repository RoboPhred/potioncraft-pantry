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
                    var ingredient = IngredientFactory.Create(pkgIngredient);
                    Managers.Ingredient.ingredients.Add(ingredient);
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

        private void CreateCustomIngredient()
        {
            Managers.Player.inventory.Clear();
            var first = Managers.Ingredient.ingredients.FindLast(x => x.name != "Default");
            // var pathGroups = Managers.Ingredient.ingredients.Where(x => x.name != "Default").Select(x => x.path.path).ToList();
            // var lastEnd = Vector2.zero;
            // pathGroups = pathGroups.Select(group =>
            // {
            //     group = group.Select(x => x.ShiftBy(lastEnd)).ToList();
            //     lastEnd = group.Last().PLast;
            //     return group;
            // }).ToList();

            // first.path.path = pathGroups.SelectMany(x => x).ToList();

            // first.path.path = new List<CubicBezierCurve> {
            //     new CubicBezierCurve(new Vector2(0, 0), new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 0)),
            //     new CubicBezierCurve(new Vector2(10, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(10, 10)),
            //     new CubicBezierCurve(new Vector2(10, 10), new Vector2(10, 20), new Vector2(-10, -20), new Vector2(-10, 10)),
            // };

            var scale = .2f;
            first.path.path = new List<CubicBezierCurve> {
                    // C-3.45197955,2.99187,-5.37878825,6.23661,-12.09523825,6.75631
                    new CubicBezierCurve(
                        new Vector2(0, 0),
                        new Vector2(-3.45197955f * scale,2.99187f * scale),
                        new Vector2(-5.37878825f * scale,6.23661f * scale),
                        new Vector2(-12.09523825f * scale,6.75631f * scale)
                    ),
                    // C-17.46697625,7.02271,-19.200683249999997,2.4401900000000003,-12.898437249999999,1.4646499999999998
                    new CubicBezierCurve(
                        new Vector2(-12.09523825f * scale,6.75631f * scale),
                        new Vector2(-17.46697625f * scale,7.02271f * scale),
                        new Vector2(-19.200683249999997f * scale,2.4401900000000003f * scale),
                        new Vector2(-12.898437249999999f * scale,1.4646499999999998f * scale)
                    ),
                    // C-7.354424549999999,1.1581699999999997,-2.001491849999999,7.93487,4.960937550000002,8.268229999999999
                    new CubicBezierCurve(
                        new Vector2(-12.898437249999999f * scale,1.4646499999999998f * scale),
                        new Vector2(-7.354424549999999f * scale,1.1581699999999997f * scale),
                        new Vector2(-2.001491849999999f * scale,7.93487f * scale),
                        new Vector2(4.960937550000002f * scale,8.268229999999999f * scale)
                    ),
                    // C11.497886750000003,8.71322,12.311960750000003,3.0834899999999994,4.2994791500000025,3.2127799999999986
                    new CubicBezierCurve(
                        new Vector2(4.960937550000002f * scale,8.268229999999999f * scale),
                        new Vector2(11.497886750000003f * scale,8.71322f * scale),
                        new Vector2(12.311960750000003f * scale,3.0834899999999994f * scale),
                        new Vector2(4.2994791500000025f*scale,3.2127799999999986f*scale)
                    ),
                    // C-0.3647214799999974,3.8753399999999987,-9.534819049999996,12.399099999999999,-14.032365249999998,12.803939999999999
                    new CubicBezierCurve(
                        new Vector2(4.2994791500000025f* scale,3.2127799999999986f* scale),
                        new Vector2(-0.3647214799999974f* scale,3.8753399999999987f* scale),
                        new Vector2(-9.534819049999996f* scale,12.399099999999999f* scale),
                        new Vector2(-14.032365249999998f* scale,12.803939999999999f* scale)
                    ),
                    // C-22.12458325,12.59005,-18.86688825,5.559219999999999,-9.685639749999998,9.354899999999999
                    new CubicBezierCurve(
                        new Vector2(-14.032365249999998f* scale,12.803939999999999f* scale),
                        new Vector2(-22.12458325f* scale,12.59005f* scale),
                        new Vector2(-18.86688825f* scale,5.559219999999999f* scale),
                        new Vector2(-9.685639749999998f* scale,9.354899999999999f* scale)
                    ),
                    // C-7.149573649999998,11.470769999999998,-4.620622349999998,12.746789999999999,0.3779762100000017,14.07959
                    new CubicBezierCurve(
                        new Vector2(-9.685639749999998f* scale,9.354899999999999f* scale),
                        new Vector2(-7.149573649999998f* scale,11.470769999999998f* scale),
                        new Vector2(-4.620622349999998f* scale,12.746789999999999f* scale),
                        new Vector2(0.3779762100000017f* scale,14.07959f* scale)
                    ),
                    // C4.107821150000001,14.82418,7.502024250000002,11.00845,1.1339285500000016,10.11087
                    new CubicBezierCurve(
                        new Vector2(0.3779762100000017f* scale,14.07959f* scale),
                        new Vector2(4.107821150000001f* scale,14.82418f* scale),
                        new Vector2(7.502024250000002f* scale,11.00845f* scale),
                        new Vector2(1.1339285500000016f* scale,10.11087f* scale)
                    ),
                    // C-4.026524049999999,8.78736,-17.57906825,15.23747,-0.37797617999999833,18.28459
                    new CubicBezierCurve(
                        new Vector2(1.1339285500000016f* scale,10.11087f* scale),
                        new Vector2(-4.026524049999999f* scale,8.78736f* scale),
                        new Vector2(-17.57906825f* scale,15.23747f* scale),
                        new Vector2(-0.37797617999999833f* scale,18.28459f* scale)
                    ),
                };
            Managers.Player.inventory.AddItem(first, 5000, true, true);
        }

        private void ApplyPatches()
        {
            var harmony = new Harmony("net.robophreddev.PotionCraft.Pantry");
            harmony.PatchAll();
        }
    }
}