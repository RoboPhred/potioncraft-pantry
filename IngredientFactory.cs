
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
            var newIngredient = ScriptableObject.CreateInstance<Ingredient>();
            Apply(pantryIngredient, newIngredient);
            return newIngredient;
        }

        public static void Apply(PantryIngredient pantryIngredient, Ingredient ingredient)
        {
            var ingredientBase = Managers.Ingredient.ingredients.FirstOrDefault(x => x.name == pantryIngredient.IngredientBase);
            if (ingredientBase == null)
            {
                throw new System.Exception($"Unknown base ingredient {pantryIngredient.IngredientBase}");
            }

            ingredient.name = pantryIngredient.QualifiedName;

            // Clone data from an existing ingredient to satisfy the game.
            if (!string.IsNullOrEmpty(pantryIngredient.InventoryImage))
            {
                ingredient.inventoryIconObject = SpriteLoader.LoadSpriteFromFile(System.IO.Path.Combine(pantryIngredient.Package.DirectoryPath, pantryIngredient.InventoryImage));
            }
            else
            {
                ingredient.inventoryIconObject = ingredientBase.inventoryIconObject;
            }

            if (!string.IsNullOrEmpty(pantryIngredient.RecipeImage))
            {
                ingredient.recipeMarkIcon = SpriteLoader.LoadSpriteFromFile(System.IO.Path.Combine(pantryIngredient.Package.DirectoryPath, pantryIngredient.RecipeImage));
            }
            else
            {
                ingredient.recipeMarkIcon = ingredientBase.recipeMarkIcon;
            }

            // TODO: This seems to be for the small ingredients marker in the ingredients list of recipes,
            // but its not working.  These seem to be built up by the InventoryAtlas, maybe at runtime, so maybe we need to regenerate that?
            ingredient.smallIcon = ingredient.inventoryIconObject;

            ingredient.itemStackPrefab = ingredientBase.itemStackPrefab;
            ingredient.grindedSubstance = ingredientBase.grindedSubstance;
            ingredient.grindedSubstanceColor = ingredientBase.grindedSubstanceColor;

            ingredient.grindStatusByLeafGrindingCurve = ingredientBase.grindStatusByLeafGrindingCurve;
            ingredient.grindedSubstanceMaxAmount = ingredientBase.grindedSubstanceMaxAmount;
            ingredient.physicalParticleType = ingredientBase.physicalParticleType;
            ingredient.substanceGrindingSettings = ingredientBase.substanceGrindingSettings;
            ingredient.effectMovement = ingredientBase.effectMovement;
            ingredient.effectCollision = ingredientBase.effectCollision;
            ingredient.effectPlantGathering = ingredientBase.effectPlantGathering;
            ingredient.viscosityDown = ingredientBase.viscosityDown;
            ingredient.viscosityUp = ingredientBase.viscosityUp;
            ingredient.isSolid = ingredientBase.isSolid;
            ingredient.spotPlantPrefab = ingredientBase.spotPlantPrefab;
            // Disable spawning in garden
            ingredient.spotPlantSpawnTypes = new List<GrowingSpotType>();

            ingredient.path = new IngredientPath
            {
                path = pantryIngredient.Path,
                grindedPathStartsFrom = pantryIngredient.GrindStartPercent,
            };

            ingredient.canBeDamaged = ingredientBase.canBeDamaged;
            ingredient.isTeleportationIngredient = pantryIngredient.IsCrystal;
            ingredient.soundPreset = ingredientBase.soundPreset;

            // FIXME: Shouldn't unity call this automatically?  Maybe the issue is we are spawning immediately after creating it.  Might need to wait a few game ticks
            ingredient.OnAwake();
        }
    }
}