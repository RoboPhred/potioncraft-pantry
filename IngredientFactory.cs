
using System.Collections.Generic;
using System.Linq;
using RoboPhredDev.PotionCraft.Pantry.PantryPackages;
using UnityEngine;

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

            // TODO: Create our own name, and override the strings in GetTooltipContent
            newIngredient.name = ingredientBase.name;

            // TODO: Load from yaml

            // Clone data from an existing ingredient to satisfy the game.
            newIngredient.inventoryIconObject = ingredientBase.inventoryIconObject;
            newIngredient.itemStackPrefab = ingredientBase.itemStackPrefab;
            newIngredient.grindedSubstance = ingredientBase.grindedSubstance;
            newIngredient.grindedSubstanceColor = ingredientBase.grindedSubstanceColor;

            // TODO: Not working?  Ingredient always at max length, doesnt allow grinding.
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

            // newIngredient.path = new IngredientPath
            // {
            //     path = new List<CubicBezierCurve> {
            //             new CubicBezierCurve(new Vector2(0, 0), new Vector2(0, 0), new Vector2(10, 0), new Vector2(10, 0)),
            //             new CubicBezierCurve(new Vector2(10, 0), new Vector2(10, 0), new Vector2(10, 10), new Vector2(10, 10)),
            //             new CubicBezierCurve(new Vector2(10, 10), new Vector2(10, 20), new Vector2(-10, -20), new Vector2(-10, 10)),
            //     }
            // };
            newIngredient.path = new IngredientPath
            {
                path = PathParser.SvgPathToBezierCurves(pantryIngredient.Path)
            };

            newIngredient.canBeDamaged = ingredientBase.canBeDamaged;
            newIngredient.isTeleportationIngredient = pantryIngredient.IsCrystal;
            newIngredient.soundPreset = ingredientBase.soundPreset;

            newIngredient.OnAwake();

            return newIngredient;
        }
    }
}