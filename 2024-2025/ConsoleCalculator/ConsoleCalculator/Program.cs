using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CalcTester.testAll();
            Console.WriteLine("\n\n\n");
            while (true)
            {
                String parsedInput = StringUtils.parseInput(Console.ReadLine());
                Console.WriteLine(StringUtils.finalizeOutput(Evaluator.evaulate(parsedInput, true), true));
            }
        }
    }
}
