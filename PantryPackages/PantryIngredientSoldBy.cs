
using System.Collections.Generic;
using RoboPhredDev.PotionCraft.Pantry.Yaml;
using YamlDotNet.Serialization;

namespace RoboPhredDev.PotionCraft.Pantry.PantryPackages
{
    [DuckTypeCandidate(typeof(PantryIngredientSoldByClass))]
    [DuckTypeCandidate(typeof(PantryIngredientSoldByTemplate))]
    abstract class PantryIngredientSoldBy
    {
        public int MinCount { get; set; }
        public int MaxCount { get; set; }

        public float ChanceToAppearPercent { get; set; }

        [YamlIgnore]
        public PantryIngredient Ingredient { get; set; }

        protected abstract IEnumerable<string> GetNpcTemplates();

        public void Initialize(PantryIngredient ingredient)
        {
            Ingredient = ingredient;

            if (ChanceToAppearPercent > 1)
            {
                ChanceToAppearPercent /= 100;
            }
        }

        public void Apply(Ingredient ingredientItem)
        {
            foreach (var name in GetNpcTemplates())
            {
                TradeDeliveryApplicator.SetDelivery(name, ingredientItem, MinCount, MaxCount, ChanceToAppearPercent);
            }
        }
    }
}