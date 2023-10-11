using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2023-2024.
 * Extended by students.
 */

namespace RecursionPlayground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine()); // Nacteme cislo n, pro ktere budeme pocitat jeho faktorial a n-ty prvek Fibonacciho posloupnosti.
            int factorial = Factorial(n); // Prvni zavolani pro vypocet faktorialu, ulozeni do promenne factorial.
            int fibonacci = Fibonacci(n); // Prvni zavolani pro vypocet Fibonacciho posloupnosti, ulozeni do promenne fibonacci.
            Console.WriteLine($"Pro cislo {n} je faktorial {factorial} a {n}. prvek Fibonacciho posloupnosti je {fibonacci}"); // Vypsani vysledku uzivateli.
            String fileSearch = findFile("C:\\Users\\matou\\Desktop", "testFile.txt");
            Console.WriteLine("\ntest: " + fileSearch);
            Console.ReadKey();
        }

        static string findFile(string startingFolderPath, string fileName)
        {
            string[] subfolders = Directory.GetDirectories(startingFolderPath);
            string[] subfiles = Directory.GetFiles(startingFolderPath);
            foreach (string subfolder in subfolders)
            {
                string newReturn = findFile(subfolder, fileName);
                if (newReturn != "") return newReturn;
            }
            foreach (string subfile in subfiles)
            {
                if (subfile.EndsWith("\\" + fileName)) return subfile;
            }
            return "";
        }
        static int Factorial(int n)
        {
            if (n != 1) return n*Factorial(n-1);
            return 1;
        }

        static int Fibonacci(int n)
        {
            if (n == 0 || n == 1) return (n==0) ? 0 : 1;
            return Fibonacci(n-1)+ Fibonacci(n - 2);
        }
    }
}
