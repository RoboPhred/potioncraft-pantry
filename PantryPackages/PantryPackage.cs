
using System.Collections.Generic;
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
            foreach (var ingredient in Ingredients)
            {
                ingredient.Package = this;
            }
        }

        public static PantryPackage Load(string directoryPath)
        {
            var pkg = Deserializer.Deserialize<PantryPackage>(System.IO.Path.Combine(directoryPath, "package.yml"));
            pkg.Name = System.IO.Path.GetDirectoryName(directoryPath);
            pkg.DirectoryPath = directoryPath;
            pkg.Initialize();
            return pkg;
        }
    }
}