
using System.Collections.Generic;
using RoboPhredDev.PotionCraft.Pantry.Yaml;

namespace RoboPhredDev.PotionCraft.Pantry.PantryPackages
{
    class PantryIngredientSoldByTemplate : PantryIngredientSoldBy
    {
        public MaybeStringArray NpcTemplateName { get; set; }

        protected override IEnumerable<string> GetNpcTemplates()
        {
            return NpcTemplateName;
        }
    }
}