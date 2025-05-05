using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Seznamy
{
    internal class Program
    {
        private static Random rnd = new Random();
        static void Main(string[] args)
        {
            int[] predefinedArray = new int[] {1,2,23,45};
            List<int> predefinedList = new List<int>() {5, 5, 5, 5 };
            int[,] twoDimArray = new int[2,3];

            int[] array = randomArray(10);
            print("Pole", array);
            int value = array[0];
            Console.WriteLine("Value in array at index 0 is:" + value);

            Console.WriteLine();

            List<int> list = new List<int>();
            addToList(list, 5);
            print("List", list);
            addToList(list, 5);
            print("List", list);
            value = list[4];
            Console.WriteLine("Value in list at index 4 is:" + value);
            list.RemoveAt(4);
            Console.WriteLine("Removed value in list at index 4.");
            value = list[4];
            Console.WriteLine("Value in list at index 4 is now:" + value);
            print("List", list);

            Console.WriteLine();

            List<int> shallowCopy = list;
            shallowCopy.Add(100);
            print("List Shallow Copy\t", shallowCopy);
            print("List\t\t\t", list);
            Console.WriteLine();

            List<int> deepCopy = new List<int>(list);
            deepCopy.Add(999);
            print("List Deep Copy\t\t", deepCopy);
            print("List\t\t\t", list);

            Console.ReadKey();
        }

        static void print(String name, int[] arr)
        {
            Console.WriteLine(name + ": " + String.Join(", ", arr));
        }
        static void print(String name, List<int> list)
        {
            Console.WriteLine(name + ": " + String.Join(", ", list));
        }

        static int[] randomArray(int size)
        {
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = rnd.Next(size);
            }
            return array;
        }


        static void addToList(List<int> list, int addRandom)
        {
            Console.WriteLine("Adding " + addRandom + " random integers to the list.");
            for (int i = 0; i < addRandom; i++)
            {
                list.Add(rnd.Next(10));
            }
        }
    }
}
