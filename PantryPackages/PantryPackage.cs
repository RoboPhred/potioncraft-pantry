
using System.Collections.Generic;

namespace RoboPhredDev.PotionCraft.Pantry.PantryPackages
{
    class PantryPackage
    {

        public List<PantryIngredient> Ingredients { get; set; } = new List<PantryIngredient>();

        public static PantryPackage Load(string folderPath)
        {
            return Deserializer.Deserialize<PantryPackage>(System.IO.Path.Combine("pantry", folderPath, "package.yml"));
        }
    }
}