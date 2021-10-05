
using System.Collections.Generic;
using System.Linq;
using Npc.Parts;
using Npc.Parts.Settings;
using ObjectBased.Deliveries;
using UnityEngine;

namespace RoboPhredDev.PotionCraft.Pantry
{
    static class TradeDeliveryApplicator
    {
        public static void SetDelivery(string npcTemplate, InventoryItem item, int minCount, int maxCount, float chance)
        {
            var template = NpcTemplate.allNpcTemplates.Find(x => x.name == npcTemplate);
            if (template == null)
            {
                Debug.LogError($"[Pantry] Could not find npc template {npcTemplate} when adding item {item.name}.");
                return;
            }

            NpcTemplate.UsedSubTemplatesOnGetParts.Clear();
            var parts = template.GetListOfPartsToApply();
            var tradeSettings = parts.Item1.OfType<TraderSettings>().ToList();

            if (tradeSettings.Count == 0)
            {
                Debug.Log($"[Pantry] Npc template {npcTemplate} has no trader settings.  Cannot add ingredient {item.name}");
                return;
            }

            foreach (var setting in tradeSettings)
            {
                var category = GetPantryCategory(setting);
                category.deliveries.Add(new Delivery
                {
                    name = item.name,
                    item = item,
                    appearingChance = chance,
                    minCount = minCount,
                    maxCount = maxCount,
                    applyDiscounts = true,
                    applyExtraCharge = true
                });
            }
        }

        private static Category GetPantryCategory(TraderSettings settings)
        {
            var category = settings.deliveriesCategories.Find(x => x.name == "Pantry");
            if (category == null)
            {
                category = new Category
                {
                    name = "Pantry",
                    deliveries = new List<Delivery>()
                };
                settings.deliveriesCategories.Add(category);
            }

            return category;
        }
    }
}