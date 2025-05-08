using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyklusRekurze
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //0, 1, 1, 2, 3, 5, 8, 13, 21, 34

            Console.WriteLine("Enter a number");
            int number;
            while(!Int32.TryParse(Console.ReadLine(), out number)) {}
            int original = number;

            Console.WriteLine("[RECURSIVE] #" + number + " of the Fibonacci sequence is: " + FibonacciRecursive(7));
            Console.WriteLine("[RECURSIVE] #" + number + " of the Factorial sequence is: " + FactorialRecursive(6));
            Console.WriteLine("#" + number + " of the Fibonacci sequence is: " + Fibonacci(7));
            Console.WriteLine("#" + number + " of the Factorial sequence is: " + Factorial(6));

            Console.WriteLine();

            cycles(number);

            Console.ReadKey();
        }
        static void cycles(int number)
        {
            //For cyklus
            Console.Write("For: ");
            for (int i = 10; i < 100; i += 5)
            {
                if (i != 10) Console.Write(", ");
                Console.Write(i);
            }
            Console.WriteLine();

            //While cyklus
            Console.Write("While: ");
            int whileNum = number;
            while (whileNum > 5)
            {
                if (whileNum != number) Console.Write(", ");
                Console.Write(whileNum);
                whileNum--;
            }
            Console.WriteLine();

            //Do while cyklus
            Console.Write("Do while: ");
            int doWhileNum = number;
            do
            {
                if (doWhileNum != number) Console.Write(", ");
                Console.Write(doWhileNum);
                doWhileNum--;
            }
            while (doWhileNum > 5);
            Console.WriteLine();

            //Foreach cyklus
            Console.Write("Foreach: ");
            int[] ints = new int[] { 7,9,3,4,1};
            bool started = false;
            foreach(int i in ints)
            {
                if (started) Console.Write(", ");
                Console.Write(i);
                started = true;
            }
            Console.WriteLine();
        }
        static int FibonacciRecursive(int n)
        {
            if (n == 0) return 0;
            if (n == 1) return 1;
            return FibonacciRecursive(n - 1) + FibonacciRecursive(n - 2);
        }
        static int FactorialRecursive(int n)
        {
            return n == 0 ? 1 : n * FactorialRecursive(n - 1);
        }

        static int Fibonacci(int n)
        {
            int last1 = 0;
            int last2 = 1;
            for (int i = 0; i < n-1; i++)
            {
                int thisNumber = last1 + last2;
                last1 = last2;
                last2 = thisNumber;
            }
            return last2;
        }
        static int Factorial(int n)
        {
            int total = 1;
            while (n > 0)
            {
                total *= n;
                n--;
            }
            return total;
        }
    }
}
