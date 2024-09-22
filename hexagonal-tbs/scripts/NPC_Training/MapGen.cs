using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using System.Xml;
using NPC_Training;

namespace NPC_Training{
    public class MapGenerator
    {
        public List<MapTile> Map { get; private set; }
        private int sizeX, sizeY;

        public MapGenerator(int sizeX, int sizeY) // Constructor made public
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            Map = new List<MapTile>(); // Initialize the map list

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    MapTile tile = new MapTile(i, j); // Create a tile for each coordinate
                    Map.Add(tile); // Add the tile to the map
                }
            }
        }

        // Method to get the tile at a specific (x, y) position
        public MapTile GetTile(int x, int y)
        {
            return Map.Find(t => t.location == (x, y));
        }

        // Method to place a new agent on the map
        public void PlaceAgent(Agent a)
        {
            MapTile tile = GetTile(a.Pos.x, a.Pos.y); // Try to find the location the agent is supposed to be at

            if (tile.covered)
            {
                throw new Exception("Trying to place agent in occupied space!");
            }
            else
            {
                tile.Update(a); // Place the agent on the intended tile
            }
        }

        // Function to show the map
        public void ShowMap()
        {
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    output.Append(GetTile(i, j).icon); // Append each tile's icon
                }
                output.Append('\n'); // Newline after each row
            }

            Console.WriteLine(output.ToString()); // Output the map to the console
        }

        public void Update(Agent a, int x, int y){
            MapTile lastTile = GetTile(a.Pos.x, a.Pos.y); //get the agent's current tile
            MapTile nextTile = GetTile(x,y); //get the agent's current move

            if(nextTile.covered){
                throw new Exception("Unable to move agent to tile, tile covered!");
            }
            
            lastTile.Update(null); //update the tile it used to be at
            nextTile.Update(a); //move the agent to the new location
            a.setLocation(x,y);
        }
    }

    public class MapTile
    {
        public Agent space { get; private set; }
        public bool covered { get; private set; }
        public (int x, int y) location { get; private set; }
        public char icon { get; private set; }

        public MapTile(int x, int y)
        {
            location = (x, y);
            covered = false;
            icon = '0'; // Default icon when no agent is on the tile
        }

        public void Update(Agent agent)
        {
            space = agent; // Update the tile's contents

            if (space != null)
            {
                covered = true;
                icon = agent.Alignment; // Update icon based on agent's alignment
            }
            else
            {
                covered = false;
                icon = '0'; // Reset icon when there's no agent
            }
        }
    }
}