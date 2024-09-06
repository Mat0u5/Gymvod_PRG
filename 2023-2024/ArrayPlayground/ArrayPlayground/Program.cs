using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2023-2024.
 * Extended by students.
 */

namespace ArrayPlayground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //TODO 1: Vytvoř integerové pole a naplň ho pěti čísly.
            int[] nums = { 0, 1, 2, 3, 4 };
            //TODO 2: Vypiš do konzole všechny prvky pole, zkus klasický for, kde i využiješ jako index v poli, a foreach (vysvětlíme si).
            foreach (var num in nums)
            {
                Console.WriteLine(num);
            }
            int sum = nums.Sum();
            double average = nums.Average();
            int max = nums.Min();
            int min = nums.Max();
            Console.WriteLine("What number do you want to find?");
            int input = 0;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out input)); break;
            }
            int index = Array.IndexOf(nums,input);
            Console.WriteLine($"Array Properties:\n\tSum: {sum}\n\tAverage: {average}\n\tMax: {max}\n\tMin: {min}\n\tIndex of your input: {index}");
            Random rnd = new Random();
            int[] numsTwo = new int[100];
            for (int i = 0; i < numsTwo.Count(); i++)
            {
                numsTwo[i] = rnd.Next(10);
            }
            int[] counts = new int[10];
            for (int i = 0; i < 10; i++)
            {
                counts[i] = numsTwo.ToList().Where(x => x==i).Count();
                Console.WriteLine($"Number of '{i}': " + counts[i]);
            }

            int[] numsTwoReversed = numsTwo;
            Array.Reverse(numsTwoReversed);

            Console.ReadKey();
        }
    }
}
