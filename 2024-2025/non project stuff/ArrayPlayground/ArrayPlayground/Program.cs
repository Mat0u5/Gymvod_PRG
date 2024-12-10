using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2024-2025.
 * Extended by students.
 */

namespace ArrayPlayground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] array = {1, 3, 4, 6, 7};

            //TODO 2: Vypiš do konzole všechny prvky pole, zkus jak klasický for, kde i využiješ jako index v poli, tak foreach.
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i]+", ");
            }
            Console.WriteLine("");
            foreach (int i in array)
            {
                Console.Write(i+ ", ");
            }
            Console.WriteLine("");



            int sum = 0;
            int average;
            int max = array[0];
            int min = array[0];
            foreach (int i in array)
            {
                sum += i;
                if (i > max) max = i;
                if (i < min) min = i;
            }
            average = sum / array.Length;
            Console.WriteLine($"Sum: {sum}");
            Console.WriteLine($"Average: {average}");
            Console.WriteLine($"Max: {max}");
            Console.WriteLine($"Min: {min}");


            int index;
            while(true)
            {
                Console.WriteLine("Enter a number.");
                if (Int32.TryParse(Console.ReadLine(), out int newNum))
                {
                    if (newNum < 0) continue;
                    if (newNum >= array.Length) continue;
                    index = newNum;
                    break;
                }
                continue;
            }
            Console.WriteLine($"Number at index {index} is {array[index]}");



            Random rng = new Random();
            int[] newArray = new int[100];
            for (int i = 0; i < 100; i++)
            {
                newArray[i] = rng.Next(10);
            }
            array = newArray;
            Console.Write("Changed array: ");
            foreach (int i in array)
            {
                Console.Write(i + ", ");
            }
            Console.WriteLine("");


            int[] counts = new int[10];
            for (int i = 0; i < array.Length; i++)
            {
                int value = array[i];
                counts[value] += 1;
            }
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Array contains the number {i} {counts[i]} times");
            }



            int[] reversed = new int[array.Length];
            for ( int i = 0;i < array.Length; i++)
            {
                reversed[array.Length - i - 1] = array[i];
            }
            Console.Write("Reversed array: ");
            foreach (int i in array)
            {
                Console.Write(i + ", ");
            }
            Console.WriteLine("");


            Console.ReadKey();
        }
    }
}
