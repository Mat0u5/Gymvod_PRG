﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SearchPlayground
{
    internal class Program
    {
        static int LinearSearch(int[] array, int elementToSearch)
        {
            for (int i = 0; i < array.Length-1;i++)
            {
                if (array[i] == elementToSearch)
                {
                    return i;
                }
            }
            return -1;
        }

        static int BinarySearch(int[] array, int elementToSearch)
        {
            int lowerBound = 0;
            int upperBound = array.Length -1;
            int cycles = 0;
            while (lowerBound <= upperBound)
            {
                int middle = (upperBound + lowerBound) / 2;
                if (array[middle] == elementToSearch)
                {
                    Console.WriteLine($"binary search cycles: {cycles}");
                    return middle;
                }
                else if (array[middle] > elementToSearch) upperBound = middle - 1;
                else if (array[middle] < elementToSearch) lowerBound = middle + 1;
                cycles++;
            }
            return -1;
        }

        static int BinarySearchRecursive(int[] array, int elementToSearch, int lower, int upper)
        {
            int middle = (upper + lower) / 2;
            if (array[middle] == elementToSearch) return middle;
            else if (array[middle] > elementToSearch) return BinarySearchRecursive(array, elementToSearch, lower, middle - 1);
            else if (array[middle] < elementToSearch) return BinarySearchRecursive(array,elementToSearch, middle + 1, upper);
            return -1;
        }

        //Naplní pole náhodnými rostoucími čísly.
        static void FillArray(int[] array)
        {
            Random rng = new Random();
            int lastNumber = 0;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = lastNumber + rng.Next(1, 10);
                lastNumber = array[i];
            }
        }

        //Vypíše pole do konzole
        static void WriteArrayToConsole(int[] array, string arrayName)
        {
            Console.Write(arrayName + " = [");
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i]);
                if (i < array.Length - 1)
                {
                    Console.Write(", ");
                }
            }
            Console.Write("]\n\n");
        }

        //Zavolá postupně lineární, binární a rekurzivní binární vyhledávání pro zadané pole (a vypíše jeho jméno pro přehlednost)
        static void SearchArray(int[] array, string arrayName)
        {
            Random rng = new Random();
            int randomElement = array[rng.Next(array.Length)];
            Console.WriteLine($"Prohledávám {arrayName} a hledám v něm prvek {randomElement}:");
            int index;

            index = LinearSearch(array, randomElement);
            Console.WriteLine($"    Lineární vyhledávání našlo prvek {randomElement} na indexu {index}");

            index = BinarySearch(array, randomElement);
            Console.WriteLine($"    Binární vyhleádávání našlo prvek {randomElement} na indexu {index}");

            index = BinarySearchRecursive(array, randomElement, 0, array.Length - 1);
            Console.WriteLine($"    Rekurzivní binární vyhledávání našlo prvek {randomElement} na indexu {index}");

            Console.WriteLine();
        }

        static void Main(string[] args)
        {

            
            int[] smallArray = new int[10];
            FillArray(smallArray);
            
            int[] mediumArray = new int[1000];
            FillArray(mediumArray);

            int[] largeArray = new int[1000000];
            FillArray(largeArray);
            

            WriteArrayToConsole(smallArray, "Malé pole");
            SearchArray(smallArray, "Malé pole");

            //WriteArrayToConsole(mediumArray, "Střední pole");
            SearchArray(mediumArray, "Střední pole");

            //WriteArrayToConsole(largeArray, "Velké pole");
            SearchArray(largeArray, "Velké pole");
            
            Console.ReadKey();
        }
    }
}
