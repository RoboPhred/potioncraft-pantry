
using System.Collections.Generic;
using System.Linq;
using ObjectBased.UIElements.Tooltip;
using RoboPhredDev.PotionCraft.Pantry.PantryPackages;

namespace RoboPhredDev.PotionCraft.Pantry
{

    static class PantryIngredientRegistry
    {
        private static readonly Dictionary<Ingredient, PantryIngredient> ingredientLookup = new();

        public static IEnumerable<Ingredient> RegisteredIngredients { get => ingredientLookup.Keys; }

        public static void Initialize()
        {
            IngredientGetTooltipContentPatch.OnIngredientGetTooltipContent += (_, e) =>
            {
                if (!ingredientLookup.TryGetValue(e.Ingredient, out var pantryIngredient))
                {
                    return;
                }

                e.Result = new TooltipContent
                {
                    header = pantryIngredient.Name,
                    path = pantryIngredient.QualifiedName,
                    description1 = pantryIngredient.Description,
                };
            };

            IngredientsAtlasGetSpriteNamePatch.OnGetSpriteName += (_, e) =>
            {
                if (e.InventoryItem is Ingredient ingredient)
                {
                    if (!ingredientLookup.TryGetValue(ingredient, out var pantryIngredient))
                    {
                        return;
                    }

                    e.Result = pantryIngredient.IngredientBase + " SmallIcon";
                }
            };
        }

        public static Ingredient RegisterIngredient(PantryIngredient pantryIngredient)
        {
            var ingredient = IngredientFactory.Create(pantryIngredient);
            Managers.Ingredient.ingredients.Add(ingredient);
            ingredientLookup.Add(ingredient, pantryIngredient);
            return ingredient;
        }

        public static Ingredient GetIngredientByName(string name)
        {
            return ingredientLookup.Keys.FirstOrDefault(x => x.name == name);
        }
    }
}