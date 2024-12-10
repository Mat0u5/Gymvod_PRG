using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursionPlayground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            int factorial = Factorial(n);
            int fibonacci = Fibonacci(n);
            Console.WriteLine($"Pro cislo {n} je faktorial {factorial} a {n}. prvek Fibonacciho posloupnosti je {fibonacci}");
            Console.ReadKey();
        }

        static int Factorial(int n)
        {
            if (n == 1) return n;
            return n * Factorial(n - 1);
        }

        static int Fibonacci(int n)
        {
            if (n ==2) return 1;
            if (n == 1) return 0;
            return Fibonacci(n - 1)+Fibonacci(n-2);
        }
    }
}
