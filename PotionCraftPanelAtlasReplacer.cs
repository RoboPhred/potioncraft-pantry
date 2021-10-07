
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace RoboPhredDev.PotionCraft.Pantry
{
    [HarmonyPatch(typeof(PotionCraftPanel.PotionCraftPanel), "UpdateIngredientsList")]
    static class PotionCraftPanelAtlasReplacer
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var found = false;
            foreach (var instruction in instructions)
            {
                // TODO: We shouldn't trust that this code always uses loc.0 for this value...
                // Probably should look for ldstr "<voffset=0.1em><size=270%><sprite=\"", then skip over its stelem.ref, the array dup, and lcd.i4.3 which prepares the next array index.
                if (!found && instruction.opcode == OpCodes.Ldloc_0)
                {
                    found = true;
                    yield return new CodeInstruction(OpCodes.Ldloc_3); // index
                    yield return new CodeInstruction(OpCodes.Call, typeof(PotionCraftPanelAtlasReplacer).GetMethod("GetAtlasForUsedComponentIndex", BindingFlags.Static | BindingFlags.NonPublic));

                }
                else
                {
                    yield return instruction;
                }
            }

            if (found)
            {
                Debug.Log("[PotionCraft] Injected atlas replacement for PotionCraftPanel.");
            }
            else
            {
                Debug.Log("[PotionCraft] Failed to inject atlas replacement for PotionCraftPanel!");
            }
        }

        private static string GetAtlasForUsedComponentIndex(int usedComponentIndex)
        {
            var component = Managers.Potion.usedComponents[usedComponentIndex];
            if (component.componentObject is not Ingredient ingredient)
            {
                return Managers.TmpAtlas.settings.IngredientsAtlasName;
            }

            if (PantryIngredientRegistry.IsPantryIngredient(ingredient))
            {
                return PantryIngredientAtlas.AtlasName;
            }

            return Managers.TmpAtlas.settings.IngredientsAtlasName;
        }
    }
}