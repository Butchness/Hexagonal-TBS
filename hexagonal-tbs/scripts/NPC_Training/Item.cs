using System;
using System.Collections.Generic;
using NPC_Training;

namespace NPC_Training
{
    public class Item
    {
        public string Name { get; set; }
        public List<(string Stat, int Value)> Effects { get; private set; } = new List<(string Stat, int Value)>(); // Store each effect

        private static Random random = new Random();

        public Item(){
            Name = "item_Name";
        }

        // The maximum primary stat effect (+3 to +5)
        private const int MaxEffectMin = 3;
        private const int MaxEffectMax = 5;

        // The range for minor effects (-2 to +2)
        private const int MinorEffectMin = -2;
        private const int MinorEffectMax = 2;

        // Generate random item effects for each round
        public void GenerateRandomEffects()
        {
            Effects.Clear();

            // List of possible stats to affect
            var statList = new List<string>
            {
                "Health", "Mana", "Armor", "Fortitude", "WeaponDamage", "Speed", "Range"
            };

            // Primary effect: affect a single stat with +3 to +5
            string primaryStat = statList[random.Next(statList.Count)];
            int primaryEffectValue = random.Next(MaxEffectMin, MaxEffectMax + 1) * (random.Next(0, 2) == 0 ? 1 : -1); // Randomly decide to increase or decrease
            Effects.Add((primaryStat, primaryEffectValue));

            // Now apply two random minor effects (-2 to +2)
            for (int i = 0; i < 2; i++)
            {
                string minorStat = statList[random.Next(statList.Count)];
                int minorEffectValue = random.Next(MinorEffectMin, MinorEffectMax + 1);
                Effects.Add((minorStat, minorEffectValue));
            }
        }
    }
}