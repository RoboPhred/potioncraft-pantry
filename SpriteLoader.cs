using System;
using System.IO;
using UnityEngine;

namespace RoboPhredDev.PotionCraft.Pantry
{
    static class SpriteLoader
    {
        public static Sprite LoadSpriteFromFile(string filePath)
        {
            var data = File.ReadAllBytes(filePath);
            var tex = new Texture2D(2, 2);
            if (!tex.LoadImage(data))
            {
                throw new Exception("Failed to load image from file: " + filePath);
            }

            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
    }
}