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
        public static String impliedMultiplication(String input)
        {
            input = input.Replace("−", "-");
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
                        || (move == -1 && (neighbor.Equals(')') || neighbor.Equals(']') || neighbor.Equals('-'))))
                    {
                        String insert = !neighbor.Equals('-') ? "×" : "1×";
                        input = input.Insert(index + (move == -1 ? 0 : move), insert);
                    }
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
        public static bool containsSign(String input, String sign)
        {
            if (sign == "*/")
            {
                bool result = input.Contains("*") || input.Contains("/");
                return result;
            }
            return input.Contains(sign);
        }
        public static bool containsAnySign(String input, String[] signs)
        {
            foreach (String sign in signs)
            {
                if (sign == "*/" && (input.Contains("*") || input.Contains("/"))) return true;
                else if (input.Contains(sign)) return true;
            }
            return false;
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
            for (int i = (moveSide > 0 ? 0 : input.Length); (moveSide > 0 ? i < input.Length : i > 0); i += moveSide)
            {
                String sub = (moveSide > 0) ? input.Substring(i) : input.Substring(0, i);
                String finalized = finalize(sub);
                if (finalized != "") return finalized;
            }

            String finalize(String sub)
            {
                bool bracketed = false;
                sub = unifyBrackets(sub).Replace("−", "-");
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
        public static String finalizeOutput(String input)
        {
            input = removeBrackets(input).Replace("−", "-");
            if (Double.TryParse(input, out double num))
            {
                double roundPrecision = Math.Pow(10, 10);
                input = Convert.ToString(Math.Round(num* roundPrecision)/ roundPrecision);
            }
            return input;
        }
        public static String replaceVariables(String input)
        {
            return input;
        }
    }
}
