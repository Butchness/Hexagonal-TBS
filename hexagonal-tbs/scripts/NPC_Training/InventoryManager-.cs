using System;
using System.Collections.Generic;
using NPC_Training;

namespace NPC_Training
{
    public class InventoryManager
    {
        public List<Item> Items { get; private set; }
        private const int InventorySize = 3;

        public InventoryManager()
        {
            Items = new List<Item>();
            GenerateNewItemsForRound();
        }

        // Generate new items for each round
        public void GenerateNewItemsForRound()
        {
            Items.Clear();

            for (int i = 0; i < InventorySize; i++)
            {
                Item newItem = new Item
                {
                    Name = $"Item {i + 1}"
                };
                newItem.GenerateRandomEffects();
                Items.Add(newItem);
            }
        }

        // Access the item effects for decision-making
        public List<(string statName, int effectValue)> GetItemEffects(int index)
        {
            if (index >= 0 && index < Items.Count)
            {
                return Items[index].Effects; // Directly returning the stored effects
            }
            return null;
        }

        // Function to print inventory
        public void PrintInventory()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Item item = Items[i];
                Console.WriteLine($"{item.Name}:");

                foreach (var effect in item.Effects)
                {
                    Console.WriteLine($"- {effect.Stat}: {effect.Value}");
                }
            }
        }
    }
}
