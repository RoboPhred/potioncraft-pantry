
using YamlDotNet.Serialization;

namespace RoboPhredDev.PotionCraft.Pantry.PantryPackages
{
    class PantryIngredient
    {
        public string Name { get; set; }

        [YamlIgnore]
        public string QualifiedName
        {
            get
            {
                return $"{Package.Name}.{Name}";
            }
        }

        public string Description { get; set; }

        public string InventoryImage { get; set; }

        public string RecipeImage { get; set; }

        public string IngredientBase { get; set; }

        public float GrindStartPercent { get; set; }

        public PantryIngredientPath Path { get; set; }

        public bool IsCrystal { get; set; }

        [YamlIgnore]
        public PantryPackage Package { get; set; }
    }
}