
using System;
using HarmonyLib;

namespace RoboPhredDev.PotionCraft.Pantry
{

    [HarmonyPatch(typeof(RecipeMapObject), "Awake")]
    static class RecipeMapObjectAwakeEvent
    {
        public static event EventHandler<RecipeMapObjectAwakeEventArgs> OnRecipeMapObjectAwake;

        static void Postfix(RecipeMapObject __instance)
        {
            OnRecipeMapObjectAwake?.Invoke(__instance, new RecipeMapObjectAwakeEventArgs(__instance));
        }
    }

    class RecipeMapObjectAwakeEventArgs : EventArgs
    {
        public RecipeMapObject RecipeMapObject { get; }

        public RecipeMapObjectAwakeEventArgs(RecipeMapObject potionEffectMapItem)
        {
            RecipeMapObject = potionEffectMapItem;
        }
    }
}