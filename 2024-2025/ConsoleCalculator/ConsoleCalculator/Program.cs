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
            Console.WriteLine("\n");
            String instructions = "" +
                "Implemented operations:\n" +
                "   - Multiplication   *,×\t- Modulus          %\n" +
                "   - Division         /,÷\t- Factorials       !\n" +
                "   - Addition         +\t\t- Exponentiation   ^\n" +
                "   - Subtraction      -\n" +
                "Implemented functions:\n" +
                "   - sqrt(x)\t\t- abs(x)\t\t- round(number, decimals)\n" +
                "   - floor(x)\t\t- ceiling(x)\t\t\n" +
                "   - ln(x)\t\t- log(x)\t\t- log(base; number)\n" +
                "   - sin(x)\t\t- arcsin(x)\t\t- csc(x)\n" +
                "   - cos(x)\t\t- arccos(x)\t\t- sec(x)\n" +
                "   - tg(x)\t\t- arctg(x)\t\t- cotg(x)\n" +
                "Saved Variables:\n" +
                "   - ans = [Value of the last answer]\n" +
                "   - pi\t\t\t- e\n" +
                "   - x = 0\t\t- y = 0\t\t\t- z = 0\n" +
                "";
            Console.WriteLine(instructions);
            while (true)
            {
                //Main loop that reads input from user
                Console.WriteLine("\nEnter your math problem:");
                String input = Console.ReadLine();
                String parsedInput = StringUtils.parseInput(input);
                if (input.Contains("="))
                {
                    //Setting a variable
                    String[] split = input.Split('=');
                    Evaluator.setVariable(split[0].Trim(), split[1].Trim());
                    continue;
                }
                String result = StringUtils.finalizeOutput(Evaluator.evaulate(parsedInput, true));
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("ans = "+result);
                Console.ResetColor();
            }
        }
    }
}
