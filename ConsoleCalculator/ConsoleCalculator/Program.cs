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
        static void Main(string[] args)
        {

            //USING THE PEJMDAS SYSTEM

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
            Console.WriteLine("Saved Variables:");
            Console.WriteLine("   - ans    (value of the last answer)");
            Console.WriteLine("\nExample: > 5*((1-3)^2)+4");
            Console.WriteLine("         ans = 24");
            String lastAns = "0";
            while (true)
            {
                String input = unifyBrackets(Console.ReadLine()).Replace("ans", lastAns).Replace(" ", "").Replace("÷", "/").Replace("×", "*");
                //input = juxtaposition(input);
                String evaledInput = evaluateString(input, showCalcProcess);
                lastAns = finalizeOutput(evaledInput);
                Console.WriteLine("ans = " + finalizeOutput(evaledInput) + "\n");
            }
            Console.ReadKey();
        }

        static String evaluateString(String input, bool sendOutput)
        {
            input = evaluateBrackets(input, sendOutput);
            String[] order = new String[] { "^", "×", "*/", "+", "-" };
            foreach (String sign in order)
            {
                input = evaluateWithSign(input, sign, sendOutput);
            }
            return input;
        }
        static String juxtaposition(String input)
        {
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
                /*if (double.TryParse(inBracketStr, out double result))
                {
                }*/
                String beforeBracket = input.Substring(0, bracketStartIndex);
                String afterBracket = input.Substring(bracketStartIndex + bracketStr.Length);
                input = beforeBracket + "[" + evaluateString(inBracketStr, false) + "]" + afterBracket;
                input = removeDoubledSigns(input);
                lastEvaledBracket = beforeBracket.Length;
                if (sendOutput) Console.WriteLine("=> " + unifyBrackets(input));
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

                    if (sendOutput) Console.WriteLine("=> " + unifyBrackets(input));
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
            if (double.TryParse(input, out double result))
            {
                input = Convert.ToString(double.Parse(input));
            }
            return input;
        }
    }
}
