
using System;
using HarmonyLib;
using TMPAtlasGenerationSystem;
using UnityEngine;

namespace RoboPhredDev.PotionCraft.Pantry
{
    // WARN: Messing with a namespace called TMP in an early access game is just asking for things to break.
    [HarmonyPatch(typeof(IngredientsAtlasGenerator), "GetAtlasSpriteName")]
    static class IngredientsAtlasGetSpriteNamePatch
    {
        public static EventHandler<IngredientsAtlasGetSpriteNameEventArgs> OnGetSpriteName;

        static bool Prefix(ref string __result, ScriptableObject scriptableObject)
        {
            if (scriptableObject is InventoryItem item)
            {
                var e = new IngredientsAtlasGetSpriteNameEventArgs(item);
                OnGetSpriteName?.Invoke(null, e);
                if (e.Result != null)
                {
                    __result = e.Result;
                    return false;
                }
            }

            return true;
        }
    }

    class IngredientsAtlasGetSpriteNameEventArgs : EventArgs
    {
        public InventoryItem InventoryItem { get; }
        public string Result { get; set; }

        public IngredientsAtlasGetSpriteNameEventArgs(InventoryItem inventoryItem)
        {
            InventoryItem = inventoryItem;
        }
    }
}
