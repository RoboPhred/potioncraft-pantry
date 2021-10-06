using System;
using System.IO;
using UnityEngine;

namespace RoboPhredDev.PotionCraft.Pantry
{
    static class TextureLoader
    {
        public static Texture2D LoadTexture(string filePath)
        {
            var data = File.ReadAllBytes(filePath);
            var tex = new Texture2D(0, 0);
            if (!tex.LoadImage(data))
            {
                throw new Exception("Failed to load image from file: " + filePath);
            }
            return tex;
        }
    }
}