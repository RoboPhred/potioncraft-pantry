
using System;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;

namespace RoboPhredDev.PotionCraft.Pantry.PantryPackages
{
    class PantryPackage
    {

        [YamlIgnore]
        public string Name { get; set; }

        [YamlIgnore]
        public string DirectoryPath { get; set; }

        public List<PantryIngredient> Ingredients { get; set; } = new List<PantryIngredient>();

        private void Initialize()
        {
            Ingredients.ForEach(x => x.Initialize(this));
        }

        public void Apply()
        {
            Debug.Log($"[Pantry] Applying {Ingredients.Count} ingredients in {Name}");
            Ingredients.ForEach(x => x.Apply());
        }

        public static PantryPackage Load(string directoryPath)
        {
            var pkg = Deserializer.Deserialize<PantryPackage>(System.IO.Path.Combine(directoryPath, "package.yml"));
            pkg.Name = System.IO.Path.GetFileName(directoryPath);
            pkg.DirectoryPath = directoryPath;
            pkg.Initialize();
            return pkg;
        }
    }
}