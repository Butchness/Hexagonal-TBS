using System;
using System.Collections.Generic;
using NPC_Training;

namespace NPC_Training_Test
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            // Create agent stats
            AgentStats agentStats = new AgentStats(20, 10, 5, 3, 3, 4, 5);
            AgentStats targetStats = new AgentStats(30, 15, 4, 5, 5, 6, 3); // Create target stats for testing

            // Output initial agent stats
            Console.WriteLine("Initial Agent Stats:");
            PrintAgentStats(agentStats);

            // Create InventoryManager and generate random items
            InventoryManager inventoryManager = new InventoryManager();
            Console.WriteLine("\nGenerated Items:");
            PrintInventory(inventoryManager);

            // Create spells and store them in a dictionary
            Dictionary<string, Spell> spellBook = CreateSpellBook();
            
            // Output the spells available
            Console.WriteLine("\nSpells Available:");
            foreach (var spell in spellBook)
            {
                Console.WriteLine($"{spell.Key}: {spell.Value.Name}");
            }

            string spellToCast = "Fireball";
            Spell fireball = spellBook[spellToCast];

            if (fireball.CanCast(agentStats, 1))
            {
                Console.WriteLine("\nTarget Stats before spell:");
                PrintAgentStats(targetStats);

                Console.WriteLine($"\nAttempting to cast {fireball.Name} on the target.");

                // Apply the effect to the caster and target
                targetStats.AdjustStats(fireball.ApplyEffect(agentStats));

                Console.WriteLine("\nUpdated Target Stats after spell:");
                PrintAgentStats(targetStats);
            }

            // Simulate agent using an item from the inventory
            int itemIndex = 0; // Choosing the first item to use
            Console.WriteLine($"\nUsing Item {itemIndex + 1}: {inventoryManager.Items[itemIndex].Name}");
            UseItemOnAgent(inventoryManager.Items[itemIndex], agentStats);

            // Output updated agent stats
            Console.WriteLine("\nUpdated Agent Stats after using the item:");
            PrintAgentStats(agentStats);
        }

        // Create the spell book containing predefined spells
        static Dictionary<string, Spell> CreateSpellBook()
        {
            Dictionary<string, Spell> spellBook = new Dictionary<string, Spell>();

            // Create a fireball spell (damages target)
            AgentStats fireballCasterEffect = new AgentStats(0, -3, 0, 0, 0, 0, 0); // Reduces mana by 3 for caster
            AgentStats fireballTargetEffect = new AgentStats(-6, 0, 0, 0, 0, 0, 0); // Deals 5 damage to the target
            Spell fireball = new Spell("Fireball", 5, 1, fireballCasterEffect, fireballTargetEffect);
            spellBook.Add("Fireball", fireball);

            // Create a healing spell (heals target)
            AgentStats healCasterEffect = new AgentStats(0, -2, 0, 0, 0, 0, 0); // Reduces mana by 2 for caster
            AgentStats healTargetEffect = new AgentStats(5, 0, 0, 0, 0, 0, 0); // Heals 5 health to the target
            Spell healing = new Spell("Healing", 4, 1, healCasterEffect, healTargetEffect);
            spellBook.Add("Healing", healing);

            // Create a shield spell (boosts target's armor)
            AgentStats shieldCasterEffect = new AgentStats(0, -4, 0, 0, 0, 0, 0); // Reduces mana by 4 for caster
            AgentStats shieldTargetEffect = new AgentStats(0, 0, 0, 3, 0, 0, 0); // Boosts armor by 3 for the target
            Spell shield = new Spell("Shield", 3, 2, shieldCasterEffect, shieldTargetEffect);
            spellBook.Add("Shield", shield);

            return spellBook;
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
                        agentStats.Heal(Math.Min(agentStats.Health + effect.Value, agentStats.MaxHealth)); //this can also reduce health theoretically
                        break;
                    case "Mana":
                        agentStats.RegainMana(Math.Min(agentStats.Mana + effect.Value, agentStats.MaxMana)); //this can also reduce mana theoretically
                        break;
                    case "Armor":
                        agentStats.AdjustStats(0, 0, 0, effect.Value, 0, 0, 0);
                        break;
                    case "Fortitude":
                        agentStats.AdjustStats(0, 0, 0, 0, effect.Value, 0, 0);
                        break;
                    case "WeaponDamage":
                        agentStats.AdjustStats(0, 0, 0, 0, 0, effect.Value, 0);
                        break;
                    case "Speed":
                        agentStats.AdjustStats(0, 0, effect.Value, 0, 0, 0, 0);
                        break;
                    case "Range":
                        agentStats.AdjustStats(0, 0, 0, 0, 0, 0, effect.Value);
                        break;
                    default:
                        throw new ArgumentException($"Cannot apply unknown effect: {effect.Stat}!");
                }
            }
        }
    }
}
