using System;
using NPC_Training;

namespace NPC_Training_Test
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            // Create agent stats
            AgentStats agentStats = new AgentStats(100, 50, 10, 5, 3, 20, 5);

            // Output initial agent stats
            Console.WriteLine("Initial Agent Stats:");
            PrintAgentStats(agentStats);

            // Create InventoryManager and generate random items
            InventoryManager inventoryManager = new InventoryManager();
            Console.WriteLine("\nGenerated Items:");
            PrintInventory(inventoryManager);

            // Simulate agent using an item from the inventory
            int itemIndex = 0; // Choosing the first item to use
            Console.WriteLine($"\nUsing Item {itemIndex + 1}: {inventoryManager.Items[itemIndex].Name}");
            UseItemOnAgent(inventoryManager.Items[itemIndex], agentStats);

            // Output updated agent stats
            Console.WriteLine("\nUpdated Agent Stats after using the item:");
            PrintAgentStats(agentStats);
        }

        // Function to print agent stats
        static void PrintAgentStats(AgentStats agentStats)
        {
            Console.WriteLine($"Health: {agentStats.Health}/{agentStats.MaxHealth}");
            Console.WriteLine($"Mana: {agentStats.Mana}/{agentStats.MaxMana}");
            Console.WriteLine($"Speed: {agentStats.Speed}");
            Console.WriteLine($"Armor: {agentStats.Armor}");
            Console.WriteLine($"Fortitude: {agentStats.Fortitude}");
            Console.WriteLine($"Weapon Damage: {agentStats.WeaponDamage}");
            Console.WriteLine($"Range: {agentStats.AttackRange}");
        }

        // Function to print inventory
        static void PrintInventory(InventoryManager inventoryManager)
        {
            for (int i = 0; i < inventoryManager.Items.Count; i++)
            {
                Item item = inventoryManager.Items[i];
                Console.WriteLine($"{item.Name}:");

                foreach (var effect in item.Effects)
                {
                    Console.WriteLine($"- {effect.Stat}: {effect.Value}");
                }
            }
        }

        // Function to apply the item to the agent's stats
        static void UseItemOnAgent(Item item, AgentStats agentStats)
        {
            foreach (var effect in item.Effects)
            {
                switch (effect.Stat)
                {
                    case "Health":
                        agentStats.Health = Math.Min(agentStats.Health + effect.Value, agentStats.MaxHealth);
                        break;
                    case "Mana":
                        agentStats.Mana = Math.Min(agentStats.Mana + effect.Value, agentStats.MaxMana);
                        break;
                    case "Armor":
                        agentStats.Armor += effect.Value;
                        break;
                    case "Fortitude":
                        agentStats.Fortitude += effect.Value;
                        break;
                    case "WeaponDamage":
                        agentStats.WeaponDamage += effect.Value;
                        break;
                    case "Speed":
                        agentStats.Speed += effect.Value;
                        break;
                    case "Range":
                        agentStats.AttackRange += effect.Value;
                        break;
                }
            }
        }
    }
}
