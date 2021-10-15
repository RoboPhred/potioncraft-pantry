
using System.Collections.Generic;

namespace RoboPhredDev.PotionCraft.Pantry.PantryPackages
{
    class PantryIngredientSoldByClass : PantryIngredientSoldBy
    {
        private static readonly Dictionary<NpcClass, string[]> Templates = new()
        {
            {
                NpcClass.Herbalist,
                new[] {
                    "Demo2GroundHogDayHerbalistNpc",
                    "HerbalistNpc 1",
                    "HerbalistNpc 2",
                    "HerbalistNpc 3",
                    "HerbalistNpc 4",
                    "HerbalistNpc 5",
                    "HerbalistNpc 6",
                    "HerbalistNpc 7",
                    "HerbalistNpc 8",
                    "HerbalistNpc_Day1",
                    "HerbalistNpc_Day3",
                }
            },
            {
                NpcClass.DwarfMiner,
                new[] {
                    "Playtest2GroundHogDayDwarfMinerNpc",
                    "DwarfMinerNpc 1",
                    "DwarfMinerNpc 2",
                    "DwarfMinerNpc 3",
                    "DwarfMinerNpc 4",
                }
            },
            {
                NpcClass.Mushroomer,
                new[] {
                    "Demo2GroundHogDayRandomMainTrader",
                    "Demo2GroundHogDayMushroomerNpc",
                    "MushroomerNpc 1",
                    "MushroomerNpc 2",
                    "MushroomerNpc 3",
                    "MushroomerNpc 4",
                    "MushroomerNpc 5",
                    "MushroomerNpc 6",
                    "MushroomerNpc 7",
                    "MushroomerNpc 8",
                    "MushroomerNpc_Day2",
                    "MushroomerNpc_Day4",
                    "MushroomerNpcEarlyGame",
                    "MushroomerNpcLateGame",
                    "MushroomerNpcMidGame",
                    "MushroomerNpcTutorial"
                }
            },
            {
                NpcClass.Alchemist,
                new[] {
                    "Playtest2GroundHogDayAlchemistNpc",
                    "AlchemistNpc 1",
                    "AlchemistNpc 2",
                    "AlchemistNpc 3",
                    "AlchemistNpc 4",
                    "AlchemistNpc 5",
                    "AlchemistNpc 6",

                }
            }
        };

        public NpcClass NpcClass { get; set; }

        protected override IEnumerable<string> GetNpcTemplates()
        {
            if (Templates.TryGetValue(NpcClass, out string[] templates))
            {
                return templates;
            }
            return new string[0];
        }
    }

    public enum NpcClass
    {
        Herbalist,
        DwarfMiner,
        Mushroomer,
        Alchemist,
    }

}