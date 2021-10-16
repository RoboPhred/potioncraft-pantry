
using System;
using HarmonyLib;
using ObjectBased.UIElements.Tooltip;

namespace RoboPhredDev.PotionCraft.Pantry
{
    [HarmonyPatch(typeof(Ingredient), "GetTooltipContent")]
    static class IngredientGetTooltipContentPatch
    {
        public static EventHandler<IngredientGetTooltipContentEventArgs> OnIngredientGetTooltipContent;

        static bool Prefix(Ingredient __instance, ref TooltipContent __result)
        {
            var e = new IngredientGetTooltipContentEventArgs(__instance);
            OnIngredientGetTooltipContent?.Invoke(__instance, e);
            if (e.Result != null)
            {
                __result = e.Result;
                return false;
            }

            return true;
        }
    }

    class IngredientGetTooltipContentEventArgs : EventArgs
    {
        public Ingredient Ingredient { get; }
        public TooltipContent Result { get; set; }

        public IngredientGetTooltipContentEventArgs(Ingredient ingredient)
        {
            Ingredient = ingredient;
        }
    }
}
