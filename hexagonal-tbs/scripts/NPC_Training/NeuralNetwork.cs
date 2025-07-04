using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices;
using NPC_Training;

namespace NPC_Training{
    public class NeuralNetwork
    {
        public List<List<(Node, double)>> nodeLayers { get; private set; } // 2D list of node layers
        public (int, int) numIO { get; private set; }

        public NeuralNetwork(int numIn, int numOut, List<MapTile> vis, char alignment, AgentStats maxStats, AgentStats curStats, InventoryManager inv, SpellManager spells)
        {
            numIO = (numIn, numOut);
            nodeLayers = new List<List<(Node, double)>>();

            // Initialize the input layer
            var inputLayer = new List<(Node, double)>();
            foreach (MapTile tile in vis)
            {
                inputLayer.AddRange<(Node,double)>(tileToList(tile), 1.0); // Add nodes with a default weight of 1.0
            }

            // Ensure the input layer has a fixed size, adding placeholders if needed
            for(int i = 0; i < inputLayer.Count; i++)
            {
                inputLayer.Add((new Node(), 1)); // Placeholder nodes with default weight
            }

            nodeLayers.Add(inputLayer);

            // Initialize hidden layers or other logic as needed
            // (You can add logic here to dynamically create hidden layers)

            // Initialize the output layer
            var outputLayer = new List<(Node, double)>();
            for (int i = 0; i < numOut; i++)
            {
                outputLayer.Add((new Node(NodeType.Output), 0)); // Output nodes with default weight
            }

            nodeLayers.Add(outputLayer);

            Console.WriteLine($"Neural network initialized with {nodeLayers.Count} layers.");
        }

        // Helper function to convert a MapTile into a list of input nodes
        private List<Node> tileToList(MapTile tile)
        {
            var nodelist = new List<Node>();

            //nodelist.Add(new Node(tile.covered); // If the tile is covered
            nodelist.Add(new Node(tile.space.statsCurrent.Health));
            nodelist.Add(new Node(tile.space.statsCurrent.Mana));
            nodelist.Add(new Node(tile.space.statsCurrent.Speed));
            nodelist.Add(new Node(tile.space.statsCurrent.Armor));
            nodelist.Add(new Node(tile.space.statsCurrent.MaxHealth));
            nodelist.Add(new Node(tile.space.statsCurrent.MaxMana));
            nodelist.Add(new Node(tile.space.statsCurrent.Fortitude));
            nodelist.Add(new Node(tile.space.statsCurrent.WeaponDamage));
            nodelist.Add(new Node(tile.space.statsCurrent.AttackRange));

            nodelist.Add(new Node(tile.space.statsMax.Health));
            nodelist.Add(new Node(tile.space.statsMax.Mana));
            nodelist.Add(new Node(tile.space.statsMax.Speed));
            nodelist.Add(new Node(tile.space.statsMax.Armor));
            nodelist.Add(new Node(tile.space.statsMax.MaxHealth));
            nodelist.Add(new Node(tile.space.statsMax.MaxMana));
            nodelist.Add(new Node(tile.space.statsMax.Fortitude));
            nodelist.Add(new Node(tile.space.statsMax.WeaponDamage));
            nodelist.Add(new Node(tile.space.statsMax.AttackRange));

            nodelist.Add(new Node(tile.space.Alignment)); // The team alignment

            return nodelist;
        }
    }

    public class Node
    {
        public List<(Node previousNode, double Weight)> Connections { get; private set; }
        public double Bias { get; set; }
        public NodeType NodeType { get; private set; }
        public double ActivationVal { get; private set; }
        public double Data { get; private set; }

        // Constructor for hidden nodes
        public Node(NodeType nodeType = NodeType.Hidden)
        {
            Connections = new List<(Node, double)>();
            Bias = 0;
            ActivationVal = 0;
            NodeType = nodeType;
        }

        // Constructor for input nodes
        public Node(double data) : this(NodeType.Input)
        {
            Data = data;
        }

        // Activation logic
        public double Activate()
        {
            if (NodeType == NodeType.Input)
                return Data; // Input nodes return their data

            double sum = Connections.Sum(connection => connection.previousNode.ActivationVal * connection.Weight);
            sum += Bias;
            ActivationVal = Sigmoid(sum);
            return ActivationVal;
        }

        private double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
    }

    public enum NodeType
    {
        Input,
        Hidden,
        Output
    }
}