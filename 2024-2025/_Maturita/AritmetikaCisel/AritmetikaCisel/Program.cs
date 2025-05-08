using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AritmetikaCisel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Byte: \t" + Byte.MinValue + " to " + Byte.MaxValue);
            Console.WriteLine("Short: \t" + short.MinValue + " to " + short.MaxValue);
            Console.WriteLine("Int: \t" + int.MinValue + " to " + int.MaxValue);
            Console.WriteLine("Long: \t" + long.MinValue + " to " + long.MaxValue);

            Console.WriteLine();

            int overflowInt = (int) Math.Pow(12, 12);
            long overflowLong = (long) Math.Pow(12, 12);
            Console.WriteLine("12^12 saved to an integer: " + overflowInt);
            Console.WriteLine("12^12 saved to a long: " + overflowLong);



            Console.WriteLine();
            Console.WriteLine("Enter a number.");
            int number;
            while (true)
            {
                String input = Console.ReadLine();
                try
                {
                    int inputNumber = Convert.ToInt32(input);
                    number = inputNumber;
                    break;
                }
                catch { 
                    Console.WriteLine("Invalid number, try again.");
                }
            }
            Console.WriteLine("num " + number);
            Console.WriteLine("num + 1 = " + (number + 1));
            Console.WriteLine("num * 2 = " + (number * 1));
            Console.WriteLine("num ^ 2 = " + (Math.Pow(number,2)));

            Console.WriteLine("num in base 10: " + number);
            Console.WriteLine("num in base 2: " + Convert.ToString(number, 2));
            Console.WriteLine("num in base 16: " + Convert.ToString(number, 16));

            Console.WriteLine();
            double circumference = 2 * Math.PI * number;
            double area = Math.PI * Math.Pow(number, 2);
            Console.WriteLine("A circle with a radius of " + number + " has a circumference of " + circumference + " and an area of " + area);


            Console.ReadKey();
        }
    }
}
