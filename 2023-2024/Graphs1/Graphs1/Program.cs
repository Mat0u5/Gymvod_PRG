using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Node node1 = new Node(1);
            Node node2 = new Node(2);
            Node node3 = new Node(3);
            Node node4 = new Node(4);
            Node node5 = new Node(5);
            Node node6 = new Node(6);
            Node node7 = new Node(7);
            Node node8 = new Node(8);
            Node node9 = new Node(9);
            Node node10 = new Node(10);
            node1.addChildren(new List<Node>() {node2,node3,node4});
            node2.addChildren(new List<Node>() { node5, node6});
            node3.addChildren(new List<Node>() { node7, node8});
            node4.addChildren(new List<Node>() { node9, node10});

            Node currentNode = node1;
            while(true)
            {
                List<Node> connectedNodes = new List<Node>();
                connectedNodes.AddRange(currentNode.children);

                Console.WriteLine($"CurrentPos: {currentNode.index}");
                if (currentNode.parent != null)
                {
                    connectedNodes.Add(currentNode.parent);
                    Console.WriteLine($"\tParent: {currentNode.parent.index}");
                }
                Console.Write($"\tChildren: ");
                foreach (Node child in currentNode.children) Console.Write(child.index + " ");
                Console.Write("\n");
                Console.WriteLine($"Where do you want to move?");
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out int moveToIndex))
                    {
                        Console.WriteLine($"That's not a valid number, try again.");
                        continue;
                    }
                    Node moveToNode = connectedNodes.Find(node => node.index == moveToIndex);
                    if (moveToNode == null) continue;
                    currentNode = moveToNode;
                    Console.WriteLine("");
                    break;
                }
            }
        }
    }
    class Node
    {
        public int index;
        public Node parent;
        public List<Node> children = new List<Node>();
        public Node(int index)
        {
            this.index = index;
        }
        public Node(int index, List<Node> children) 
        {
            this.index = index;
            this.children = children;
        }

        public void addChildren(List<Node> children)//automatically adds this as a parent of the children
        {
            foreach (Node child in children)
            {
                child.parent = this;
                this.children.Add(child);
            }
        }
    }
}
