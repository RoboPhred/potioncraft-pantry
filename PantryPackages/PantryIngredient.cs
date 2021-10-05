
using System;
using System.Collections.Generic;
using UnityEngine;
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

        public List<PantryIngredientSoldByTemplate> SoldBy { get; set; } = new List<PantryIngredientSoldByTemplate>();

        [YamlIgnore]
        public PantryPackage Package { get; set; }

        public void Initialize(PantryPackage pkg)
        {
            Package = pkg;

            if (GrindStartPercent > 1)
            {
                GrindStartPercent /= 100;
            }

            SoldBy.ForEach(x => x.Initialize(this));
        }

        public void Apply()
        {
            try
            {
                var ingredient = PantryIngredientRegistry.GetIngredientByName(QualifiedName);
                if (ingredient != null)
                {
                    IngredientFactory.Apply(this, ingredient);
                }
                else
                {
                    ingredient = PantryIngredientRegistry.RegisterIngredient(this);
                }

                SoldBy.ForEach(x => x.Apply(ingredient));
            }
            catch (Exception ex)
            {
                Debug.Log($"[Pantry]: Failed to load ingredient {Name} from package {Package.Name}: {ex.Message}\n{ex.StackTrace}\n\n");
            }
        }
    }
}