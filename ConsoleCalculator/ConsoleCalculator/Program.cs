using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        static void Main(string[] args)
        {

            //USES THE PEJMDAS SYSTEM


            Console.WriteLine("Allowed operations:");
            Console.WriteLine("   - exponentiation   ^");
            Console.WriteLine("   - multiplication   *");
            Console.WriteLine("   - division         /");
            Console.WriteLine("   - addition         +");
            Console.WriteLine("   - subtraction      -");
            Console.WriteLine("Example: > 5*((1-3)^2)+4");
            Console.WriteLine("         = 24");
            while (true)
            {
                String input = Console.ReadLine();
                String evaledInput = evaluateString(input, true);
                Console.WriteLine("= " + evaledInput);
            }
            Console.ReadKey();
        }

        static String evaluateString(String input, bool sendOutput)
        {
            int maxLoop = 100;
            int currentLoop = 0;
            while (input.Contains("(") && currentLoop < maxLoop)
            {
                int bracketStartIndex = input.IndexOf("(");
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
                input = beforeBracket + evaluateString(inBracketStr, false) + afterBracket;
                if (sendOutput) Console.WriteLine("=> " + input);
                currentLoop++;
            }

            String[] order = new String[] { "^", "*/", "+", "-" };
            foreach (String sign in order)
            {
                input = evaluateWithSign(input, sign, sendOutput);
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

                if (index == -1)
                {
                    finished = true;
                    break;
                }
                String start = input.Substring(0, index);
                String end = input.Substring(index + 1);
                String startNumStr = getEndingNumber(start);
                String endNumStr = getStartingNumber(end);
                double startNum = (startNumStr == "") ? double.NaN : double.Parse(startNumStr);
                double endNum = (endNumStr == "") ? double.NaN : double.Parse(endNumStr);
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
                    if (sign == "*") result = startNum * endNum;
                    if (sign == "/") result = startNum / endNum;
                    if (sign == "^") result = Math.Pow(startNum, endNum);
                    String strResult = Convert.ToString(result);
                    if (Convert.ToString(startNum).StartsWith("-") && !strResult.StartsWith("-")) strResult = "+" + strResult;//to prevent situations when the result doesnt have a sign in from when it should
                    input = input.Substring(0, index - startNumStr.Length) + strResult + input.Substring(index + endNumStr.Length + 1);
                    startAtIndex = 0;

                    if (sendOutput) Console.WriteLine("=> " + input);
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
        static String getEndingNumber(String input)//converts "11*2+32" --> 32
        {
            for (int i = 0; i < input.Length; i++)
            {
                String sub = input.Substring(i);
                try
                {
                    double num = double.Parse(sub);
                    return sub;
                }
                catch (Exception) { }
            }
            return "";
        }
        static String getStartingNumber(String input)//converts "11*2+32" --> 11
        {
            for (int i = input.Length; i > 0; i--)
            {
                String sub = input.Substring(0, i);
                try
                {
                    double num = double.Parse(sub);
                    return sub;
                }
                catch (Exception) { }
            }
            return "";
        }
    }
}
