using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Sorting2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] array = GenerateRandomArray(15);
            BubbleSort((int[])array.Clone());
            InsertionSort((int[])array.Clone());
            SelectionSort((int[])array.Clone());

            Console.ReadKey();
        }

        static int[] GenerateRandomArray(int size)
        {
            Random rnd = new Random();
            int[] array = new int[size];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rnd.Next(size);
            }
            Console.Write("Generated Array: \t");
            DisplayArray(array);
            return array;
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
            Console.Write("Bubble sort: \t\t");
            for (int i = 0; i < arr.Length-1; i++)
            {
                for (int j = 0; j < arr.Length-i-1; j++)
                {
                    if (arr[j] > arr[j+1])
                    {
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
            DisplayArray(arr);
        }
        static void InsertionSort(int[] arr)
        {
            Console.Write("Insertion sort: \t");
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    if (arr[j] < arr[j-1])
                    {
                        int temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j-1] = temp;
                    }
                }
            }
            DisplayArray(arr);
        }
        static void SelectionSort(int[] arr)
        {
            Console.Write("Selection sort: \t");
            for (int i = 0; i < arr.Length; i++)
            {
                int smallestIndex = i;
                for (int j = i; j < arr.Length; j++)
                {
                    if (arr[j] < arr[smallestIndex])
                    {
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
