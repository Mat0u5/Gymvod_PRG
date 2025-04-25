using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            int[] array = new int[10];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rnd.Next(10);
            }
            Console.Write("Generated Array: ");
            DisplayArray(array);

            //BubbleSort(array);
            //InsertionSort(array);
            SelectionSort(array);

            Console.ReadKey();
        }

        static void DisplayArray(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (i != 0) Console.Write(", ");
                Console.Write(arr[i]);
            }
            Console.WriteLine();
        }

        static void BubbleSort(int[] arr)
        {
            Console.Write("Bubble sort: ");
            for (int i = 0; i < arr.Length-1; i++)
            {
                Boolean swappedAnything = false;
                for (int j = 0; j < arr.Length - i - 1; j++)
                {
                    if (arr[j] > arr[j+1])
                    {
                        swappedAnything = true;
                        int temp = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = temp;
                    }
                }
                if (!swappedAnything) break;
            }
            DisplayArray(arr);
        }
        static void InsertionSort(int[] arr)
        {
            Console.Write("Insertion sort: ");
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        int temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                }
            }
            DisplayArray(arr);
        }
        static void SelectionSort(int[] arr)
        {
            Console.Write("Selection sort: ");
            for(int i = 0; i < arr.Length - 1; i++)
            {
                int smallestValue = arr[i];
                int smallestIndex = i;
                for (int j = i; j < arr.Length; j++)
                {
                    if (smallestValue > arr[j])
                    {
                        smallestValue = arr[j];
                        smallestIndex = j;
                    }
                }

                int temp = arr[i];
                arr[i] = arr[smallestIndex];
                arr[smallestIndex] = temp;
            }
            DisplayArray(arr);
        }
    }
}
