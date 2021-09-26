using System.Collections.Generic;
using System.IO;
using System.Linq;
using RoboPhredDev.PotionCraft.Pantry.PantryPackages;

namespace RoboPhredDev.PotionCraft.Pantry
{
    static class PantryPackageLoader
    {
        public static List<PantryPackage> LoadAllPackages()
        {
            if (!Directory.Exists("pantry"))
            {
                return new List<PantryPackage>();
            }

            var folders = Directory.GetDirectories("pantry");
            return folders.Select(folder => TryLoadPackage(folder)).Where(x => x != null).ToList();
        }

        private static PantryPackage TryLoadPackage(string folder)
        {
            UnityEngine.Debug.Log($"[Pantry] Loading package {folder}");
            try
            {
                return PantryPackage.Load(folder);
            }
            catch (YamlFileException ex)
            {
                UnityEngine.Debug.Log($"[Pantry] Failed to load package \"{ex.FilePath}\": {ex.Message}");
                return null;
            }
        }
    }
}