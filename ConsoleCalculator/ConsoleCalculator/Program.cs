using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2023-2024.
 * Extended by students.
 */

namespace ConsoleCalculator
{
    internal class Program
    {
        static bool showCalcProcess = true;
        static bool usePejmdas = true;//if false, use pemdas
        static bool useRadians = false;
        static Dictionary<String, String> variables = new Dictionary<String, String>() 
        {
            {"ans","0"},{"pi",Convert.ToString(Math.PI)},{"e",Convert.ToString(Math.E)},{"x","0"},{"y","0"},{"z","0"}
        };
        static String[] functions = new String[] { "sqrt", "abs", "log", "ln", "round", "floor", "ceiling", "divremainder",
            "arcsin", "arccos", "arctg", "sin", "cos", "cotg", "tg" , "sec", "csc"};
        static void Main(string[] args)
        {
            //USING THE PEJMDAS SYSTEM
            //FUNCTIONS USE DEGREES, NOT RADIANS
            String instructions = "" + 
            "Allowed operations:\n" +
            "   - exponentiation   ^\n" +
            "   - multiplication   *,×\n" +
            "   - division         /,÷\n" +
            "   - addition         +\n" +
            "   - subtraction      -\n" +
            "Allowed functions:\n" +
            "   - sqrt(x)\t\t\t- abs(x)\t\t\t- round(number, decimals)\n" +
            "   - floor(x)\t\t\t- ceiling(x)\t\t\t- divremainder(number, divisor)\n" +
            "   - ln(x)\t\t\t- log(x)\t\t\t- log(number; base)\n" +
            "   - sin(x)\t\t\t- arcsin(x)\t\t\t- csc(x)\n" +
            "   - cos(x)\t\t\t- arccos(x)\t\t\t- sec(x)\n" +
            "   - tg(x)\t\t\t- arctg(x)\t\t\t- cotg(x)\n" +
            "Saved Variables:\n" +
            "   - ans = [value of the last answer]\n" +
            "   - pi\n" +
            "   - e\n" +
            "   - x=0\n" +
            "   - y=0\n" +
            "   - z=0\n" +
            "\nOutput the whole calculation process? (y/n)";
            Console.WriteLine(instructions);
            String seeProcess = Console.ReadLine();
            bool skippedQuestion = true;
            if (seeProcess.ToLower() == "y" || seeProcess.ToLower() == "n")
            {
                skippedQuestion = false;
                showCalcProcess = (seeProcess.ToLower() == "y") ? true : false;
                Console.WriteLine("Enter your problem.   example: 5*((1-3)^2)+4\n");
            }
            while (true)
            {
                String input = (skippedQuestion) ? seeProcess : Console.ReadLine();
                skippedQuestion = false;
                //input = juxtaposition(input);
                if (!input.Contains("="))
                {
                    input = parseInput(input);
                    String evaledInput = evaluateString(input, showCalcProcess);
                    variables["ans"] = finalizeOutput(evaledInput, true);

                    String aproxValue = (finalizeOutput(evaledInput, false) != finalizeOutput(evaledInput, true)) ? "     (= "+finalizeOutput(evaledInput, false)+")" : "";
                    Console.WriteLine("ans = " + finalizeOutput(evaledInput, true) + aproxValue+"\n");
                }
                else
                {
                    String varName = input.Split('=')[0].Trim();
                    String varValue = input.Split('=')[1].Trim();
                    Console.WriteLine("Set the value of '" + varName + "' to " + varValue + "\n");
                    if (variables.ContainsKey(varName))
                    {
                        variables[varName] = varValue;
                    }
                }
            }
            Console.ReadKey();
        }
        static String parseInput(String input)
        {
            input = unifyBrackets(input).Replace(".", ",").Replace(" ", "").Replace("÷", "/").Replace("×", "*");
            input = juxtaposition(input);

            foreach (KeyValuePair<String, String> entry in variables)
            {
                String varName = entry.Key;
                String varValue = entry.Value;
                List<Char> bannedCharsBeforeVariable = new List<Char>();
                List<Char> bannedCharsAfterVariable = new List<Char>();
                //to prevent letter e in sec from getting replaced
                foreach (String function in functions)
                {
                    if (!function.Contains(varName)) continue;
                    String[] split = function.Split(new String[] { varName }, function.Length, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (i == 0)
                        {
                            bannedCharsBeforeVariable.Add(split[i].ToCharArray()[split[i].ToCharArray().Length - 1]);
                            continue;
                        }
                        if (i == split.Length - 1)
                        {
                            bannedCharsAfterVariable.Add(split[i].ToCharArray()[0]);
                            continue;
                        }
                        bannedCharsBeforeVariable.Add(split[i].ToCharArray()[split[i].ToCharArray().Length - 1]);
                        bannedCharsAfterVariable.Add(split[i].ToCharArray()[0]);
                    }
                }

                input = input.Replace(varName, "_" + varValue + "_");
                foreach (Char bannedChar in bannedCharsBeforeVariable) input = input.Replace(bannedChar + "_" + varValue + "_", bannedChar+varName);
                foreach (Char bannedChar in bannedCharsAfterVariable) input = input.Replace("_" + varValue + "_"+ bannedChar, varName+ bannedChar);
                input = input.Replace("_" + varValue + "_", "("+varValue+")");

            }
            return input;
        }
        static String evaluateFunction(String input)//is run from evaluateBrackets()  !INPUT HAS TO BE A SINGLE FUNCTION ex. sqrt(2)
        {
            foreach (String function in functions)
            {
                if (!input.StartsWith(function + "(") || !input.EndsWith(")")) continue;

                String inFunction = input.Substring((function + "(").Length);
                inFunction = removeBrackets(inFunction.Substring(0, inFunction.Length - 1));

                double result = 0;
                bool multipleArgs = inFunction.Contains(";");
                String[] args = (multipleArgs) ? inFunction.Split(';') : new String[] {inFunction};
                if (!double.TryParse(args[0], out double firstArg)) continue;
                double degreeToRad = (useRadians) ? 1 : Math.PI / 180.0;
                double radToDegree = (useRadians) ? 1 : 180 / Math.PI;

                if (function == "sqrt") return Convert.ToString(Math.Sqrt(firstArg));
                if (function == "abs") return Convert.ToString(Math.Abs(firstArg));
                if (function == "floor") return Convert.ToString(Math.Floor(firstArg));
                if (function == "ceiling") return Convert.ToString(Math.Ceiling(firstArg));
                if (function == "ln") return Convert.ToString(Math.Log(firstArg));
                if (function == "log" && !multipleArgs) return Convert.ToString(Math.Log10(firstArg));
                if (function == "sin") return Convert.ToString(Math.Sin(firstArg* degreeToRad));
                if (function == "cos") return Convert.ToString(Math.Cos(firstArg * degreeToRad));
                if (function == "csc") return Convert.ToString(1 / Math.Sin(firstArg * degreeToRad));
                if (function == "sec") return Convert.ToString(1 / Math.Cos(firstArg * degreeToRad));
                if (function == "tg") return Convert.ToString(Math.Tan(firstArg * degreeToRad));
                if (function == "cotg") return Convert.ToString(1 / Math.Tan(firstArg * degreeToRad));
                if (function == "arcsin") return Convert.ToString(Math.Asin(firstArg) * radToDegree);
                if (function == "arccos") return Convert.ToString(Math.Acos(firstArg) * radToDegree);
                if (function == "arctg") return Convert.ToString(Math.Atan(firstArg) * radToDegree);

                if (args.Length < 2) continue;
                if (!double.TryParse(args[1], out double secongArg)) continue;

                if (function == "log") return Convert.ToString(Math.Log(firstArg, secongArg));
                if (function == "round" && int.TryParse(Convert.ToString(secongArg), out int parsedSecond)) return Convert.ToString(Math.Round(firstArg, parsedSecond));
                if (function == "divremainder") return Convert.ToString(firstArg % secongArg);

                input = Convert.ToString(result);
            }
            return input;
        }
        static String evaluateString(String input, bool sendOutput)
        {
            input = evaluateBrackets(input, sendOutput);//contains juxtaposition() and evaluateFunction()
            String[] order = new String[] { "^", "×", "*/", "+", "-" };
            foreach (String sign in order)  input = evaluateWithSign(input, sign, sendOutput);
            return input;
        }
        static String juxtaposition(String input)
        {
            foreach (String function in functions) input = input.Replace(function, "(" + function.ToUpper());
            foreach (String variable in variables.Keys) input = input.Replace(variable, "(" + variable.ToUpper() + ")");



            if (input.Contains("("))
            {
                String[] split = input.Split('(');
                for (int i = 0; i < split.Length; i++)
                {
                    String beforeBracket = split[i];
                    if ((getNumberOnSideOfString(beforeBracket, false, false) != "" || beforeBracket.EndsWith(")") || beforeBracket.EndsWith("-")) && i != split.Length - 1)
                    {
                        if (beforeBracket.EndsWith("-")) split[i] += 1;
                        if (usePejmdas) split[i] += "×";
                        else split[i] += "*";

                    }
                }
                String newInput = "";
                foreach (String str in split)
                {
                    newInput += str + "(";
                }
                input = newInput.Substring(0, newInput.Length - 1);
            }
            if (input.Contains(")"))
            {
                String[] split = input.Split(')');
                for (int i = 0; i < split.Length; i++)
                {
                    String afterBracket = split[i];
                    if ((getNumberOnSideOfString(afterBracket, true, false) != "" || afterBracket.StartsWith("(")) && !afterBracket.StartsWith("+") && !afterBracket.StartsWith("-") && i != 0)
                    {
                        if (usePejmdas) split[i] = "×" + split[i];
                        else split[i] = "*" + split[i];

                    }
                }
                String newInput = "";
                foreach (String str in split)
                {
                    newInput += str + ")";
                }
                input = newInput.Substring(0, newInput.Length - 1);
            }
            foreach (String variable in variables.Keys) input = input.Replace("(" + variable.ToUpper() + ")", variable);
            foreach (String function in functions) input = input.Replace("(" + function.ToUpper(), function);
            return input;
        }
        static String evaluateBrackets(String input, bool sendOutput)
        {
            input = juxtaposition(input);
            int maxLoop = 100;
            int currentLoop = 0;
            int lastEvaledBracket = 0;
            while (input.Contains("(") && currentLoop < maxLoop)
            {
                int bracketStartIndex = input.IndexOf("(", lastEvaledBracket);
                String bracketStr = "";

                for (int i = 1; i <= (input.Length - bracketStartIndex); i++)
                {
                    //String keeps getting longer and longer until it has the same number of '(' as ')' 
                    //this is done so that composite brackets work ((x)+(y))
                    String maybeBracketStr = input.Substring(bracketStartIndex, i);
                    if (maybeBracketStr.Count(x => x == '(') == maybeBracketStr.Count(x => x == ')'))
                    {
                        bracketStr = maybeBracketStr;
                        break;
                    }
                }
                String inBracketStr = bracketStr.Substring(1, bracketStr.Length - 2);//contents of the bracket
                String beforeBracket = input.Substring(0, bracketStartIndex);
                String afterBracket = input.Substring(bracketStartIndex + bracketStr.Length);
                input = beforeBracket + "[" + evaluateString(inBracketStr, false) + "]" + afterBracket;
                input = removeDoubledSigns(input);
                lastEvaledBracket = beforeBracket.Length;
                foreach (String function in functions)
                {
                    if (beforeBracket.EndsWith(function))
                    {
                        String functionStr = function + "(" + evaluateString(inBracketStr, false) + ")";
                        String evaluatedFunction = evaluateFunction(functionStr);
                        input = beforeBracket.Substring(0, beforeBracket.Length - function.Length) + "[" + evaluatedFunction + "]" + afterBracket;
                        break;
                    }
                }
                if (sendOutput) Console.WriteLine("=> " + finalizeOutput(input, true));
                currentLoop++;
            }
            return input;
        }
        static String evaluateWithSign(String input, String sign, bool sendOutput)
        {
            int maxLoop = 100;
            int currentLoop = 0;
            int startAtIndex = 0;
            bool finished = false;
            while (canBeSimplified(input, sign) && currentLoop < maxLoop)
            {
                currentLoop++;

                int index = input.IndexOf(sign, startAtIndex);
                if (sign == "*/")//find which one is first to go from left to right
                {
                    int index1 = input.IndexOf("*", startAtIndex);
                    int index2 = input.IndexOf("/", startAtIndex);
                    index1 = (index1 == -1) ? int.MaxValue : index1;//to prevent -1 from "being before" the other sign
                    index2 = (index2 == -1) ? int.MaxValue : index2;
                    sign = (index1 < index2) ? "*" : "/";
                    index = (index1 < index2) ? index1 : index2;
                }

                if (index == -1 || index == int.MaxValue)
                {
                    finished = true;
                    break;
                }
                String start = input.Substring(0, index);
                String end = input.Substring(index + 1);
                String startNumStrBracketed = getNumberOnSideOfString(start, false, (sign == "^"));
                String endNumStrBracketed = getNumberOnSideOfString(end, true, false);

                String startNumStr = removeBrackets(startNumStrBracketed);
                String endNumStr = removeBrackets(endNumStrBracketed);
                double startNum = (startNumStr == "") ? double.NaN : double.Parse(startNumStr);
                double endNum = (endNumStr == "") ? double.NaN : double.Parse(endNumStr);
                if (sign == "-" && double.IsNaN(startNum) && endNumStrBracketed.StartsWith("(")) startNum = 0;
                if (double.IsNaN(startNum) || double.IsNaN(endNum))
                {
                    startAtIndex = index + 1;
                    //Console.WriteLine("NaN=>" + input);
                }
                else
                {
                    double result = double.NaN;
                    if (sign == "+") result = startNum + endNum;
                    if (sign == "-") result = startNum - endNum;
                    if (sign == "×") result = startNum * endNum;
                    if (sign == "*") result = startNum * endNum;
                    if (sign == "/") result = startNum / endNum;
                    if (sign == "^") result = Math.Pow(startNum, endNum);
                    String strResult = Convert.ToString(result);
                    if (!strResult.StartsWith("-")) strResult = "+" + strResult;//to prevent situations when the result doesnt have a sign in front when it should
                    input = input.Substring(0, index - startNumStrBracketed.Length) + strResult + input.Substring(index + endNumStrBracketed.Length + 1);
                    input = removeDoubledSigns(input);
                    startAtIndex = 0;

                    if (sendOutput) Console.WriteLine("=> " + finalizeOutput(input, true));
                }
                if (sign == "*" || sign == "/") sign = "*/";//to reset the sign for the next round
            }
            return input;
        }
        static bool canBeSimplified(String editedInput, String sign)
        {
            if (sign == "*/")
            {
                bool result = editedInput.Contains("*") || editedInput.Contains("/");
                return result;
            }
            return editedInput.Contains(sign);
        }
        static String getNumberOnSideOfString(String input, bool sideLeft, bool exponentiating)//converts "11*2+32" --> 11
        {

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
            if (sideLeft)
            { //getStartingNumber

                for (int i = input.Length; i > 0; i--)
                {
                    String sub = input.Substring(0, i);
                    if (finalize(sub) != "") return finalize(sub);
                }
            }
            else
            {//getEndingNumber
                for (int i = 0; i < input.Length; i++)
                {
                    String sub = input.Substring(i);
                    if (finalize(sub) != "") return finalize(sub);
                }
            }

            return "";
        }
        static String removeBrackets(String input)
        {
            input = unifyBrackets(input);
            if (input.StartsWith("(") && input.EndsWith(")"))
            {
                input = input.Replace("(", "").Replace(")", "");
            }
            return input;
        }
        static String unifyBrackets(String input)//replace all [,{,},] with ()
        {
            return input.Replace("[", "(").Replace("{", "(").Replace("]", ")").Replace("}", ")");
        }
        static String removeDoubledSigns(String input)
        {
            return input.Replace("++", "+").Replace("--", "+").Replace("-+", "-").Replace("+-", "-");
        }
        static String finalizeOutput(String input, bool showIrrationals)
        {
            input = removeBrackets(unifyBrackets(removeDoubledSigns(input)));
            if (!double.TryParse(input, out double ans)) return input;
            if (!showIrrationals) return Convert.ToString(Math.Round(ans, 10));
            //currently only replacing irrationals if the answer is a number
            Dictionary<double, String> replaceIrrationals = new Dictionary<double, String>();
            replaceIrrationals.Add(Math.PI, "pi");
            replaceIrrationals.Add(Math.E, "e");
            replaceIrrationals.Add(Math.Sqrt(2), "sqrt(2)");
            replaceIrrationals.Add(Math.Sqrt(3), "sqrt(3)");
            replaceIrrationals.Add(0, "0");//TODO THIS IS SO UGLY       to prevent situation like 1,22460635382238E-16
            foreach (KeyValuePair<double, String> entry in replaceIrrationals)
            {
                double value = entry.Key;
                String valueStr = entry.Value;
                double tryMultiply = double.NaN;
                double tryDivide = double.NaN;
                if (value != 0) tryMultiply = Math.Round(ans / value, 12);
                if (value != 0) tryDivide = Math.Round(value / ans, 12);
                if (value == 0 && Math.Round(ans, 15) == 0)
                {
                    input = "0";
                }
                if (int.TryParse(Convert.ToString(tryMultiply), out int multiplyBy) && multiplyBy != 0)
                {
                    input = ((multiplyBy == 1) ? "" : Convert.ToString(multiplyBy)) + valueStr;
                }
                if (int.TryParse(Convert.ToString(tryDivide), out int divideBy) && divideBy != 0)
                {
                    input = valueStr + ((divideBy == 1) ? "" : "/" + Convert.ToString(divideBy));
                }
            }
            if (double.TryParse(input, out double finalAns)) return Convert.ToString(Math.Round(finalAns, 10));
            return input;
        }
    }
}
