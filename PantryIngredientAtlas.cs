using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RoboPhredDev.PotionCraft.Pantry
{
    static class PantryIngredientAtlas
    {
        public const string AtlasName = "PantryIngredientAtlas";

        private static readonly Dictionary<string, Texture2D> atlasContent = new();
        private static TMP_SpriteAsset spriteAsset;

        private static TMP_SpriteAsset Atlas
        {
            get
            {
                if (spriteAsset == null)
                {
                    RebuildAtlas();
                }
                return spriteAsset;
            }
        }

        public static void Initialize()
        {
            var old = Reflection.GetPrivateStaticField<TMP_Text, Func<int, string, TMP_SpriteAsset>>("OnSpriteAssetRequest");
            var assetHashCode = TMP_TextUtilities.GetSimpleHashCode(AtlasName);
            TMP_Text.OnSpriteAssetRequest += (hashCode, assetName) =>
            {
                if (hashCode == assetHashCode)
                {
                    return Atlas;
                }

                if (old != null)
                {
                    return old(hashCode, assetName);
                }

                return null;
            };
        }

        public static void AddOrUpdateSprite(string spriteName, Texture2D texture)
        {
            atlasContent[spriteName] = texture;
            spriteAsset = null;
        }

        public static void RebuildAtlas()
        {
            var asset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
            asset.spriteInfoList = new List<TMP_Sprite>();

            var texture = new Texture2D(0, 0, TextureFormat.ARGB32, false, false);

            var ingredientTextures = (from ingredient in PantryIngredientRegistry.PantryIngredients
                                      let iconTex = TextureLoader.LoadTexture(System.IO.Path.Combine(ingredient.Package.DirectoryPath, ingredient.RecipeImage))
                                      select new { Name = ingredient.QualifiedName, Texture = iconTex }).ToArray();

            var rects = texture.PackTextures(ingredientTextures.Select(x => x.Texture).ToArray(), 0, 4096, false);

            asset.spriteSheet = texture;
            ShaderUtilities.GetShaderPropertyIDs();
            var material = new Material(Shader.Find("TextMeshPro/Sprite"));
            material.SetTexture(ShaderUtilities.ID_MainTex, texture);
            material.hideFlags = HideFlags.HideInHierarchy;
            asset.material = material;

            var scaleW = (float)texture.width;
            var scaleH = (float)texture.height;

            for (var i = 0; i < ingredientTextures.Length; i++)
            {
                var rect = rects[i];
                var data = ingredientTextures[i];

                var pixelRect = new Rect(rect.x * scaleW, rect.y * scaleH, rect.width * scaleW, rect.height * scaleH);
                var spriteName = $"{data.Name} SmallIcon";
                var sprite = new TMP_Sprite
                {
                    id = i,
                    name = spriteName,
                    hashCode = TMP_TextUtilities.GetSimpleHashCode(spriteName),
                    x = pixelRect.x,
                    y = pixelRect.y,
                    width = pixelRect.width,
                    height = pixelRect.height,
                    xAdvance = pixelRect.width,
                    xOffset = 0,
                    yOffset = pixelRect.height,
                    scale = 1.5f,
                };

                asset.spriteInfoList.Add(sprite);
            }

            asset.UpdateLookupTables();

            spriteAsset = asset;

            ResetAssets();
        }

        private static void ResetAssets()
        {
            Debug.Log("Getting instance");
            var instance = Reflection.GetPrivateStaticField<MaterialReferenceManager, MaterialReferenceManager>("s_Instance");
            if (instance == null)
            {
                Debug.Log("> No instance");
                return;
            }
            Debug.Log("Clearing font material");
            Reflection.GetPrivateField<Dictionary<int, Material>>(instance, "m_FontMaterialReferenceLookup").Clear();
            Debug.Log("Clearing font asset");
            Reflection.GetPrivateField<Dictionary<int, TMP_FontAsset>>(instance, "m_FontAssetReferenceLookup").Clear();
            Debug.Log("Clearing sprite asset");
            Reflection.GetPrivateField<Dictionary<int, TMP_SpriteAsset>>(instance, "m_SpriteAssetReferenceLookup").Clear();
            Debug.Log("Clearing color gradiant");
            Reflection.GetPrivateField<Dictionary<int, TMP_ColorGradient>>(instance, "m_ColorGradientReferenceLookup").Clear();
        }
    }
}