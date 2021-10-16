
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Books.RecipeBook;
using HarmonyLib;
using UnityEngine;

namespace RoboPhredDev.PotionCraft.Pantry
{
    [HarmonyPatch(typeof(RecipeBookLeftPageContent), "UpdateIngredientsList")]
    static class RecipeBookLeftPageAtlasReplacer
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var found = false;
            foreach (var instruction in instructions)
            {
                if (!found && instruction.opcode == OpCodes.Ldloc_S && instruction.operand is LocalBuilder localBuilder && localBuilder.LocalIndex == 5 && localBuilder.LocalType == typeof(Potion.UsedComponent))
                {
                    found = true;

                    // We should now be right before the if statement checking if the current potion is in stock

                    yield return new CodeInstruction(OpCodes.Ldloc_S, 5); // currentPotion
                    yield return new CodeInstruction(OpCodes.Call, typeof(RecipeBookLeftPageAtlasReplacer).GetMethod("GetAtlasForUsedComponentIndex", BindingFlags.Static | BindingFlags.NonPublic));
                    // Store the result into ingredientsAtlasName so it will be used in one of the two branching string constructions.
                    yield return new CodeInstruction(OpCodes.Stloc_0);

                    // Return the first part of the if-check and continue as normal.
                    yield return instruction;

                }
                else
                {
                    yield return instruction;
                }
            }

            if (found)
            {
                Debug.Log("[Pantry] Injected atlas replacement for RecipeBookLeftPageContent.");
            }
            else
            {
                Debug.Log("[Pantry] Failed to inject atlas replacement for RecipeBookLeftPageContent!");
            }
        }

        private static string GetAtlasForUsedComponentIndex(Potion.UsedComponent component)
        {
            Debug.Log($"Getting component for " + component.componentObject.name);

            if (component.componentObject is not Ingredient ingredient)
            {
                Debug.Log("> Is not ingredient");
                return Managers.TmpAtlas.settings.IngredientsAtlasName;
            }

            if (PantryIngredientRegistry.IsPantryIngredient(ingredient))
            {
                Debug.Log("> Is pantry");
                return PantryIngredientAtlas.AtlasName;
            }

            Debug.Log("> Is not pantry");

            return Managers.TmpAtlas.settings.IngredientsAtlasName;
        }
    }
}