
using Books.RecipeBook;
using HarmonyLib;
using TMPro;

namespace RoboPhredDev.PotionCraft.Pantry
{
    [HarmonyPatch(typeof(RecipeBookLeftPageContent), "Awake")]
    static class RecipeBookSpriteAtlasInjector
    {
        private static void Postfix(RecipeBookLeftPageContent __instance)
        {
            var ingredientsText = Reflection.GetPrivateField<TextMeshPro>(__instance, "ingredientsText");
        }
    }
}