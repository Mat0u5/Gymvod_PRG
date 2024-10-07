using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator
{
    internal class CalcTester
    {
        static Dictionary<String, String> problems = new Dictionary<String, String>()
        {
            // Basic Arithmetic
            {"2 + 3", "5"},
            {"10 - 4", "6"},
            {"6 * 7", "42"},
            {"20 / 5", "4"},
    
            // Powers and Roots
            {"5 ^ 3", "125"},
            {"16 ^ 0.5", "4"},
            {"27 ^ (1/3)", "3"},
            {"-2 ^ 2", "-4"},
            {"(-2) ^ 2", "4"},
            {"2 ^ -2", "0,25"},
    
            // Complex Expressions
            {"(3 + 5) * 2", "16"},
            {"(4 - 1) ^ 3", "27"},
            {"(6 * 3) / 2 + 4", "13"},
    
            // Modulus
            //{"17 % 3", "2"},
    
            // Trigonometric Functions (Assuming angles in degrees for stress testing)
            //{"sin(90)", "1"},
            //{"cos(0)", "1"},
            //{"tan(45)", "1"},
            //{"sin(30)", "0.5"},
    
            // Factorials
            //{"5!", "120"},
            //{"7!", "5040"},
    
            // Nested Operations
            {"((2 + 3) * (4 - 1)) / 5", "3"},
            {"((5^2) - (3^2)) + (4 * 2)", "24"},
            {"-(2 + 3)", "-5"},
            {"-(2 - 3)", "1"},
    
            // Logarithmic Expressions (Assuming base 10)
            //{"log(100)", "2"},
            //{"log(1000)", "3"},
    
            // Large Numbers
            {"999999999 + 999999999", "1999999998"},
            {"123456789 * 987654321", "1,21932631112635E17"},//121932631112635269
    
            // Negative Numbers
            {"-3 + 7", "4"},
            {"-10 * -5", "50"},
            {"-15 / 3", "-5"},
    
            // Decimal Arithmetic
            {"3.5 + 2.1", "5.6"},
            {"7.8 * 1.2", "9.36"},
            {"10.5 / 2.5", "4.2"},
    
            // Mixed Operations with Decimals
            {"(2.5 + 3.1) * 1.4", "7.84"},
            {"(9.2 / 2) + 1.3", "5.9"},
    
            // Square Root (non-exact)
            //{"sqrt(2)", "1.414213562"},
            //{"sqrt(50)", "7.071067812"},
    
            // Exponentials (Stress Test for Larger Exponents)
            {"2 ^ 10", "1024"},
            {"9 ^ 5", "59049"}
        };

        public static void testAll()
        {
            foreach (String problem in problems.Keys)
            {
                String expectedSolution = problems[problem].Replace(".",",");
                String actualSolution = StringUtils.finalizeOutput(Evaluator.evaulate(StringUtils.parseInput(problem), false), true);


                if (expectedSolution != actualSolution)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Problem ({problem}) Unsuccessful");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Problem ({problem}) Successful");
                    Console.ResetColor();
                }
            }
        }
    }
}
