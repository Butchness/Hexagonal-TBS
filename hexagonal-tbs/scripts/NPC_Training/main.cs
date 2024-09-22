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
            AgentStats agent1Stats = new AgentStats(20, 10, 5, 3, 3, 4, 5);
            AgentStats agent2Stats = new AgentStats(20, 10, 5, 3, 3, 4, 5);
            MapGenerator Map = new MapGenerator(25, 100);

            // Output initial agent stats
            Console.WriteLine("Initial Agent Stats:");
            PrintAgentStats(agent1Stats);

            // Create InventoryManager and generate random items
            InventoryManager inventoryManager = new InventoryManager();
            Console.WriteLine("\nGenerated Items:");
            PrintInventory(inventoryManager);

            // Create spells and store them in a dictionary
            Dictionary<string, Spell> spellBook = CreateSpellBook();

            Agent a = new Agent(agent1Stats, spellBook);
            Agent b = new Agent(agent2Stats, spellBook);

            Console.WriteLine("\nCreated agent ");
            Console.WriteLine($"Has the spell: {a.spellSlots.Spells[0].Name}");
            PrintAgentStats(a.statsMax);
            PrintInventory(a.inventory);

            // Instantiate SpellManager
            SpellManager spellManager = new SpellManager(spellBook);
            
            // Cast the randomly selected spell from SpellManager
            Spell selectedSpell = spellManager.Spells[0]; // There's only one spell in the list
            if (selectedSpell.CanCast(agent1Stats, 1))
            {
                Console.WriteLine("\nTarget Stats before spell:");
                PrintAgentStats(agent2Stats);

                Console.WriteLine($"\nAttempting to cast {selectedSpell.Name} on the target.");

                // Apply the effect to the target
                agent2Stats.AdjustStats(selectedSpell.ApplyEffect(agent1Stats));

                Console.WriteLine("\nUpdated Target Stats after spell:");
                PrintAgentStats(agent2Stats);
            }

            // Simulate agent using an item from the inventory
            int itemIndex = 0; // Choosing the first item to use
            Console.WriteLine($"\nUsing Item {itemIndex + 1}: {inventoryManager.Items[itemIndex].Name}");
            UseItemOnAgent(inventoryManager.Items[itemIndex], agent1Stats);

            // Output updated agent stats
            Console.WriteLine("\nUpdated Agent Stats after using the item:");
            PrintAgentStats(agent1Stats);

            Console.WriteLine("\nCurrent Map without agent\n");
            Map.ShowMap();

            a.setLocation(1, 1);
            Console.WriteLine($"{a}'s current location: {a.Pos.x} x, {a.Pos.y} y");
            a.setAlignment('1');
            Map.PlaceAgent(a);

            b.setLocation(3, 3);
            Console.WriteLine($"{b}'s current location: {b.Pos.x} x, {b.Pos.y} y");
            b.setAlignment('2');
            Map.PlaceAgent(b);

            Console.WriteLine("Current Map with new agents\n");
            Map.ShowMap();
        }

        // Create the spell book containing predefined spells
        static Dictionary<string, Spell> CreateSpellBook()
        {
            Dictionary<string, Spell> spellBook = new Dictionary<string, Spell>();

            // Create a fireball spell (damages target)
            AgentStats fireballCasterEffect = new AgentStats(0, -3, 0, 0, 0, 0, 0); // Reduces mana by 3 for caster
            AgentStats fireballTargetEffect = new AgentStats(-12, 0, 0, 0, 0, 0, 0); // Deals 5 damage to the target
            Spell fireball = new Spell("Fireball", 5, 1, fireballCasterEffect, fireballTargetEffect);
            spellBook.Add("Fireball", fireball);

            // Create a healing spell (heals target)
            AgentStats healCasterEffect = new AgentStats(0, -2, 0, 0, 0, 0, 0); // Reduces mana by 2 for caster
            AgentStats healTargetEffect = new AgentStats(2, 0, 0, 0, 0, 0, 0); // Heals 5 health to the target
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
        static void PrintAgentStats(AgentStats agent1Stats)
        {
            Console.WriteLine($"Health: {agent1Stats.Health}/{agent1Stats.MaxHealth}");
            Console.WriteLine($"Mana: {agent1Stats.Mana}/{agent1Stats.MaxMana}");
            Console.WriteLine($"Speed: {agent1Stats.Speed}");
            Console.WriteLine($"Armor: {agent1Stats.Armor}");
            Console.WriteLine($"Fortitude: {agent1Stats.Fortitude}");
            Console.WriteLine($"Weapon Damage: {agent1Stats.WeaponDamage}");
            Console.WriteLine($"Range: {agent1Stats.AttackRange}");
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
        static void UseItemOnAgent(Item item, AgentStats agent1Stats)
        {
            foreach (var effect in item.Effects)
            {
                switch (effect.Stat)
                {
                    case "Health":
                        agent1Stats.Heal(Math.Min(agent1Stats.Health + effect.Value, agent1Stats.MaxHealth)); //this can also reduce health theoretically
                        break;
                    case "Mana":
                        agent1Stats.RegainMana(Math.Min(agent1Stats.Mana + effect.Value, agent1Stats.MaxMana)); //this can also reduce mana theoretically
                        break;
                    case "Armor":
                        agent1Stats.AdjustStats(0, 0, 0, effect.Value, 0, 0, 0);
                        break;
                    case "Fortitude":
                        agent1Stats.AdjustStats(0, 0, 0, 0, effect.Value, 0, 0);
                        break;
                    case "WeaponDamage":
                        agent1Stats.AdjustStats(0, 0, 0, 0, 0, effect.Value, 0);
                        break;
                    case "Speed":
                        agent1Stats.AdjustStats(0, 0, effect.Value, 0, 0, 0, 0);
                        break;
                    case "Range":
                        agent1Stats.AdjustStats(0, 0, 0, 0, 0, 0, effect.Value);
                        break;
                    default:
                        throw new ArgumentException($"Cannot apply unknown effect: {effect.Stat}!");
                }
            }
        }
    }
}
