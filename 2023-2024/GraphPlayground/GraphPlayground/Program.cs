using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;

namespace GraphPlayground
{
    internal class Program
    {
        public static void DFS(Graph graph, Node startNode, Node targetNode = null)
        {
            Console.WriteLine($"DFS Starting: {startNode.index}");
            startNode.visited = true;
            Node currentNode = startNode;
            while(true)
            {
                bool visitedAllNeighbors = true;
                foreach (Node neighborNode in currentNode.neighbors)
                {
                    if (neighborNode.visited) continue;
                    Console.WriteLine($"Path: {currentNode.index} -> {neighborNode.index}");
                    visitedAllNeighbors = false;
                    neighborNode.visited = true;
                    neighborNode.cameFrom = currentNode;
                    currentNode = neighborNode;
                    break;
                }
                if (visitedAllNeighbors)
                {
                    if (currentNode == startNode) break;
                    Console.WriteLine($"Returning: {currentNode.index} -> {currentNode.cameFrom.index}");
                    currentNode = currentNode.cameFrom;
                }
            }

        }
        public static void BFS(Graph graph, Node startNode, Node targetNode = null)
        {
            Console.WriteLine($"BFS Starting: {startNode.index}");
            List<Node> queue = new List<Node>();
            while(true)
            {

            }
        }

        static void Main(string[] args)
        {
            //Create and print the graph
            Graph graph = new Graph();
            graph.PrintGraph();
            graph.PrintGraphForVisualization();

            //Call both algorithms with a random starting node
            Random rng = new Random();
            DFS(graph, graph.nodes[rng.Next(0, graph.nodes.Count)]);
            BFS(graph, graph.nodes[rng.Next(0, graph.nodes.Count)]);

            Console.ReadKey();
        }
    }
}
