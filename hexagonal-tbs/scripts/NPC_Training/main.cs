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

            // Create spells and store them in a dictionary
            Dictionary<string, Spell> spellBook = CreateSpellBook();

            int numAgents = 10, numTeams = 3;
            List<Agent> agents = new List<Agent>();

            for(int j = 1; j < numTeams+1; j++){
                for(int i = 0; i < numAgents/numTeams; i++){
                    Agent a = new Agent(agentStats, spellBook);
                    a.setAlignment(j.ToString()[0]);
                    agents.Add(a);
                }
            }

            simManager SimulationManager = new simManager(40, 40, agents);

            Console.Write(agents[0].Alignment);
            Console.Write(SimulationManager.map.ShowMap());
            Console.Write(agents[0].NN.nodeLayers[1][1].Item1.Data);
        }
    }
}
