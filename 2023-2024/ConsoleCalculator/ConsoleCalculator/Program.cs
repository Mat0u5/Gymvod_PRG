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
        static bool useRadians = false;// if false, use degrees
        static Dictionary<String, String> variables = new Dictionary<String, String>() 
        {
            {"ans","0"},{"pi",Convert.ToString(Math.PI)},{"e",Convert.ToString(Math.E)},{"x","0"},{"y","0"},{"z","0"}
            //to add new variables, just add them here
        };
        static String[] functions = new String[] { "sqrt", "abs", "log", "ln", "round", "floor", "ceiling", "divremainder",
            "arcsin", "arccos", "arctg", "sin", "cos", "cotg", "tg" , "sec", "csc"};
        //to add new functions, add them here and then add them to evaluateFunction()

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
                //whether you want to see the calculation process or not
                skippedQuestion = false;
                showCalcProcess = (seeProcess.ToLower() == "y") ? true : false;
                Console.WriteLine("Enter your problem.   example: 5*((1-3)^2)+4\n");
            }
            while (true)
            {
                String input = (skippedQuestion) ? seeProcess : Console.ReadLine();
                skippedQuestion = false;
                if (!input.Contains("="))
                {
                    input = parseInput(input);
                    String evaledInput = evaluateString(input, showCalcProcess);

                    //calculate the aproximate value when answer has irrationals
                    String aproxValue = (finalizeOutput(evaledInput, false) != finalizeOutput(evaledInput, true)) ? "     (= "+finalizeOutput(evaledInput, false)+")" : "";
                    Console.WriteLine("ans = " + finalizeOutput(evaledInput, true) + aproxValue+"\n");
                    //set value of ans to the answer
                    variables["ans"] = finalizeOutput(evaledInput, true);
                }
                else
                {
                    //set value of a variable
                    String varName = input.Split('=')[0].Trim();
                    String varValue = input.Split('=')[1].Trim();
                    Console.WriteLine("Set the value of '" + varName + "' to " + varValue + "\n");
                    if (variables.ContainsKey(varName))
                    {
                        variables[varName] = varValue;
                    }
                }
            }
        }
        static String parseInput(String input)// adds implied multiplication, replaces variables with their number counterparts
        {
            input = unifyBrackets(input).Replace(".", ",").Replace(" ", "").Replace("÷", "/").Replace("×", "*");
            input = impliedMultiplication(input);

            foreach (KeyValuePair<String, String> entry in variables)//replace variable names with their values
            {
                String varName = entry.Key;
                String varValue = entry.Value;
                List<Char> bannedCharsBeforeVariable = new List<Char>();
                List<Char> bannedCharsAfterVariable = new List<Char>();
                //finds any functions that contain the variable name and prevents then from being replaces 
                //ex. -  prevents the constant e in sec() from getting replaced
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
                //replace all variables with their values
                input = input.Replace(varName, "(" + varValue + ")");
                //if there are banned characters before or after the variable, change it back (I KNOW, ITS NOT VERY ELEGANT)
                foreach (Char bannedChar in bannedCharsBeforeVariable) input = input.Replace(bannedChar + "(" + varValue + ")", bannedChar + varName);
                foreach (Char bannedChar in bannedCharsAfterVariable) input = input.Replace("(" + varValue + ")" + bannedChar, varName + bannedChar);

            }
            return input;
        }
        static String evaluateFunction(String input)//evaluates a SINGLE function  ex. sqrt(2)
        {
            foreach (String function in functions)
            {
                //only template allowed is:   function(arg0;arg1;...)
                if (!input.StartsWith(function + "(") || !input.EndsWith(")")) continue;

                String inFunction = input.Substring((function + "(").Length);
                inFunction = removeBrackets(inFunction.Substring(0, inFunction.Length - 1));

                double result = 0;
                bool hasMultipleArgs = inFunction.Contains(";");
                String[] args = (hasMultipleArgs) ? inFunction.Split(';') : new String[] {inFunction};
                //check if the first argument is a number
                if (!double.TryParse(removeBrackets(args[0]), out double firstArg)) continue;
                double degreeToRad = (useRadians) ? 1 : Math.PI / 180.0;
                double radToDegree = (useRadians) ? 1 : 180 / Math.PI;

                if (function == "sqrt") return Convert.ToString(Math.Sqrt(firstArg));
                if (function == "abs") return Convert.ToString(Math.Abs(firstArg));
                if (function == "floor") return Convert.ToString(Math.Floor(firstArg));
                if (function == "ceiling") return Convert.ToString(Math.Ceiling(firstArg));
                if (function == "ln") return Convert.ToString(Math.Log(firstArg));
                if (function == "log" && !hasMultipleArgs) return Convert.ToString(Math.Log10(firstArg));
                if (function == "sin") return Convert.ToString(Math.Sin(firstArg* degreeToRad));
                if (function == "cos") return Convert.ToString(Math.Cos(firstArg * degreeToRad));
                if (function == "csc") return Convert.ToString(1 / Math.Sin(firstArg * degreeToRad));
                if (function == "sec") return Convert.ToString(1 / Math.Cos(firstArg * degreeToRad));
                if (function == "tg") return Convert.ToString(Math.Tan(firstArg * degreeToRad));
                if (function == "cotg") return Convert.ToString(1 / Math.Tan(firstArg * degreeToRad));
                if (function == "arcsin") return Convert.ToString(Math.Asin(firstArg) * radToDegree);
                if (function == "arccos") return Convert.ToString(Math.Acos(firstArg) * radToDegree);
                if (function == "arctg") return Convert.ToString(Math.Atan(firstArg) * radToDegree);

                //chest if the function has multiple args, and if the second arg is a number
                if (args.Length < 2) continue;
                if (!double.TryParse(removeBrackets(args[1]), out double secongArg)) continue;

                if (function == "log") return Convert.ToString(Math.Log(firstArg, secongArg));
                if (function == "round" && int.TryParse(Convert.ToString(secongArg), out int parsedSecond)) return Convert.ToString(Math.Round(firstArg, parsedSecond));
                if (function == "divremainder") return Convert.ToString(firstArg % secongArg);

                input = Convert.ToString(result);
            }
            return input;
        }
        static String evaluateString(String input, bool sendOutput)//evaluates brackets, functions, then signs
        {
            input = evaluateBrackets(input, sendOutput);
            String[] signListInOrder = new String[] { "^", "×", "*/", "+", "-" };
            //"×" is used for implied multiplication instead of "*", and has higher priority
            foreach (String sign in signListInOrder)  input = evaluateSign(input, sign, sendOutput);
            return input;
        }
        static String impliedMultiplication(String input)//   2(3+1)  -> 2×(3+1)
        {
            //add brackets next to variables and functions, so that implied multiplication works with them
            // 2pi -> 2(pi)
            foreach (String function in functions) input = input.Replace(function, "(" + function.ToUpper());
            foreach (String variable in variables.Keys) input = input.Replace(variable, "(" + variable.ToUpper() + ")");

            if (input.Contains("("))
            {
                //implied multiplication before (
                // 5(pi) -> 5×(pi)
                String[] split = input.Split('(');
                for (int i = 0; i < split.Length; i++)
                {
                    //get string before (
                    String beforeBracket = split[i];
                    if ((getNumberOnSideOfString(beforeBracket, false, false) != "" || beforeBracket.EndsWith(")") || beforeBracket.EndsWith("-")) && i != split.Length - 1)
                    {
                        if (beforeBracket.EndsWith("-")) split[i] += 1;
                        // if usePejmdas, we add "×", else add "*"
                        // "×" has higher priority than "*"
                        if (usePejmdas) split[i] += "×";
                        else split[i] += "*";

                    }
                }
                String newInput = string.Join("(", split);
                input = newInput;
            }
            if (input.Contains(")"))
            {
                //implied multiplication after )
                // (pi)5 ->   (pi)×5
                String[] split = input.Split(')');
                for (int i = 0; i < split.Length; i++)
                {
                    //get string after )
                    String afterBracket = split[i];
                    if ((getNumberOnSideOfString(afterBracket, true, false) != "" || afterBracket.StartsWith("(")) && !afterBracket.StartsWith("+") && !afterBracket.StartsWith("-") && i != 0)
                    {
                        // if usePejmdas, we add "×", else add "*"
                        // "×" has higher priority than "*"
                        if (usePejmdas) split[i] = "×" + split[i];
                        else split[i] = "*" + split[i];
                           
                    }
                }
                String newInput = string.Join(")", split);
                input = newInput;
            }
            //remove brackets from around the variables and functions
            // 2×(pi)  -> 2×pi
            foreach (String variable in variables.Keys) input = input.Replace("(" + variable.ToUpper() + ")", variable);
            foreach (String function in functions) input = input.Replace("(" + function.ToUpper(), function);
            return input;
        }
        static String evaluateBrackets(String input, bool sendOutput)
        {
            input = impliedMultiplication(input);
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
                    //when the number of '(' matches number of ')', the string gets saved to bracketStr
                    String maybeBracketStr = input.Substring(bracketStartIndex, i);
                    if (maybeBracketStr.Count(x => x == '(') == maybeBracketStr.Count(x => x == ')'))
                    {
                        bracketStr = maybeBracketStr;
                        break;
                    }
                }
                String inBracketStr = bracketStr.Substring(1, bracketStr.Length - 2);//contents of the bracket
                String beforeBracket = input.Substring(0, bracketStartIndex);//string before the bracket
                String afterBracket = input.Substring(bracketStartIndex + bracketStr.Length);//string after the bracket
                String evaluatedBracket = evaluateString(inBracketStr, false);
                //put the evaluated bracket into the input
                //the reason i add [] is so that i know that i've already evaluated that bracket (prevent an infinite loop)
                input = beforeBracket + "[" + evaluatedBracket + "]" + afterBracket;
                input = removeDoubledSigns(input);
                lastEvaledBracket = beforeBracket.Length;
                //check if there is a function in front of the bracket
                foreach (String function in functions)
                {
                    if (!beforeBracket.EndsWith(function)) continue;

                    String functionStr = function + "(" + evaluateString(inBracketStr, false) + ")";
                    //evaluate the function and save to input
                    String evaluatedFunction = evaluateFunction(functionStr);
                    input = beforeBracket.Substring(0, beforeBracket.Length - function.Length) + "[" + evaluatedFunction + "]" + afterBracket;
                    break;
                }
                //if sendOutput, output the calculation process
                if (sendOutput) Console.WriteLine("=> " + finalizeOutput(input, true));
                currentLoop++;
            }
            return input;
        }
        static String evaluateSign(String input, String sign, bool sendOutput)//evaluates all the occurences of a single sign ("+","-",...)
        {
            int maxLoop = 100;
            int currentLoop = 0;
            int searchFromIndex = 0;
            while (containsSign(input, sign) && currentLoop < maxLoop)
            {
                currentLoop++;

                int signIndex = input.IndexOf(sign, searchFromIndex);
                
                if (sign == "*/")//find which one is first to evaluate them from from left to right
                {
                    int index1 = input.IndexOf("*", searchFromIndex);
                    int index2 = input.IndexOf("/", searchFromIndex);
                    index1 = (index1 == -1) ? int.MaxValue : index1;//to prevent -1 from "being before" the other sign
                    index2 = (index2 == -1) ? int.MaxValue : index2;
                    //set sign to "*" or "/" depending on which one has a lower index
                    sign = (index1 < index2) ? "*" : "/";
                    signIndex = (index1 < index2) ? index1 : index2;
                }

                if (signIndex == -1 || signIndex == int.MaxValue) break;

                String start = input.Substring(0, signIndex);//before the signIndex
                String end = input.Substring(signIndex + 1);//after the signIndex

                String startNumStrBracketed = getNumberOnSideOfString(start, false, (sign == "^"));
                String endNumStrBracketed = getNumberOnSideOfString(end, true, false);
                String startNumStr = removeBrackets(startNumStrBracketed);
                String endNumStr = removeBrackets(endNumStrBracketed);
                double startNum = (startNumStr == "") ? double.NaN : double.Parse(startNumStr);//number on the left of the sign
                double endNum = (endNumStr == "") ? double.NaN : double.Parse(endNumStr);//number on the right of the sign

                if (sign == "-" && double.IsNaN(startNum) && endNumStrBracketed.StartsWith("(")) startNum = 0;
                if (double.IsNaN(startNum) || double.IsNaN(endNum))
                {
                    searchFromIndex = signIndex + 1;
                    //if startNum or endNum is not a number, search from index signIndex+1 to prevent an infinite loop
                }
                else
                {
                    double result = double.NaN;
                    //evaluate the result based on the sign
                    if (sign == "+") result = startNum + endNum;
                    if (sign == "-") result = startNum - endNum;
                    if (sign == "×") result = startNum * endNum;
                    if (sign == "*") result = startNum * endNum;
                    if (sign == "/") result = startNum / endNum;
                    if (sign == "^") result = Math.Pow(startNum, endNum);
                    String strResult = Convert.ToString(result);
                    if (!strResult.StartsWith("-")) strResult = "+" + strResult;//to prevent situations when the result doesnt have a sign in front when it should
                    input = input.Substring(0, signIndex - startNumStrBracketed.Length) + strResult + input.Substring(signIndex + endNumStrBracketed.Length + 1);
                    input = removeDoubledSigns(input);
                    searchFromIndex = 0;

                    //output the progress if showCalcProcess
                    if (sendOutput) Console.WriteLine("=> " + finalizeOutput(input, true));
                }
                if (sign == "*" || sign == "/") sign = "*/";//to reset the sign for the next round
            }
            return input;
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
        static String getNumberOnSideOfString(String input, bool sideLeft, bool exponentiating)//gets the starting/ending number of a string
        {

            if (sideLeft) //getStartingNumber
            { 

                for (int i = input.Length; i > 0; i--)
                {
                    /* -> 22*64     is not a number
                     * -> 22*6      is not a number
                     * -> 22*       is not a number
                     * -> 22        is a number -> returns 22
                    */
                    String sub = input.Substring(0, i);
                    if (finalize(sub) != "") return finalize(sub);
                }
            }
            else //getEndingNumber
            {
                for (int i = 0; i < input.Length; i++)
                {
                    /* -> 22*64     is not a number
                     * -> 2*64      is not a number
                     * -> *64       is not a number
                     * -> 64        is a number -> returns 64
                    */
                    
                    String sub = input.Substring(i);
                    if (finalize(sub) != "") return finalize(sub);
                }
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
        static String removeBrackets(String input)//remove brackets on the start and end of string
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
        static String finalizeOutput(String input, bool showIrrationals)//rounds the input and replace values with irrationals
        {
            input = removeBrackets(unifyBrackets(removeDoubledSigns(input)));
            //if the result is not a number, return the unchanged input
            if (!double.TryParse(input, out double ans)) return input;
            //if you dont want to show irrationals, return the rounded input
            if (!showIrrationals) return Convert.ToString(Math.Round(ans, 10));

            Dictionary<double, String> replaceIrrationals = new Dictionary<double, String>();
            //key= replace what number    value = what to replace with
            //{Math.PI, "pi"}  -> replace 3.14159 with "pi"
            replaceIrrationals.Add(Math.PI, "pi");
            replaceIrrationals.Add(Math.E, "e");
            replaceIrrationals.Add(Math.Sqrt(2), "sqrt(2)");
            replaceIrrationals.Add(Math.Sqrt(3), "sqrt(3)");
            foreach (KeyValuePair<double, String> entry in replaceIrrationals)
            {
                double value = entry.Key;
                String valueStr = entry.Value;
                double tryMultiply = double.NaN;
                double tryDivide = double.NaN;
                if (value != 0) tryMultiply = Math.Round(ans / value, 12);
                if (value != 0) tryDivide = Math.Round(value / ans, 12);
                if (int.TryParse(Convert.ToString(tryMultiply), out int multiplyBy) && multiplyBy != 0)
                {
                    // finds if the number is a multiple of a irrational number, then replaces it
                    // 6,2831853072  -> 2pi
                    input = ((multiplyBy == 1) ? "" : Convert.ToString(multiplyBy)) + valueStr;
                }
                if (int.TryParse(Convert.ToString(tryDivide), out int divideBy) && divideBy != 0)
                {
                    // finds if the number is a divisor of a irrational number, then replaces it
                    // 1,5707963268  -> pi/2
                    input = valueStr + ((divideBy == 1) ? "" : "/" + Convert.ToString(divideBy));
                }
            }
            if (double.TryParse(input, out double finalAns)) return Convert.ToString(Math.Round(finalAns, 10));
            return input;
        }
    }
}
