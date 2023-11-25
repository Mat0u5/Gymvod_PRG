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
using System.Runtime.CompilerServices;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2023-2024.
 * Extended by students.
 */

namespace MatrixManupulation
{
    internal class Program
    {
        static Dictionary<string, int[,]> matrices = new Dictionary<string, int[,]>();//saved matrices
        static void Main(string[] args)
        {
            printInstructions();
            while (true)
            {
                string input = Console.ReadLine();
                processInput(input);
            }
        }
        static void processInput(string input)
        {
            string[] args = { input };
            if (input.Contains(" ")) args = input.Split(' ');
            //the varables are declared here because switch statements dont allow 
            // declaring two variables with the same name in two different cases
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
            switch (args[0].ToLower())
            {
                //this is not pretty at all, but i didnt want to have like 30 different functions
                //doesn't have many comments since you can tell whats going on from the console write lines.
                case "create":
                case "createrandom":
                    if (args.Length < 3 || !args[2].Contains(","))
                    {
                        if (args[0].ToLower() == "create") WriteColorLine("Invalid Syntax. Usage:  create  <matrixName>  <rowNum>,<colNum>", ConsoleColor.Red);
                        else if (args[0].ToLower() == "createrandom") WriteColorLine("Invalid Syntax. Usage:  createRandom  <matrixName>  <rowNum>,<colNum>  <rndMin>,<rndMax>", ConsoleColor.Red);
                        break;
                    }
                    if (int.TryParse(args[1], out int matrixTestNameNumber))
                    {
                        WriteColorLine("The name cannot be a number.", ConsoleColor.Red);
                        break;
                    }
                    if (!int.TryParse(args[2].Split(',')[0], out int matrixWidth) || matrixWidth <=0)
                    {
                        WriteColorLine("Invalid number.", ConsoleColor.Red);
                        break;
                    }
                    if (!int.TryParse(args[2].Split(',')[1], out int matrixHeight) || matrixHeight<=0)
                    {
                        WriteColorLine("Invalid number.", ConsoleColor.Red);
                        break;
                    }
                    matrixName = args[1];
                    bool createRandom = args[0].ToLower() == "createrandom";
                    if (createRandom)
                    {
                        //creating a randomly filled matrix
                        if (args.Length < 4 || !args[3].Contains(","))
                        {
                            WriteColorLine("Invalid Syntax. Usage:  createRandom  <matrixName>  <matrixWidth>,<matrixHeight>  <rndMin>,<rndMax>", ConsoleColor.Red);
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
                        matrix = createRandomMatrix(matrixWidth, matrixHeight, rndMin, rndMax);
                    }
                    else
                    {
                        matrix = createAndFillMatrix(matrixWidth, matrixHeight);
                    }
                    if (matrices.ContainsKey(matrixName))
                    {
                        WriteColorLine("A matrix with that name already exists.", ConsoleColor.Red);
                        break;
                    }
                    printMatrix(matrix, matrixName);
                    matrices.Add(matrixName, matrix);
                    break;
                case "delete":
                    if (args.Length != 2)
                    {
                        WriteColorLine("Invalid Syntax. Usage:  delete  <matrixName>", ConsoleColor.Red);
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
                case "listmatrices":
                    if (matrices.Keys.Count == 0) WriteColorLine("There are no matrices to print.", ConsoleColor.Red);
                    foreach (string name in matrices.Keys)
                    {
                        matrix = matrices[name];
                        printMatrix(matrix, name);
                    }
                    break;
                case "print":
                    if (args.Length < 2)
                    {
                        WriteColorLine("Invalid Syntax. Usage:  print  <matrixName>", ConsoleColor.Red);
                        break;
                    }
                    if (!matrices.ContainsKey(args[1]))
                    {
                        WriteColorLine("A matrix with that name does not exist.", ConsoleColor.Red);
                        break;
                    }
                    matrixName = args[1];
                    matrix = matrices[matrixName];
                    printMatrix(matrix, matrixName);
                    break;
                case "printcolumn":
                case "printrow":
                    if (args.Length < 3)
                    {
                        if (args[0].ToLower() == "printrow") WriteColorLine("Invalid Syntax. Usage:  printRow  <matrixName> <rowNum>", ConsoleColor.Red);
                        else if (args[0].ToLower() == "printcolumn") WriteColorLine("Invalid Syntax. Usage:  printColumn  <matrixName> <columnNum>", ConsoleColor.Red);
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
                    if (args[0].ToLower() == "printrow") printMatrixRow(matrix, num);
                    else if (args[0].ToLower() == "printcolumn") printMatrixColumn(matrix, num);
                    break;
                case "swaprows":
                case "swapcolumns":
                    if (args.Length < 3 || !args[2].Contains(","))
                    {
                        if (args[0].ToLower() == "swaprows") WriteColorLine("Invalid Syntax. Usage:  swapRows  <matrixName>  <row1>,<row2>", ConsoleColor.Red);
                        else if (args[0].ToLower() == "swapcolumns") WriteColorLine("Invalid Syntax. Usage:  swapColumns  <matrixName>  <col1>,<col2>", ConsoleColor.Red);
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
                    oldMatrix = (int[,])matrix.Clone(); matrix.Clone();//cloning the matrix now so that i can display the changes with printMatrixWithChanges()
                    matrix = (args[0].ToLower() == "swaprows") ? swapRows(matrix, swap, swapWith) : swapColumns(matrix, swap, swapWith);
                    printMatrixWithChanges(matrix, oldMatrix, matrixName);
                    matrices[matrixName] = matrix;
                    break;
                case "swapentries":
                    if (args.Length < 4 || !args[2].Contains(",") || !args[3].Contains(","))
                    {
                        WriteColorLine("Invalid Syntax. Usage:  swapEntries  <matrixName>  <row1>,<col1>  <row2>,<col2>", ConsoleColor.Red);
                        break;
                    }
                    if (!int.TryParse(args[2].Split(',')[0], out int x1)) { WriteColorLine("Invalid number.", ConsoleColor.Red); break; }
                    if (!int.TryParse(args[2].Split(',')[1], out int y1)) { WriteColorLine("Invalid number.", ConsoleColor.Red); break; }
                    if (!int.TryParse(args[3].Split(',')[0], out int x2)) { WriteColorLine("Invalid number.", ConsoleColor.Red); break; }
                    if (!int.TryParse(args[3].Split(',')[1], out int y2)) { WriteColorLine("Invalid number.", ConsoleColor.Red); break; }
                    x1--;x2--;y1--;y2--;
                    matrixName = args[1];
                    matrix = matrices[matrixName];
                    oldMatrix = (int[,])matrix.Clone();
                    matrix = swapTwoEntries(matrix, new int[] { x1, y1 }, new int[] { x2, y2 });
                    printMatrixWithChanges(matrix, oldMatrix, matrixName);
                    matrices[matrixName] = matrix;
                    break;
                case "mirrordiagonalsecondary":
                case "mirrordiagonalmain":
                    if (args.Length < 2)
                    {
                        if (args[0].ToLower() == "mirrordiagonalmain") WriteColorLine("Invalid Syntax. Usage:  mirrorDiagonalMain  <matrixName>", ConsoleColor.Red);
                        else if (args[0].ToLower() == "mirrordiagonalsecondary") WriteColorLine("Invalid Syntax. Usage:  mirrorDiagonalSecondary  <matrixName>", ConsoleColor.Red);
                        break;
                    }
                    if (!matrices.ContainsKey(args[1]))
                    {
                        WriteColorLine("A matrix with that name does not exist.", ConsoleColor.Red);
                        break;
                    }
                    matrixName = args[1];
                    matrix = matrices[matrixName];
                    oldMatrix = (int[,])matrix.Clone();//cloning the matrix now so that i can display the changes with printMatrixWithChanges()
                    matrix = (args[0].ToLower() == "mirrordiagonalmain") ? mirrorDiagonalMain(matrix) : mirrorDiagonalSecondary(matrix);
                    printMatrixWithChanges(matrix, oldMatrix, matrixName);
                    matrices[matrixName] = matrix;
                    break;
                case "transposemain":
                case "transposesecondary":
                    if (args.Length < 2)
                    {
                        if (args[0].ToLower() == "transposemain") WriteColorLine("Invalid Syntax. Usage:  transposeMain  <matrixName>", ConsoleColor.Red);
                        else if (args[0].ToLower() == "transposesecondary") WriteColorLine("Invalid Syntax. Usage:  transposeSecondary  <matrixName>", ConsoleColor.Red);
                        break;
                    }
                    if (!matrices.ContainsKey(args[1]))
                    {
                        WriteColorLine("A matrix with that name does not exist.", ConsoleColor.Red);
                        break;
                    }
                    matrixName = args[1];
                    matrix = matrices[matrixName];
                    oldMatrix = (int[,])matrix.Clone(); matrix.Clone();//cloning the matrix now so that i can display the changes with printMatrixWithChanges()
                    matrix = (args[0].ToLower() == "transposemain") ? transposeMain(matrix) : transposeSecondary(matrix);
                    printMatrixWithChanges(matrix, oldMatrix, matrixName);
                    matrices[matrixName] = matrix;
                    break;
                case "add":
                case "subtract":
                case "multiply":
                    if (args.Length < 3)
                    {
                        if (args[0].ToLower() == "add") WriteColorLine("Invalid Syntax. Usage:  add  <matrixName> <number/matrix2Name>", ConsoleColor.Red);
                        else if (args[0].ToLower() == "subtract") WriteColorLine("Invalid Syntax. Usage:  subtract  <matrixName> <number/matrix2Name>", ConsoleColor.Red);
                        else if (args[0].ToLower() == "multiply") WriteColorLine("Invalid Syntax. Usage:  multiply  <matrixName> <number/matrix2Name>", ConsoleColor.Red);
                        break;
                    }
                    if (!matrices.ContainsKey(args[1]))
                    {
                        WriteColorLine("A matrix with that name ("+ args[1]+") does not exist.", ConsoleColor.Red);
                        break;
                    }
                    matrixName = args[1];
                    matrix = matrices[matrixName];
                    if (int.TryParse(args[2], out int operationValue))
                    {
                        //Operation between a matrix and a number
                        oldMatrix = (int[,])matrix.Clone();
                        matrix = matrixAndNumberOperations(matrix, operationValue, args[0]);
                        printMatrixWithChanges(matrix, oldMatrix, matrixName);
                        matrices[matrixName] = matrix;
                    }
                    else
                    {
                        //Operation between two matrices
                        if (!matrices.ContainsKey(args[2]))
                        {
                            WriteColorLine("A matrix with that name (" + args[2] + ") does not exist.", ConsoleColor.Red);
                            break;
                        }
                        String matrixTwoName = args[2];
                        int[,] matrixTwo = matrices[matrixTwoName];

                        bool areSameSize = matrix.GetLength(0) == matrixTwo.GetLength(0) && matrix.GetLength(1) == matrixTwo.GetLength(1);
                        if ((args[0].ToLower() == "add" || args[0].ToLower() == "subtract") && !areSameSize)
                        {
                            WriteColorLine("You cant add/subtract these two matrices since they don't have the same size.", ConsoleColor.Red);
                            break;
                        }
                        if (args[0].ToLower() == "multiply" && (matrix.GetLength(1) != matrixTwo.GetLength(0)))
                        {
                            WriteColorLine("You cant multiply these two matrices since the #cols in the first matrix != #rows in the second.", ConsoleColor.Red);
                            break;
                        }

                        oldMatrix = (int[,])matrix.Clone();//cloning the matrix now so that i can display the changes with printMatrixWithChanges()
                        matrix = matrixOperations((int[,])matrix.Clone(), matrixTwo, args[0]);
                        printMatrixWithChanges((int[,])matrix.Clone(), oldMatrix, "result");
                        matrices["result"] = matrix;
                    }
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

        static void printInstructions()
        {

            string instructions =
                "Commands:\n" +
                "  create  <matrixName>  <rowNum>,<colNum>\n" +
                "  createRandom  <matrixName>  <rowNum>,<colNum>  <rndMin>,<rndMax>\n" +
                "  delete  <matrixName>\n" +
                "  listMatrices\n" +
                "  print  <matrixName>\n" +
                "  printRow  <matrixName>  <rowNum>\n" +
                "  printColumn  <matrixName>  <columnNum>\n" +
                "  swapRows  <matrixName>  <row1>,<row2>\n" +
                "  swapColumns  <matrixName>  <col1>,<col2>\n" +
                "  swapEntries  <matrixName>  <row1>,<col1>  <row2>,<col2>\n" +
                "  mirrorDiagonalMain  <matrixName>\n" +
                "  mirrorDiagonalSecondary  <matrixName>\n" +
                "  transposeMain  <matrixName>\n" +
                "  transposeSecondary  <matrixName>\n" +
                "  <add/subtract/multiply>  <matrixName>  <number>\n" +
                "  <add/subtract/multiply>  <matrixName>  <matrix2Name>\n" +
                "  clear";
            WriteColorLine(instructions, ConsoleColor.Cyan);
        }
        static int[,] createAndFillMatrix(int rows, int cols)//fills the matrix with 1,2,3,...
        {
            int[,] matrix = new int[rows, cols];
            int row = 0;
            Random rnd = new Random();
            for (int i = 1; i <= cols * rows; i++)
            {
                if ((i - 1) % cols == 0 && i != 1) row++;
                int col = i - 1 - row * cols;
                matrix[row, col] = i;

            }
            return matrix;
        }
        static int[,] createRandomMatrix(int rows, int cols, int rndMin, int rndMax)//fills the matrix with numbers between rndMin and rndMax (both inclusive)
        {
            int[,] matrix = new int[rows, cols];
            int row = 0;
            Random rnd = new Random();
            for (int i = 1; i <= cols * rows; i++)
            {
                if ((i - 1) % cols == 0 && i != 1) row++;
                int col = i - 1 - row * cols;
                matrix[row, col] = rnd.Next(rndMin, rndMax+1);

            }
            return matrix;
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
        static int getMaxMatrixValueLength(int[,] matrix)//returns the highest length entry from all matrix entries (for printing alignment)
        {
            int maxLength = Convert.ToString(matrix[0, 0]).Length;
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    maxLength = (Convert.ToString(matrix[row, col]).Trim().Length > maxLength) ? Convert.ToString(matrix[row, col]).Trim().Length : maxLength;

                }
            }
            return maxLength;
        }
        static void printMatrix(int[,] matrix, string matrixName)
        {
            matrixName += " = ";
            int middle = matrix.GetLength(0) / 2;//this will be the row at which the matrixName is shown
            string emptyName = new string(' ', matrixName.Length);//for alignment
            int maxMatrixValueLength = getMaxMatrixValueLength(matrix);//for alignment between entries
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                Console.Write(" " + ((row == middle) ? matrixName : emptyName));
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    String addToEnd = (col != matrix.GetLength(1) - 1) ? ", " : "";
                    String printEntry = Convert.ToString(matrix[row, col]);
                    int spacesNum = maxMatrixValueLength - printEntry.Length;
                    if (spacesNum == -1) spacesNum = 1;
                    String addSpaces = new string(' ', spacesNum);
                    Console.Write(printEntry + addToEnd + addSpaces);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
        static void printMatrixWithChanges(int[,] matrix, int[,] lastMatrix, string matrixName)//highlights changes with red
        {
            matrixName += " = ";
            int middle = matrix.GetLength(0) / 2;//this will be the row at which the matrixName is shown
            string emptyName = new string(' ', matrixName.Length);//for alignment
            int maxMatrixValueLength = getMaxMatrixValueLength(matrix);//for alignment between entries
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                Console.Write(" " + ((row == middle) ? matrixName : emptyName));
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    String addToEnd = (col != matrix.GetLength(1) - 1) ? ", " : "";
                    String printEntry = Convert.ToString(matrix[row, col]);
                    int spacesNum = maxMatrixValueLength - printEntry.Length;
                    String addSpaces = new string(' ', spacesNum);
                    try
                    {
                        //if the entries from the lastMatrix (matrix before last change) dont match, print with red color
                        if (matrix[row, col] == lastMatrix[row, col]) Console.Write(printEntry);
                        else WriteColor(printEntry, ConsoleColor.Red);
                    }
                    catch (Exception)
                    {
                        //lastMatrix and matrix have a different size
                        WriteColor(printEntry, ConsoleColor.Red);
                    }
                    Console.Write(addToEnd + addSpaces);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
        static void printMatrixRow(int[,] matrix, int row)// first row is 0
        {
            for (int col = 0; col < matrix.GetLength(1); col++)
            {
                if (row >= 0 && row < matrix.GetLength(0))
                {
                    Console.Write(matrix[row, col]);
                    if (col != matrix.GetLength(1) - 1) Console.Write(", ");
                    else Console.Write("\n\n");
                }
                else
                {
                    WriteColorLine("Out of bounds of the matrix.", ConsoleColor.Red);
                    return;
                }
            }
        }
        static void printMatrixColumn(int[,] matrix, int col)//first column is 0
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                if (col >= 0 && col < matrix.GetLength(1))
                {
                    Console.Write(matrix[row, col]);
                    if (row != matrix.GetLength(0) - 1) Console.Write(", ");
                    else Console.Write("\n\n");
                }
                else
                {
                    WriteColorLine("Out of bounds of the matrix.", ConsoleColor.Red);
                    return;
                }
            }
        }
        static int[,] transposeMain(int[,] matrix)
        {
            int[,] newMatrix = new int[matrix.GetLength(1), matrix.GetLength(0)];
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    newMatrix[col, row] = matrix[row, col];
                }
            }
            return newMatrix;
        }
        static int[,] transposeSecondary(int[,] matrix)
        {
            int originalRowsNum = matrix.GetLength(0);
            int originalColsNum = matrix.GetLength(1);
            int[,] newMatrix = new int[matrix.GetLength(1), matrix.GetLength(0)];
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    newMatrix[originalColsNum - 1 - col, originalRowsNum - 1 - row] = matrix[row, col];
                }
            }
            return newMatrix;
        }
        static int[,] swapRows(int[,] matrix, int swapAt, int swapWith)// first row is 0
        {
            for (int col = 0; col < matrix.GetLength(1); col++)
            {
                if (swapAt >= 0 && swapAt < matrix.GetLength(0) && swapWith >= 0 && swapWith < matrix.GetLength(0))
                {
                    int temp = matrix[swapAt, col];
                    matrix[swapAt, col] = matrix[swapWith, col];
                    matrix[swapWith, col] = temp;
                }
                else
                {
                    WriteColorLine("Out of bounds of the matrix.", ConsoleColor.Red);
                    return matrix;
                }
            }
            return matrix;
        }
        static int[,] swapColumns(int[,] matrix, int swapAt, int swapWith)//first column is 0
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                if (swapAt >= 0 && swapAt < matrix.GetLength(1) && swapWith >= 0 && swapWith < matrix.GetLength(1))
                {
                    int temp = matrix[row, swapAt];
                    matrix[row, swapAt] = matrix[row, swapWith];
                    matrix[row, swapWith] = temp;
                }
                else
                {
                    WriteColorLine("Out of bounds of the matrix.", ConsoleColor.Red);
                    return matrix;
                }
            }
            return matrix;
        }
        static int[,] swapTwoEntries(int[,] matrix, int[] swapAt, int[] swapWith)
        {
            if (swapAt[0] < 0 || swapAt[1] < 0 || swapAt[1] >= matrix.GetLength(1) || swapAt[0] >= matrix.GetLength(0) ||
                swapWith[0] < 0 || swapWith[1] < 0 || swapWith[1] >= matrix.GetLength(1) || swapWith[0] >= matrix.GetLength(0))
            {
                WriteColorLine("Out of bounds of the matrix.", ConsoleColor.Red);
                return matrix;
            }
            int temp = matrix[swapAt[0], swapAt[1]];
            matrix[swapAt[0], swapAt[1]] = matrix[swapWith[0], swapWith[1]];
            matrix[swapWith[0], swapWith[1]] = temp;
            return matrix;
        }
        static int[,] mirrorDiagonalMain(int[,] matrix)//flips the entries in the main diagonal
        {
            int size = Math.Min(matrix.GetLength(0) - 1, matrix.GetLength(1) - 1);
            int middle = size / 2;
            for (int i = 0; i <= middle; i++)
            {
                int temp = matrix[i,i];
                matrix[i, i] = matrix[size-i,size-i];
                matrix[size - i, size - i] = temp;
            }
            return matrix;
        }
        static int[,] mirrorDiagonalSecondary(int[,] matrix)//flips the entries in the secondary diagonal
        {
            int size = Math.Min(matrix.GetLength(0) - 1, matrix.GetLength(1) - 1);
            int diff = Math.Max(matrix.GetLength(0) - 1, matrix.GetLength(1) - 1) - size;
            if (matrix.GetLength(1) < matrix.GetLength(0)) diff = 0;
            int middle = size / 2;
            for (int i = 0; i <= middle; i++)
            {
                int temp = matrix[i, size - i+diff];
                matrix[i, size - i + diff] = matrix[size - i, i + diff];
                matrix[size - i, i + diff] = temp;
            }
            return matrix;
        }
        static int[,] matrixAndNumberOperations(int[,] matrix, int number, String operationStr)//adds/subtracts/multiplies a value to all entries
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    if (operationStr == "add") matrix[row, col] += number;
                    if (operationStr == "subtract") matrix[row, col] -= number;
                    if (operationStr == "multiply") matrix[row, col] *= number;
                    if (operationStr == "divide") matrix[row, col] /= number;

                }
            }
            return matrix;
        }
        static int[,] matrixOperations(int[,] matrix, int[,] matrixTwo, String operationStr)//adds/subtracts/multiplies two matrices
        {
            if (operationStr == "add" || operationStr == "subtract")//assuming the two matrices are the same size
            {
                for (int row = 0; row < matrix.GetLength(0); row++)
                {
                    for (int col = 0; col < matrix.GetLength(1); col++)
                    {
                        if (operationStr == "add") matrix[row, col] += matrixTwo[row, col];
                        if (operationStr == "subtract") matrix[row, col] -= matrixTwo[row, col];
                    }
                }
                return matrix;
            }
            else if (operationStr == "multiply")
            {
                int[,] newMatrix = new int[matrix.GetLength(0), matrixTwo.GetLength(1)];
                for (int r1 = 0; r1 < newMatrix.GetLength(0); r1++)
                {
                    for (int c1 = 0; c1 < newMatrix.GetLength(1); c1++)
                    {
                        int finalValue = 0;
                        for (int c2 = 0; c2 < matrix.GetLength(1); c2++)
                        {
                            finalValue += matrix[r1, c2] * matrixTwo[c2, c1];
                        }
                        newMatrix[r1, c1] = finalValue;
                    }
                }
                return newMatrix;
            }
            return matrix;
        }
    }
}
