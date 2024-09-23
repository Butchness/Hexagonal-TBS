using System;
using System.Collections.Generic;
using NPC_Training;

namespace NPC_Training{
    public class simManager
    {
        public MapGenerator map {get; private set;}
        public List<Agent> agentList{get; private set;}

        public simManager(int l, int w, List<Agent> agents)
        {
            // Make the map
            map = new MapGenerator(l, w);
            agentList = agents;

            // Place the agents on the map
            foreach(var agent in agents){
                bool a = true;
                while(a){
                    Random random = new Random();
                    int x = random.Next(l);
                    int y = random.Next(w);

                    MapTile tile = map.GetTile(x,y);
                    if(!(tile.covered)){
                        tile.Update(agent);
                        agent.setLocation(x,y);
                        map.PlaceAgent(agent);
                        a = false;
                    }
                }
            }
        }

        // Function to apply the item to the agent's stats
        static void UseItemOnAgent(Item item, Agent agent)
        {
            AgentStats agent1Stats = agent.statsCurrent;
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