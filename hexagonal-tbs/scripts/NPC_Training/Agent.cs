using System;
using System.Collections.Generic;
using System.IO.Pipes;
using NPC_Training;

namespace NPC_Training{
    public class Agent
    {
        public AgentStats statsMax {get; private set;}
        public AgentStats statsCurrent{get; private set;}
        public InventoryManager inventory{get; private set;}
        public SpellManager spellSlots{get; private set;}
        public NeuralNetwork NN{get; private set;}
        public List<MapTile> vis {get; private set;} //5x5 map around the agent
        public (int x, int y) Pos {get; private set;}
        public char Alignment {get; private set;}

        public Agent(AgentStats startingStats, Dictionary<string, Spell> spellBook)
        {  
            statsMax = startingStats;
            statsCurrent = startingStats;
            inventory = new InventoryManager();
            spellSlots = new SpellManager(spellBook);
            Alignment = '1';
        }
        
        // Function that will set the character that shows the allignment of
        // the agent for vision as well as displaying on the map
        public void setAlignment(char alignment){
            Alignment = alignment;
        }

        public void Perceive(List<MapTile> map){
            //Function that is used to allow the Simulation manager to update the
            //agent's current vision
            
            if(map.Count > 25)
                throw new Exception("Visual map passed to unit is too large!");

            vis = map; //the sim manager will give this to the agent
        }

        // Function for the Simulation manager to update the agent's percieving location
        public void setLocation(int x, int y)
        {
            Pos = (x,y);
        }

        public List<int> descisionList(){
            //Function that will take its current information passing it throught its NN
            //to return a rank ordered list of actions the Agent wants to do in order
            List<int> a = new List<int>();
            return a;
        }
    }
}