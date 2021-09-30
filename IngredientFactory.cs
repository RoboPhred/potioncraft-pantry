
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
            // FIXME: Prefix name with package folder to avoid collisions.
            newIngredient.name = pantryIngredient.QualifiedName;

            // Clone data from an existing ingredient to satisfy the game.
            if (!string.IsNullOrEmpty(pantryIngredient.InventoryImage))
            {
                newIngredient.inventoryIconObject = SpriteLoader.LoadSpriteFromFile(System.IO.Path.Combine(pantryIngredient.Package.DirectoryPath, pantryIngredient.InventoryImage));

                // TESTING
                newIngredient.recipeMarkIcon = newIngredient.inventoryIconObject;
            }
            else
            {
                newIngredient.inventoryIconObject = ingredientBase.inventoryIconObject;
                newIngredient.recipeMarkIcon = ingredientBase.recipeMarkIcon;
            }

            if (!string.IsNullOrEmpty(pantryIngredient.RecipeImage))
            {
                newIngredient.recipeMarkIcon = SpriteLoader.LoadSpriteFromFile(System.IO.Path.Combine(pantryIngredient.Package.DirectoryPath, pantryIngredient.RecipeImage));
            }
            else
            {
                newIngredient.recipeMarkIcon = ingredientBase.recipeMarkIcon;
            }

            // TODO: This seems to be for the small ingredients marker in the ingredients list of recipes,
            // but its not working.  These seem to be built up by the InventoryAtlas, maybe at runtime, so maybe we need to regenerate that?
            newIngredient.smallIcon = newIngredient.inventoryIconObject;

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

            newIngredient.path = new IngredientPath
            {
                path = pantryIngredient.Path,
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