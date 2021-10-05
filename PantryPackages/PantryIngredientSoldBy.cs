
using YamlDotNet.Serialization;

namespace RoboPhredDev.PotionCraft.Pantry.PantryPackages
{
    class PantryIngredientSoldByTemplate
    {
        public string NpcTemplateName { get; set; }

        public int MinCount { get; set; }
        public int MaxCount { get; set; }

        public float ChanceToAppearPercent { get; set; }

        [YamlIgnore]
        public PantryIngredient Ingredient { get; set; }

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
            TradeDeliveryApplicator.SetDelivery(NpcTemplateName, ingredientItem, MinCount, MaxCount, ChanceToAppearPercent);
        }
    }
}