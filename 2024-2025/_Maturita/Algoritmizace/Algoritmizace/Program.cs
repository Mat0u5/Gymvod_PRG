using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmizace
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ArrayStuff();
            PrimeNumberChecker();
        }
        static void ArrayStuff()
        {
            int[] array = { 5,6,1,9,3,0,8,7,5};
            int max = array[0];
            int min = array[0];
            foreach(int i in array)
            {
                if (i > max) max = i;
                if (i < min) min = i;
            }
            Console.WriteLine("Array max: " + max);
            Console.WriteLine("Array min: " + min);
            Console.ReadKey();
        }
        static void PrimeNumberChecker()
        {
            while(true)
            {
                int number;
                do Console.WriteLine("Enter a number.");
                while (!Int32.TryParse(Console.ReadLine(), out number));

                Console.WriteLine(number + (IsPrimeNumber(number) ? " is " : " is not ") + "a prime number.\n");
            }
        }
        static bool IsPrimeNumber(int number)
        {
            number = Math.Abs(number);
            if (number <= 1) return false;
            for (int i = 2; i <= (number/2); i++) {
                if (number % i == 0) return false;
            }
            return true;
        }
    }
}
