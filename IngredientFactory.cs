
using System.Collections.Generic;
using System.Linq;
using ObjectBased.UIElements.Tooltip;
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
            // FIXME: Prefix name with package folder to avoid collisions.
            newIngredient.name = pantryIngredient.Name;

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

            newIngredient.path = new IngredientPath
            {
                path = PathParser.SvgPathToBezierCurves(pantryIngredient.Path),
                grindedPathStartsFrom = pantryIngredient.GrindStartPercent,
            };

            newIngredient.canBeDamaged = ingredientBase.canBeDamaged;
            newIngredient.isTeleportationIngredient = pantryIngredient.IsCrystal;
            newIngredient.soundPreset = ingredientBase.soundPreset;

            // FIXME: Shouldn't unity call this automatically?  Maybe the issue is we are spawning immediately after creating it.  Might need to wait a few game ticks
            newIngredient.OnAwake();

            return newIngredient;
        }
    }
}