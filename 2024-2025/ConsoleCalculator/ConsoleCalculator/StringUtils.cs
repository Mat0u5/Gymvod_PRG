using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator
{
    internal class StringUtils
    {
        public static int[] getInnerMostBracketIndeces(String input)
        {
            int lastOpenBracketIndex = -1;
            for (int currentIndex = 0; currentIndex < input.Length; currentIndex++)
            {
                if (input[currentIndex] == '(') lastOpenBracketIndex = currentIndex;
                if (input[currentIndex] == ')') return new int[] { lastOpenBracketIndex, currentIndex - lastOpenBracketIndex+1 };
            }
            return new int[] { -1,-1 };
        }
        public static String parseInput(string input)
        {
            Dictionary<String, String> inputReplacer = new Dictionary<String, String>()
            {
                {"++","+"}, {"--","+"}, {"+-","-"}, {"-+","-"},
                {" ",""}, {".",","},
                {"÷","/"}, {"×","*"}
            };
            
            while (containsAnythingInArray(input, inputReplacer.Keys.ToArray()))
            {
                //Replaces all keys from inputReplacer to their values.
                foreach (String key in inputReplacer.Keys)
                {
                    if (input.Contains(key)) input = input.Replace(key, inputReplacer[key]);
                }
            }
            return impliedMultiplication(replaceVariables(unifyBrackets(input)));
        }
        public static String impliedMultiplication(String input)
        {
            // This function insures that implied multiplication is handled correctly. Ex. 2(3-1) -> 2×(3-1)
            input = input.Replace("−", "-");
            //Dictionary with brackets and directions (1 or -1) that point to where the impl. mult. should be.
            Dictionary<String, int> brackets = new Dictionary<String, int>() { { "(", -1 }, { ")", 1 } };
            foreach (String bracket in brackets.Keys)
            {
                if (!input.Contains(bracket)) continue;
                int move = brackets[bracket];
                int index = input.IndexOf(bracket);
                input = replaceFirst(input, bracket, bracket.Equals("(") ? "[" : "]");
                //If is in bounds
                if (index == 0 || index == input.Length - 1) continue;
                char neighbor = input.ToCharArray()[index + move];
                //If neighbor is a number, or a different bracket, then add implied multiplication
                if (Int32.TryParse(Convert.ToString(neighbor), out int num)
                    || (move == 1 && (neighbor.Equals('(') || neighbor.Equals('[')))
                    || (move == -1 && (neighbor.Equals(')') || neighbor.Equals(']') || neighbor.Equals('-'))))
                {
                    String insert = !neighbor.Equals('-') ? "×" : "1×";
                    input = input.Insert(index + (move == -1 ? 0 : move), insert);
                }
            }
            if (hasBrackets(input)) return impliedMultiplication(input);
            return unifyBrackets(input);
        }
        public static bool hasBrackets(String input)
        {
            return input.Contains("(") || input.Contains(")");
        }
        public static String unifyBrackets(String input)
        {
            return input.Replace("[", "(").Replace("{", "(").Replace("]", ")").Replace("}", ")");
        }
        public static bool containsAnythingInArray(String input, String[] array)
        {
            foreach (String str in array)
            {
                if (str == "*/" && (input.Contains("*") || input.Contains("/"))) return true;
                else if (input.Contains(str)) return true;
            }
            return false;
        }
        public static String replaceFirst(String str, String term, String replace)
        {
            int position = str.IndexOf(term);
            if (position < 0) return str;
            str = str.Substring(0, position) + replace + str.Substring(position + term.Length);
            return str;
        }
        public static String removeBrackets(String input)//remove brackets on the start and end of string
        {
            if (input.StartsWith("(") && input.EndsWith(")"))
            {
                input = input.Substring(1).Substring(0,input.Length-2);
            }
            return input;
        }
        public static String getNumberOnSideOfString(String input, int moveSide, bool exponentiating)
        {
            // Variable for loop that either moves from 0->input.Length or from input.Length->0
            for (int i = (moveSide > 0 ? 0 : input.Length); (moveSide > 0 ? i < input.Length : i > 0); i += moveSide)
            {
                String sub = (moveSide > 0) ? input.Substring(i) : input.Substring(0, i);
                String finalized = finalize(sub);
                if (finalized != "") return finalized;
            }

            String finalize(String sub)
            {
                // Format the string
                bool bracketed = false;
                sub = unifyBrackets(sub).Replace("−", "-");
                String unbracketedSub = removeBrackets(sub);
                if (sub != unbracketedSub) bracketed = true;
                // Check if it's a valid double, if so, return it
                if (double.TryParse(unbracketedSub, out double result))
                {
                    if (unbracketedSub.StartsWith("-") && !bracketed && exponentiating) sub = sub.Substring(1);
                    return sub;
                }
                return "";
            }

            return "";
        }
        public static String finalizeOutput(String input)
        {
            input = removeBrackets(input).Replace("−", "-");
            //Round to 10 decimal places
            if (Double.TryParse(input, out double num))
            {
                double roundPrecision = Math.Pow(10, 10);
                input = Convert.ToString(Math.Round(num* roundPrecision)/ roundPrecision);
            }
            Evaluator.variables["ans"] = input;
            return input;
        }
        public static String replaceVariables(String input)
        {
            //Replace all function occurences with upper case variants, so that they don't get replaced when replacing variables.
            foreach (String function in Evaluator.functions)
            {
                input = input.Replace(function, function.ToUpper());
            }
            //Replace variables
            foreach (String variable in Evaluator.variables.Keys)
            {
                input = input.Replace(variable, "("+Evaluator.variables[variable]+")");
            }
            //Undo the upper case functions
            return input.ToLower();
        }
    }
}
