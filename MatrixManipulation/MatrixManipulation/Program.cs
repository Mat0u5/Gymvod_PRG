using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;
using System.Data.Common;
using System.Reflection;
using System.Xml.Linq;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2023-2024.
 * Extended by students.
 */

namespace MatrixManupulation
{
    internal class Program
    {
        static Dictionary<string, int[,]> matrices = new Dictionary<string, int[,]>();
        static void Main(string[] args)
        {
            printInstructions();
            while (true)
            {
                string input = Console.ReadLine();
                processInput(input);
            }
            Console.ReadKey();
        }
        static void printInstructions()
        {

            string instructions =
                "Commands:\n" +
                "  create  <arrayName>  <arrayWidth>,<arrayHeight>\n" +
                "  createRandom  <arrayName>  <arrayWidth>,<arrayHeight>  <rndMin>,<rndMax>\n" +
                "  delete  <arrayName>\n" +
                "  listArrays\n" +
                "  print  <arrayName>\n" +
                "  printRow  <arrayName>  <rowNum>\n" +
                "  printColumn  <arrayName>  <columnNum>\n" +
                "  swapRows  <arrayName>  <row1>,<row2>\n" +
                "  swapColumns  <arrayName>  <col1>,<col2>\n" +
                "  swapElements  <arrayName>  <x1>,<y1>  <x2>,<y2>\n" +
                "  transposeMain  <arrayName>\n" +
                "  transposeSecondary  <arrayName>\n" +
                "";
            WriteColorLine(instructions, ConsoleColor.Cyan);
        }
        static void processInput(string input)
        {
            string[] args = { input };
            if (input.Contains(" ")) args = input.Split(' ');

            string matrixName = "";
            int[,] matrix = new int[0, 0];
            int[,] oldMatrix = new int[0, 0];
            if (args.Length >= 2)
            {
                if (matrices.ContainsKey(args[1]))
                {
                    matrixName = args[1];
                    matrix = matrices[matrixName];
                    oldMatrix = (int[,])matrix.Clone();
                }
            }
            switch (args[0])
            {
                case "create":
                case "createRandom":
                    if (args.Length < 3 || !args[2].Contains(","))
                    {
                        if (args[0] == "create") WriteColorLine("Invalid Syntax. Usage:  create  <arrayName>  <arrayWidth>,<arrayHeight>", ConsoleColor.Red);
                        else if (args[0] == "createRandom") WriteColorLine("Invalid Syntax. Usage:  createRandom  <arrayName>  <arrayWidth>,<arrayHeight>  <rndMin>,<rndMax>", ConsoleColor.Red);
                        break;
                    }
                    if (!int.TryParse(args[2].Split(',')[0], out int arrayWidth))
                    {
                        WriteColorLine("Invalid number.", ConsoleColor.Red);
                        break;
                    }
                    if (!int.TryParse(args[2].Split(',')[1], out int arrayHeight))
                    {
                        WriteColorLine("Invalid number.", ConsoleColor.Red);
                        break;
                    }
                    matrixName = args[1];
                    bool createRandom = args[0] == "createRandom";
                    if (createRandom)
                    {
                        if (args.Length < 4 || !args[3].Contains(","))
                        {
                            WriteColorLine("Invalid Syntax. Usage:  createRandom  <arrayName>  <arrayWidth>,<arrayHeight>  <rndMin>,<rndMax>", ConsoleColor.Red);
                            break;
                        }

                        if (!int.TryParse(args[3].Split(',')[0], out int rndMin))
                        {
                            WriteColorLine("Invalid number.", ConsoleColor.Red);
                            break;
                        }
                        if (!int.TryParse(args[3].Split(',')[1], out int rndMax))
                        {
                            WriteColorLine("Invalid number.", ConsoleColor.Red);
                            break;
                        }
                        matrix = createRandomArray(arrayWidth, arrayHeight, rndMin, rndMax + 1);
                    }
                    else
                    {
                        matrix = createAndFillArray(arrayWidth, arrayHeight);
                    }
                    if (matrices.ContainsKey(matrixName))
                    {
                        WriteColorLine("A matrix with that name already exists.", ConsoleColor.Red);
                        break;
                    }
                    printArray(matrix, matrixName);
                    matrices.Add(matrixName, matrix);
                    break;
                case "delete":
                    if (args.Length != 2)
                    {
                        WriteColorLine("Invalid Syntax. Usage:  delete  <arrayName>", ConsoleColor.Red);
                        break;
                    }
                    if (!matrices.ContainsKey(args[1]))
                    {
                        WriteColorLine("A matrix with that name does not exist.", ConsoleColor.Red);
                        break;
                    }
                    matrices.Remove(args[1]);
                    WriteColorLine($"{args[1]} was successfully deleted", ConsoleColor.Green);
                    break;
                case "listArrays":
                    if (matrices.Keys.Count == 0) WriteColorLine("There are no arrays to print.", ConsoleColor.Red);
                    foreach (string name in matrices.Keys)
                    {
                        matrix = matrices[name];
                        printArray(matrix, name);
                    }
                    break;
                case "print":
                    if (args.Length < 2)
                    {
                        WriteColorLine("Invalid Syntax. Usage:  print  <arrayName>", ConsoleColor.Red);
                        break;
                    }
                    if (!matrices.ContainsKey(args[1]))
                    {
                        WriteColorLine("A matrix with that name does not exist.", ConsoleColor.Red);
                        break;
                    }
                    matrixName = args[1];
                    matrix = matrices[matrixName];
                    printArray(matrix, matrixName);
                    break;
                case "printColumn":
                case "printRow":
                    if (args.Length < 3)
                    {
                        if (args[0] == "printRow") WriteColorLine("Invalid Syntax. Usage:  printRow  <arrayName> <rowNum>", ConsoleColor.Red);
                        else if (args[0] == "printColumn") WriteColorLine("Invalid Syntax. Usage:  printColumn  <arrayName> <columnNum>", ConsoleColor.Red);
                        break;
                    }
                    if (!matrices.ContainsKey(args[1]))
                    {
                        WriteColorLine("A matrix with that name does not exist.", ConsoleColor.Red);
                        break;
                    }
                    if (!int.TryParse(args[2], out int num))
                    {
                        WriteColorLine("Invalid number.", ConsoleColor.Red);
                        break;
                    }
                    matrixName = args[1];
                    matrix = matrices[matrixName];
                    if (args[0] == "printRow") printArrayRow(matrix, num);
                    else if (args[0] == "printColumn") printArrayColumn(matrix, num);
                    break;
                case "swapRows":
                case "swapColumns":
                    if (args.Length < 3 || !args[2].Contains(","))
                    {
                        if (args[0] == "swapRows") WriteColorLine("Invalid Syntax. Usage:  swapRows  <arrayName>  <row1>,<row2>", ConsoleColor.Red);
                        else if (args[0] == "swapColumns") WriteColorLine("Invalid Syntax. Usage:  swapColumns  <arrayName>  <col1>,<col2>", ConsoleColor.Red);
                        break;
                    }

                    if (!int.TryParse(args[2].Split(',')[0], out int swap))
                    {
                        WriteColorLine("Invalid number.", ConsoleColor.Red);
                        break;
                    }
                    if (!int.TryParse(args[2].Split(',')[1], out int swapWith))
                    {
                        WriteColorLine("Invalid number.", ConsoleColor.Red);
                        break;
                    }
                    matrixName = args[1];
                    matrix = matrices[matrixName];
                    oldMatrix = (int[,])matrix.Clone();
                    matrix = (args[0] == "swapRows") ? swapRows(matrix, swap, swapWith) : swapColumns(matrix, swap, swapWith);
                    printArrayWithChanges(matrix, oldMatrix, matrixName);
                    matrices[matrixName] = matrix;
                    break;
                case "swapElements":
                    if (args.Length < 4 || !args[2].Contains(",") || !args[3].Contains(","))
                    {
                        WriteColorLine("Invalid Syntax. Usage:  swapElements  <arrayName>  <x1>,<y1>  <x2>,<y2>", ConsoleColor.Red);
                        break;
                    }
                    if (!int.TryParse(args[2].Split(',')[0], out int x1)) { WriteColorLine("Invalid number.", ConsoleColor.Red); break; }
                    if (!int.TryParse(args[2].Split(',')[1], out int y1)) { WriteColorLine("Invalid number.", ConsoleColor.Red); break; }
                    if (!int.TryParse(args[3].Split(',')[0], out int x2)) { WriteColorLine("Invalid number.", ConsoleColor.Red); break; }
                    if (!int.TryParse(args[3].Split(',')[1], out int y2)) { WriteColorLine("Invalid number.", ConsoleColor.Red); break; }
                    matrixName = args[1];
                    matrix = matrices[matrixName];
                    oldMatrix = (int[,])matrix.Clone();
                    matrix = swapTwoElements(matrix, new int[] { x1, y1 }, new int[] { x2, y2 });
                    printArrayWithChanges(matrix, oldMatrix, matrixName);
                    matrices[matrixName] = matrix;
                    break;
                case "transposeMain":
                case "transposeSecondary":
                    if (args.Length < 2)
                    {
                        if (args[0] == "transposeMain") WriteColorLine("Invalid Syntax. Usage:  transposeMain  <arrayName>", ConsoleColor.Red);
                        else if (args[0] == "transposeSecondary") WriteColorLine("Invalid Syntax. Usage:  transposeSecondary  <arrayName>", ConsoleColor.Red);
                        break;
                    }
                    if (!matrices.ContainsKey(args[1]))
                    {
                        WriteColorLine("A matrix with that name does not exist.", ConsoleColor.Red);
                        break;
                    }
                    matrixName = args[1];
                    matrix = matrices[matrixName];
                    oldMatrix = (int[,])matrix.Clone();
                    matrix = (args[0] == "transposeMain") ? transposeMain(matrix) : transposeSecondary(matrix);
                    printArrayWithChanges(matrix, oldMatrix, matrixName);
                    matrices[matrixName] = matrix;
                    break;
                case "clear":
                    Console.Clear();
                    printInstructions();
                    break;

                default:
                    WriteColorLine("Invalid command.", ConsoleColor.Red);
                    break;

            }
        }
        static int[,] createAndFillArray(int width, int height)
        {
            int[,] array = new int[height, width];
            int row = 0;
            Random rnd = new Random();
            for (int i = 1; i <= width * height; i++)
            {
                if ((i - 1) % width == 0 && i != 1) row++;
                int col = i - 1 - row * width;
                array[row, col] = i;

            }
            return array;
        }
        static int[,] createRandomArray(int width, int height, int rndMin, int rndMax)
        {
            int[,] array = new int[height, width];
            int row = 0;
            Random rnd = new Random();
            for (int i = 1; i <= width * height; i++)
            {
                if ((i - 1) % width == 0 && i != 1) row++;
                int col = i - 1 - row * width;
                array[row, col] = rnd.Next(rndMin, rndMax);

            }
            return array;
        }
        static void WriteColorLine(string value, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }
        static void WriteColor(string value, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ResetColor();
        }
        static int getMaxArrayValueLength(int[,] array)
        {
            int maxLength = Convert.ToString(array[0, 0]).Length;
            for (int row = 0; row < array.GetLength(0); row++)
            {
                for (int col = 0; col < array.GetLength(1); col++)
                {
                    maxLength = (Convert.ToString(array[row, col]).Trim().Length > maxLength) ? Convert.ToString(array[row, col]).Trim().Length : maxLength;

                }
            }

            return maxLength;
        }
        static void printArray(int[,] array, string arrayName)
        {
            arrayName += " = ";
            int middle = array.GetLength(0) / 2;
            string emptyName = new string(' ', arrayName.Length);
            int maxArrayValueLength = getMaxArrayValueLength(array);
            for (int row = 0; row < array.GetLength(0); row++)
            {
                Console.Write(" " + ((row == middle) ? arrayName : emptyName));
                for (int col = 0; col < array.GetLength(1); col++)
                {
                    String addToEnd = (col != array.GetLength(1) - 1) ? ", " : "";
                    String printElement = Convert.ToString(array[row, col]);
                    int spacesNum = maxArrayValueLength - printElement.Length;
                    if (spacesNum == -1) spacesNum = 1;
                    String addSpaces = new string(' ', spacesNum);
                    Console.Write(printElement + addToEnd + addSpaces);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
        static void printArrayWithChanges(int[,] array, int[,] lastArray, string arrayName)
        {
            arrayName += " = ";
            int middle = array.GetLength(0) / 2;
            string emptyName = new string(' ', arrayName.Length);
            int maxArrayValueLength = getMaxArrayValueLength(array);
            for (int row = 0; row < array.GetLength(0); row++)
            {
                Console.Write(" " + ((row == middle) ? arrayName : emptyName));
                for (int col = 0; col < array.GetLength(1); col++)
                {
                    String addToEnd = (col != array.GetLength(1) - 1) ? ", " : "";
                    String printElement = Convert.ToString(array[row, col]);
                    int spacesNum = maxArrayValueLength - printElement.Length;
                    String addSpaces = new string(' ', spacesNum);
                    try
                    {
                        //if the elements from the lastArray (array before last change) dont match, print with red color
                        if (array[row, col] == lastArray[row, col]) Console.Write(printElement);
                        else WriteColor(printElement, ConsoleColor.Red);
                    }
                    catch (Exception)
                    {
                        //lastArray and array have a different size
                        WriteColor(printElement, ConsoleColor.Red);
                    }
                    Console.Write(addToEnd + addSpaces);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
        static void printArrayRow(int[,] array, int row)
        {
            for (int col = 0; col < array.GetLength(1); col++)
            {
                if (row >= 0 && row < array.GetLength(0))
                {
                    Console.Write(array[row, col]);
                    if (col != array.GetLength(1) - 1) Console.Write(", ");
                    else Console.Write("\n\n");
                }
                else
                {
                    WriteColorLine("Out of bounds of the matrix.", ConsoleColor.Red);
                    return;
                }
            }
        }
        static void printArrayColumn(int[,] array, int col)
        {
            for (int row = 0; row < array.GetLength(0); row++)
            {
                if (col >= 0 && col < array.GetLength(1))
                {
                    Console.Write(array[row, col]);
                    if (row != array.GetLength(0) - 1) Console.Write(", ");
                    else Console.Write("\n\n");
                }
                else
                {
                    WriteColorLine("Out of bounds of the matrix.", ConsoleColor.Red);
                    return;
                }
            }
        }
        static int[,] transposeMain(int[,] array)
        {
            int[,] newArray = new int[array.GetLength(1), array.GetLength(0)];
            for (int row = 0; row < array.GetLength(0); row++)
            {
                for (int col = 0; col < array.GetLength(1); col++)
                {
                    newArray[col, row] = array[row, col];
                }
            }
            return newArray;
        }
        static int[,] transposeSecondary(int[,] array)
        {
            int originalRowsNum = array.GetLength(0);
            int originalColsNum = array.GetLength(1);
            int[,] newArray = new int[array.GetLength(1), array.GetLength(0)];
            for (int row = 0; row < array.GetLength(0); row++)
            {
                for (int col = 0; col < array.GetLength(1); col++)
                {
                    newArray[originalColsNum - 1 - col, originalRowsNum - 1 - row] = array[row, col];
                }
            }
            return newArray;
        }
        static int[,] swapRows(int[,] array, int swapAt, int swapWith)
        {
            for (int col = 0; col < array.GetLength(1); col++)
            {
                if (swapAt >= 0 && swapAt < array.GetLength(0) && swapWith >= 0 && swapWith < array.GetLength(0))
                {
                    int temp = array[swapAt, col];
                    array[swapAt, col] = array[swapWith, col];
                    array[swapWith, col] = temp;
                }
                else
                {
                    WriteColorLine("Out of bounds of the matrix.", ConsoleColor.Red);
                    return array;
                }
            }
            return array;
        }
        static int[,] swapColumns(int[,] array, int swapAt, int swapWith)
        {
            for (int row = 0; row < array.GetLength(0); row++)
            {
                if (swapAt >= 0 && swapAt < array.GetLength(1) && swapWith >= 0 && swapWith < array.GetLength(1))
                {
                    int temp = array[row, swapAt];
                    array[row, swapAt] = array[row, swapWith];
                    array[row, swapWith] = temp;
                }
                else
                {
                    WriteColorLine("Out of bounds of the matrix.", ConsoleColor.Red);
                    return array;
                }
            }
            return array;
        }
        static int[,] swapTwoElements(int[,] array, int[] swapAt, int[] swapWith)
        {
            if (swapAt[0] < 0 || swapAt[1] < 0 || swapAt[0] >= array.GetLength(1) || swapAt[1] >= array.GetLength(0) ||
                swapWith[0] < 0 || swapWith[1] < 0 || swapWith[0] >= array.GetLength(1) || swapWith[1] >= array.GetLength(0))
            {
                WriteColorLine("Out of bounds of the matrix.", ConsoleColor.Red);
                return array;
            }
            int temp = array[swapAt[1], swapAt[0]];
            array[swapAt[1], swapAt[0]] = array[swapWith[1], swapWith[0]];
            array[swapWith[1], swapWith[0]] = temp;
            return array;
        }
    }
}
