using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator
{
    internal class Evaluator
    {
        public static String evaluateBrackets(String input)
        {   
            return input;
        }
        public static String evaluateSign(String input, String sign, int startIndex)
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
            String rightNumStrBracketed = StringUtils.getNumberOnSideOfString(rightSide, -1, sign.Equals("^"));
            String leftNumStr = StringUtils.removeBrackets(leftNumStrBracketed);
            String rightNumStr = StringUtils.removeBrackets(rightNumStrBracketed);
            String strResult = "";
            bool leftValid = Double.TryParse(leftNumStr, out double leftNum);
            bool rightValid = Double.TryParse(rightNumStr, out double rightNum);
            if (!leftValid || !rightValid)
            {
                if (!leftValid && rightValid && sign.Equals("+")) strResult = rightNumStr;
                else if (!leftValid && rightValid && sign.Equals("-")) strResult = "−" + rightNumStr;
                else return throwError($"SignError at index {index}, found {leftNumStrBracketed} and {rightNumStrBracketed}");
            }
            else
            {
                double result = double.NaN;
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
            Console.WriteLine("=> " + StringUtils.finalizeOutput(input, true));
            return input.Contains(sign) ? evaluateSign(input, sign, 0): input;

            String throwError(String error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(error);
                Console.ResetColor();
                input = leftSide += rightSide;
                return input.Contains(sign) ? evaluateSign(input, sign, 0) : input;
            }
        }
        public static String evaulate(String input)
        {

            input = evaluateBrackets(input);
            String[] signListInOrder = new String[] { "^", "×", "*/", "+", "-" };
            while (StringUtils.containsAnySign(input, signListInOrder))
            {
                foreach (String sign in signListInOrder) input = evaluateSign(input, sign, 0);
            }
            return input;
        }
    }
}
