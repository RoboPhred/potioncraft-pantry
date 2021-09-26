
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace RoboPhredDev.PotionCraft.Pantry.PantryPackages
{
    class PantryPackage
    {

        [YamlIgnore]
        public string Name { get; set; }

        public List<PantryIngredient> Ingredients { get; set; } = new List<PantryIngredient>();

        public static PantryPackage Load(string fullPath)
        {
            var pkg = Deserializer.Deserialize<PantryPackage>(System.IO.Path.Combine(fullPath, "package.yml"));
            pkg.Name = System.IO.Path.GetDirectoryName(fullPath);
            return pkg;
        }
    }
}