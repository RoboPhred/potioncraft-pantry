
using System.Collections.Generic;
using System.Linq;
using RoboPhredDev.PotionCraft.Pantry.PantryPackages;
using UnityEngine;
using Utils.BezierCurves;

namespace RoboPhredDev.PotionCraft.Pantry
{
    static class IngredientFactory
    {
        public static Ingredient Create(PantryIngredient pantryIngredient)
        {
            var ingredientBase = Managers.Ingredient.ingredients.FirstOrDefault(x => x.name == pantryIngredient.IngredientBase);
            if (ingredientBase == null)
            {
                throw new System.Exception($"Unknown base ingredient {pantryIngredient.IngredientBase}");
            }

            var newIngredient = ScriptableObject.CreateInstance<Ingredient>();

            // TODO: Load from yaml

            // Clone data from an existing ingredient to satisfy the game.
            newIngredient.inventoryIconObject = ingredientBase.inventoryIconObject;
            newIngredient.itemStackPrefab = ingredientBase.itemStackPrefab;
            newIngredient.grindedSubstance = ingredientBase.grindedSubstance;
            newIngredient.grindedSubstanceColor = ingredientBase.grindedSubstanceColor;
            newIngredient.grindStatusByLeafGrindingCurve = ingredientBase.grindStatusByLeafGrindingCurve;
            newIngredient.grindedSubstanceMaxAmount = ingredientBase.grindedSubstanceMaxAmount;
            newIngredient.physicalParticleType = ingredientBase.physicalParticleType;
            newIngredient.substanceGrindingSettings = ingredientBase.substanceGrindingSettings;
            newIngredient.effectMovement = ingredientBase.effectMovement;
            newIngredient.effectCollision = ingredientBase.effectCollision;
            newIngredient.effectPlantGathering = ingredientBase.effectPlantGathering;
            newIngredient.viscosityDown = ingredientBase.viscosityDown;
            newIngredient.viscosityUp = ingredientBase.viscosityUp;
            newIngredient.isSolid = ingredientBase.isSolid;
            newIngredient.spotPlantPrefab = ingredientBase.spotPlantPrefab;
            // Disable spawning in garden
            newIngredient.spotPlantSpawnTypes = new List<GrowingSpotType>();
            newIngredient.smallIcon = ingredientBase.smallIcon;
            newIngredient.recipeMarkIcon = ingredientBase.recipeMarkIcon;

            // TODO: load path from yaml
            newIngredient.path = new IngredientPath
            {
                path = new List<CubicBezierCurve> {
                        new CubicBezierCurve(new Vector2(0, 0), new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 0)),
                        new CubicBezierCurve(new Vector2(10, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(10, 10)),
                        new CubicBezierCurve(new Vector2(10, 10), new Vector2(10, 20), new Vector2(-10, -20), new Vector2(-10, 10)),
                }
            };

            newIngredient.canBeDamaged = ingredientBase.canBeDamaged;
            newIngredient.isTeleportationIngredient = false; // is crystal?
            newIngredient.soundPreset = ingredientBase.soundPreset;

            newIngredient.OnAwake();

            return newIngredient;
        }
    }
}