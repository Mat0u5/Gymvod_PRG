﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator
{
    internal class Evaluator
    {
        static Dictionary<String, String> variables = new Dictionary<String, String>()
        {
            {"ans","0"},{"pi",Convert.ToString(Math.PI)},{"e",Convert.ToString(Math.E)},{"x","0"},{"y","0"},{"z","0"}
        };
        static String[] functions = new String[] { "sqrt", "abs", "log", "ln", "round", "floor", "ceiling", "divremainder",
            "arcsin", "arccos", "arctg", "sin", "cos", "cotg", "tg" , "sec", "csc"};
        static double evaluateFunction(String function, String inFunction)
        {
            bool hasMultipleArgs = inFunction.Contains(";");
            String[] args = (hasMultipleArgs) ? inFunction.Split(';') : new String[] { inFunction };
            //check if the first argument is a number
            if (!double.TryParse(args[0], out double firstArg)) return double.NaN;
            bool useRadians = false;
            double degreeToRad = (useRadians) ? 1 : Math.PI / 180.0;
            double radToDegree = (useRadians) ? 1 : 180 / Math.PI;

            if (function == "sqrt") return Math.Sqrt(firstArg);
            if (function == "abs") return Math.Abs(firstArg);
            if (function == "floor") return Math.Floor(firstArg);
            if (function == "ceiling") return Math.Ceiling(firstArg);
            if (function == "ln") return Math.Log(firstArg);
            if (function == "log" && !hasMultipleArgs) return Math.Log10(firstArg);
            if (function == "sin") return Math.Sin(firstArg * degreeToRad);
            if (function == "cos") return Math.Cos(firstArg * degreeToRad);
            if (function == "csc") return 1 / Math.Sin(firstArg * degreeToRad);
            if (function == "sec") return 1 / Math.Cos(firstArg * degreeToRad);
            if (function == "tg") return Math.Tan(firstArg * degreeToRad);
            if (function == "cotg") return 1 / Math.Tan(firstArg * degreeToRad);
            if (function == "arcsin") return Math.Asin(firstArg) * radToDegree;
            if (function == "arccos") return Math.Acos(firstArg) * radToDegree;
            if (function == "arctg") return Math.Atan(firstArg) * radToDegree;

            //chest if the function has multiple args, and if the second arg is a number
            if (args.Length < 2) return double.NaN;
            if (!double.TryParse(args[1], out double secongArg)) return double.NaN;

            if (function == "log") return Math.Log(firstArg, secongArg);
            if (function == "round" && int.TryParse(Convert.ToString(secongArg), out int parsedSecond)) return Math.Round(firstArg, parsedSecond);
            if (function == "divremainder") return firstArg % secongArg;

            return double.NaN;
        }
        public static String evaluateBrackets(String input, bool sendOutput)
        {
            while (input.Contains("("))
            {
                input = StringUtils.impliedMultiplication(input);
                int[] pos = StringUtils.getInnerMostBracketIndeces(input);
                if (pos[0] <= -1 || pos[1] <= -1) return input;
                String beforeBracket = input.Substring(0, pos[0]);
                String evaledBracket = evaulate(input.Substring(pos[0] + 1, pos[1] - 2), false);
                String afterBracket = input.Substring(Math.Min(pos[0] + pos[1], input.Length));
                foreach(String function in functions)
                {
                    if (beforeBracket.EndsWith(function) && !evaledBracket.Contains("("))
                    {
                        beforeBracket = beforeBracket.Substring(0, beforeBracket.Length - function.Length);
                        evaledBracket = Convert.ToString(evaluateFunction(function, evaledBracket.Replace("−", "-")));
                    }
                }
                input = beforeBracket + evaledBracket + afterBracket;
                if (afterBracket.StartsWith("^")) return input = beforeBracket + "["+ evaledBracket+"]" + afterBracket;
                if (sendOutput) Console.WriteLine("=> " + StringUtils.finalizeOutput(input));
            }
            return input;
        }
        public static String evaluateSign(String input, String sign, bool sendOutput)
        {
            if (!input.Contains(sign) && !sign.Equals("*/")) return input;
            int index = input.IndexOf(sign);
            if (sign.Equals("*/"))
            {
                int invalidIndex = input.Length + 1;
                index = Math.Min(input.IndexOf("/") == -1 ? invalidIndex : input.IndexOf("/"), input.IndexOf("*") == -1 ? invalidIndex : input.IndexOf("*"));
                if (index == -1 || index == invalidIndex) return input;
                sign = Convert.ToString(input.ToCharArray()[index]);
            }
            String leftSide = input.Substring(0, index);
            String rightSide = input.Substring(index + 1);
            String leftNumStrBracketed = StringUtils.getNumberOnSideOfString(leftSide, 1, sign.Equals("^"));
            String rightNumStrBracketed = StringUtils.getNumberOnSideOfString(rightSide, -1, false);
            String leftNumStr = StringUtils.removeBrackets(leftNumStrBracketed);
            String rightNumStr = StringUtils.removeBrackets(rightNumStrBracketed);
            String strResult = "";
            bool leftValid = Double.TryParse(leftNumStr, out double leftNum);
            bool rightValid = Double.TryParse(rightNumStr, out double rightNum);
            if (!leftValid || (!rightValid && !sign.Equals("!")))
            {
                if (!leftValid && rightValid && sign.Equals("+")) strResult = rightNumStr;
                else if (!leftValid && rightValid && sign.Equals("-")) strResult = "−" + rightNumStr;
                else return throwError($"SignError at index {index}, found {leftNumStrBracketed} and {rightNumStrBracketed}");
            }
            else
            {
                double result = double.NaN;
                if (sign.Equals("!")) result = Enumerable.Range(1, (int )Math.Round(leftNum)).Aggregate(1, (p, item) => p * item); //From Stack Overflow
                if (sign.Equals("%")) result = leftNum % rightNum;
                if (sign.Equals("+")) result = leftNum + rightNum;
                if (sign.Equals("-")) result = leftNum - rightNum;
                if (sign.Equals("/")) result = leftNum / rightNum;
                if (sign.Equals("*") || sign.Equals("×")) result = leftNum * rightNum;
                if (sign.Equals("^")) result = Math.Pow(leftNum, rightNum);
                strResult = Convert.ToString(result);
                if (!strResult.StartsWith("-")) strResult = "+" + strResult;
                if (result == double.NaN) return throwError($"NaN from using '{sign}' on {leftNumStrBracketed} and {rightNumStrBracketed}");
            }

            input = leftSide.Substring(0, leftSide.Length- leftNumStrBracketed.Length) + strResult + rightSide.Substring(rightNumStrBracketed.Length);
            if (sendOutput) Console.WriteLine("=> " + StringUtils.finalizeOutput(input));
            return input.Contains(sign) ? evaluateSign(input, sign, sendOutput) : input;

            String throwError(String error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(error);
                Console.ResetColor();
                input = leftSide += rightSide;
                return input.Contains(sign) ? evaluateSign(input, sign, sendOutput) : input;
            }
        }
        public static String evaulate(String input, bool sendOutput)
        {
            input = evaluateBrackets(input, sendOutput);
            String[] signListInOrder = new String[] { "!", "^", "×", "*/", "%", "+", "-" };
            while (StringUtils.containsAnySign(input, signListInOrder))
            {
                foreach (String sign in signListInOrder) input = evaluateSign(input, sign, sendOutput);
            }
            return input;
        }
    }
}
