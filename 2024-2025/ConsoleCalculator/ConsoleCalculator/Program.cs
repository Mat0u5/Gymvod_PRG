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
        public static Dictionary<String, String> inputReplacer = new Dictionary<String, String>()
            {
                {"++","+"},
                {"--","+"},
                {"+-","-"},
                {"-+","-"},

                {" ",""},
                {".",","},
                {"÷","/"},
                {"×","*"}
            };
        static void Main(string[] args)
        {
            while (true)
            {
                String input = parseInput(Console.ReadLine());
                Console.WriteLine(input);
            }
        }
        private static String parseInput(string input)
        {
            while (true)
            {
                bool end = true;
                foreach (String key in inputReplacer.Keys)
                {
                    if (input.Contains(key))
                    {
                        input = input.Replace(key, inputReplacer[key]);
                        end = false;
                    }
                }
                if (end) break;
            }
            return impliedMultiplication(unifyBrackets(input));
        }
        static String impliedMultiplication(String input)
        {
            Dictionary<String, int> brackets = new Dictionary<String, int>() { { "(", -1 }, { ")", 1 } };
            foreach (String bracket in brackets.Keys)
            {
                if (input.Contains(bracket))
                {
                    int move = brackets[bracket];
                    int index = input.IndexOf(bracket);
                    input = replaceFirst(input, bracket, bracket.Equals("(") ? "[" : "]");
                    if (index == 0 || index == input.Length - 1) continue;
                    char neighbor = input.ToCharArray()[index + move];
                    if (Int32.TryParse(Convert.ToString(neighbor), out int num) 
                        || (move == 1 && (neighbor.Equals('(') || neighbor.Equals('['))) 
                        || (move == -1 && (neighbor.Equals(')') || neighbor.Equals(']') || neighbor.Equals('-')))) {
                        String insert = !neighbor.Equals('-') ? "×" : "1×";
                        input = input.Insert(index + (move==-1?0:move), insert);
                    }
                }
            }
            if (hasBrackets(input)) return impliedMultiplication(input);
            return unifyBrackets(input);
        }
        static String evaluateBrackets(String input)
        {
            return input;
        }
        static bool hasBrackets(String input)
        {
            return input.Contains("(") || input.Contains(")");
        }
        static String unifyBrackets(String input) //replaces all [,{,},] with ()
        {
            return input.Replace("[", "(").Replace("{", "(").Replace("]", ")").Replace("}", ")");
        }
        static bool containsSign(String editedInput, String sign)
        {
            if (sign == "*/")
            {
                bool result = editedInput.Contains("*") || editedInput.Contains("/");
                return result;
            }
            return editedInput.Contains(sign);
        }
        static String evaluateSign(String input, String sign)
        {
            if (!input.Contains(sign)) return input;
            int index = input.IndexOf(sign);
            if (sign.Equals("*/"))
            {
                index = Math.Min(input.IndexOf("*"),input.IndexOf("/"));
                sign = Convert.ToString(input.ToCharArray()[index]);
            }


            if (input.Contains(sign)) return evaluateSign(input, sign);
            return input;
        }
        static String evaulate(String input)
        {

            input = evaluateBrackets(input);
            String[] signListInOrder = new String[] { "^", "×", "*/", "+", "-" };
            foreach (String sign in signListInOrder)
            {
                input = evaluateSign(input, sign);
            }
            return input;
        }
        public static String replaceFirst(String str, String term, String replace)
        {
            int position = str.IndexOf(term);
            if (position < 0)
            {
                return str;
            }
            str = str.Substring(0, position) + replace + str.Substring(position + term.Length);
            return str;
        }

        static String removeBrackets(String input)//remove brackets on the start and end of string
        {
            input = unifyBrackets(input);
            if (input.StartsWith("(") && input.EndsWith(")"))
            {
                input = input.Replace("(", "").Replace(")", "");
            }
            return input;
        }
        
        static String getNumberOnSideOfString(String input, int moveSide, bool exponentiating)
        {
            for (int i = (moveSide > 0 ? 0 : input.Length); (moveSide > 0 ? i < input.Length : i > 0) ; i+= moveSide)
            {
                String sub = (moveSide > 0) ? input.Substring(i) : input.Substring(0, i);
                String finalized = finalize(sub);
                if (finalized != "") return finalized;
            }

            String finalize(String sub)
            {
                bool bracketed = false;
                String unbracketedSub = removeBrackets(sub);
                if (sub != unbracketedSub) bracketed = true;
                if (double.TryParse(unbracketedSub, out double result))
                {
                    if (unbracketedSub.StartsWith("-") && !bracketed && exponentiating) sub = sub.Substring(1);
                    return sub;
                }
                return "";
            }

            return "";
        }
    }
}
