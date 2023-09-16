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
        static String lastAns = "0";
        static String[] variables = new String[] {"ans", "pi", "e"};
        static String[] functions = new String[] {"sqrt", "sin", "cos", "cotg", "tg" };
        static void Main(string[] args)
        {

            //USING THE PEJMDAS SYSTEM
            //FUNCTIONS USE DEGREES, NOT RADIANS

            /*
             * TODO add aproximate value to the result if handling irrational numbers
             */

            /* Some testing problems with outside verification
                2*3+4*6			    	=30
                (-3)(2)+18/3	    	=0
                4+(6-2)^2+1		    	=21
                8(6-2)/2(5-3)	    	=32
                (-12)(-3)+8^2	    	=100
                4/5*25+2		    	=22
                (-9(2+1))/(-2(-2-1))	=-4.5
                4(3+1)-2(5-2)		    =10
                14/(-3-4)			    =-2
                (2^2-4^2)/(-3-1)	    =3
                -(-3)+8/4			    =5
                9^2-8-2^3			    =65
                (-7-9)(8-4)+4^3/8	    =-56
                6+3*2-12/4			    =9
                7(3+2)-5			    =30
                10×4-2×(4^2÷4)÷2÷(1/2)+9    =41
                -10÷(20÷2^2×5÷5)×8-2    =-18
             */


            Console.WriteLine("Allowed operations:");
            Console.WriteLine("   - exponentiation   ^");
            Console.WriteLine("   - multiplication   *,×");
            Console.WriteLine("   - division         /,÷");
            Console.WriteLine("   - addition         +");
            Console.WriteLine("   - subtraction      -"); 
            Console.WriteLine("Allowed functions:");
            Console.WriteLine("   - sqrt()");
            Console.WriteLine("   - sin()");
            Console.WriteLine("   - cos()");
            Console.WriteLine("   - tg()");
            Console.WriteLine("   - cotg()");
            Console.WriteLine("Saved Variables:");
            Console.WriteLine("   - ans    (value of the last answer)");
            Console.WriteLine("   - pi");
            Console.WriteLine("   - e");
            Console.WriteLine("\nExample: > 5*((1-3)^2)+4");
            Console.WriteLine("         ans = 24");
            while (true)
            {
                String input = parseInput(Console.ReadLine());
                //input = juxtaposition(input);
                String evaledInput = evaluateString(input, showCalcProcess);
                lastAns = finalizeOutput(evaledInput);
                Console.WriteLine("ans = " + finalizeOutput(evaledInput) + "\n");
            }
            Console.ReadKey();
        }
        static String parseInput(String input)
        {
            input = unifyBrackets(input).Replace(".", ",").Replace(" ", "").Replace("÷", "/").Replace("×", "*");
            input = juxtaposition(input).Replace("ans", lastAns).Replace("pi", Convert.ToString(Math.PI)).Replace("e", Convert.ToString(Math.E));
            return input;
        }
        static String evaluateFunction(String input)//is run from evaluateBrackets()  !INPUT HAS TO BE A SINGLE FUNCTION ex. sqrt(2)
        {
            foreach (String function in functions) {
                if (input.StartsWith(function+"(") && input.EndsWith(")")) {
                    String inFunction = input.Substring((function + "(").Length);
                    inFunction = inFunction.Substring(0, inFunction.Length - 1);
                    if (double.TryParse(inFunction, out double inFunctionNum))
                    {
                        double result = 0;
                        double angleInBrackets = 0;
                        if (useRadians) angleInBrackets = inFunctionNum;
                        else angleInBrackets = Math.PI * (inFunctionNum / 180.0);


                        if (function == "sqrt") result = Math.Sqrt(inFunctionNum);
                        if (function == "sin") result = Math.Sin(angleInBrackets);
                        if (function == "cos") result = Math.Cos(angleInBrackets);
                        if (function == "tg") result = Math.Tan(angleInBrackets);
                        if (function == "cotg") result = 1 / Math.Tan(angleInBrackets);
                        input = Convert.ToString(result);
                    }
                }
            }
            return input;
        }
        static String evaluateString(String input, bool sendOutput)
        {
            input = evaluateBrackets(input, sendOutput);//contains juxtaposition() and evaluateFunction()
            String[] order = new String[] { "^", "×", "*/", "+", "-" };
            foreach (String sign in order)
            {
                input = evaluateWithSign(input, sign, sendOutput);
            }
            return input;
        }
        static String juxtaposition(String input)
        {
            foreach(String variable in variables) input = input.Replace(variable,"("+ variable+")");
            foreach (String function in functions) input = input.Replace(function, "(" + function.ToUpper());

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
            foreach (String variable in variables) input = input.Replace("(" + variable + ")",variable);
            foreach (String function in functions) input = input.Replace("(" + function.ToUpper(), function);
            return input;
        }
        static String evaluateBrackets(String input, bool sendOutput)
        {
            //2sqrt(3+1)^3
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
                        input = beforeBracket.Substring(0, beforeBracket.Length-function.Length) + "[" + evaluatedFunction + "]" + afterBracket;
                        break;
                    }
                }
                if (sendOutput) Console.WriteLine("=> " + finalizeOutput(input));
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

                    if (sendOutput) Console.WriteLine("=> " + finalizeOutput(input));
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
            else
            {
                return editedInput.Contains(sign);
            }
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
        static String finalizeOutput(String input)
        {
            input = removeBrackets(unifyBrackets(removeDoubledSigns(input)));
            if (double.TryParse(input, out double ans))
            {
                //currently only replacing irrationals if the answer is a number
                Dictionary<double, String> replaceIrrationals = new Dictionary<double, String>();
                replaceIrrationals.Add(Math.PI, "pi");
                replaceIrrationals.Add(Math.E, "e");
                replaceIrrationals.Add(Math.Sqrt(2), "sqrt(2)");
                replaceIrrationals.Add(Math.Sqrt(3), "sqrt(3)");
                replaceIrrationals.Add(0, "0");//TODO THIS IS SO UGLY       to prevent situation like 1,22460635382238E-16
                bool irrReplaced = false;
                foreach (KeyValuePair<double, String> entry in replaceIrrationals)
                {
                    double value = entry.Key;
                    String valueStr = entry.Value;
                    double tryMultiply = double.NaN;
                    double tryDivide = double.NaN;
                    if (value != 0) tryMultiply = Math.Round(ans / value, 12);
                    if (value != 0) tryDivide = Math.Round(value / ans, 12);
                    if (value == 0 && Math.Round(ans, 15)==0)
                    {
                        input = "0";
                        irrReplaced = true;
                    }
                    if (int.TryParse(Convert.ToString(tryMultiply), out int multiplyBy) && multiplyBy != 0)
                    {
                        input = ((multiplyBy==1) ?"": Convert.ToString(multiplyBy)) +valueStr;
                        irrReplaced = true;
                    }
                    if (int.TryParse(Convert.ToString(tryDivide), out int divideBy) && divideBy != 0)
                    {
                        input = valueStr + ((divideBy == 1) ? "" : "/" + Convert.ToString(divideBy));
                        irrReplaced = true;
                    }
                }

                if (!irrReplaced) input = Convert.ToString(ans);
            }
            //irrationals

            //
            return input;
        }
    }
}
