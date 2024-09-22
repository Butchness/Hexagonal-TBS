using System;
using System.Collections.Generic;
using System.Data;
using NPC_Training;

namespace NPC_Training{
    public class NeuralNetwork{

    }

    
    public class Node{
        public List<(Node previousNode, double Weight)> Connections {get; private set;}
        public double Bias {get; private set;}
        public double ActivationVal{get; private set;}

        public Node(int numInputs, List<(Node previousNode, double Weight)> connections, double bias)
        {
            Connections = connections ?? new List<(Node, double)>();
            Bias = bias;
        }

        public Node(){
            ActivationVal = 0;
            Bias = 0;
        }

        private double Activate()
        {  
            //Function that will return the activated value based on it's 
            //parent nodes and its own bias
            
            if(Connections.Count == 0)
                return ActivationVal; //input node

            double sum = 0;
            foreach(var connection in Connections)
            {
                sum += connection.previousNode.ActivationVal * connection.Weight;
            }
            sum += Bias;
            ActivationVal = Sigmoid(sum);
            return ActivationVal;
        }

        private double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x)); //Sigmoid activation function
        }

    }
}