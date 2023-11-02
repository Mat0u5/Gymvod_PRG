using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;
using System.Data.Common;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2023-2024.
 * Extended by students.
 */

namespace _2D_Array_Playground
{
    internal class Program
    {
        static int[,] lastArray = new int[0,0];
        static void Main(string[] args)
        {
            //TODO 1: Vytvoř integerové 2D pole velikosti 5 x 5, naplň ho čísly od 1 do 25 a vypiš ho do konzole (5 řádků po 5 číslech).
            int[ , ] array = createAndFillArray(2,5);
            lastArray = (int[,]) array.Clone();
            //TODO 2: Vypiš do konzole n-tý řádek pole, kde n určuje proměnná nRow.
            printArrayRow(array, 0);
            //TODO 3: Vypiš do konzole n-tý sloupec pole, kde n určuje proměnná nColumn.
            printArrayColumn(array, 0);
            //TODO 4: Prohoď prvek na souřadnicích [xFirst, yFirst] s prvkem na souřadnicích [xSecond, ySecond] a vypiš celé pole do konzole po prohození.
            array = swapTwoElements(array, new int[]{0,1}, new int[] {1,1});
            printArray(array, true);
            //TODO 5: Prohoď n-tý řádek v poli s m-tým řádkem (n je dáno proměnnou nRowSwap, m mRowSwap) a vypiš celé pole do konzole po prohození.
            array = swapRows(array,0,1);
            printArray(array, true);
            //TODO 6: Prohoď n-tý sloupec v poli s m-tým sloupcem (n je dáno proměnnou nColSwap, m mColSwap) a vypiš celé pole do konzole po prohození.
            array = swapColumns(array, 0, 1);
            printArray(array, true);
            //TODO 7: Otoč pořadí prvků na hlavní diagonále (z levého horního rohu do pravého dolního rohu) a vypiš celé pole do konzole po otočení.
            array = transposeMain(array);
            printArray(array, true);

            //TODO 8: Otoč pořadí prvků na vedlejší diagonále (z pravého horního rohu do levého dolního rohu) a vypiš celé pole do konzole po otočení.
            array = transposeSecondary(array);
            printArray(array, true);

            Console.ReadKey();
        }
        static int[,] createAndFillArray(int width, int height)
        {
            int[,] array = new int[height, width];
            int row = 0;
            for (int i = 1; i <= width * height; i++)
            {
                if ((i - 1) % width == 0 && i != 1) row++;
                int col = i - 1 - row * width;
                array[row, col] = i;
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
        static void printArray(int[,] array, bool showChanges)
        {
            for (int row = 0; row < array.GetLength(0); row++)
            {
                Console.Write("    ");
                for (int col = 0; col < array.GetLength(1); col++)
                {
                    String addToEnd = (col != array.GetLength(1) - 1) ? ", " : "";
                    String printElement = Convert.ToString(array[row, col]) + addToEnd;
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
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
            lastArray = (int[,])array.Clone();
        }
        static void printArrayRow(int[,] array, int row)
        {
            for (int col = 0; col < array.GetLength(1); col++)
            {
                Console.Write(array[row, col]);
                if (col != array.GetLength(1) - 1) Console.Write(", ");
                else Console.Write("\n\n");
            }
        }
        static void printArrayColumn(int[,] array, int col)
        {
            for (int row = 0; row < array.GetLength(0); row++)
            {
                Console.Write(array[row, col]);
                if (row != array.GetLength(0) - 1) Console.Write(", ");
                else Console.Write("\n\n");
            }
        }
        static int[,] transposeMain(int[,] array)
        {
            int[,] newArray = new int[array.GetLength(1), array.GetLength(0)];
            for (int row = 0; row < array.GetLength(0); row++)
            {
                for (int col = 0; col < array.GetLength(1); col++)
                {
                    newArray[col,row] = array[row,col];
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
                    newArray[originalColsNum - 1 - col,originalRowsNum - 1-row] = array[row, col];
                }
            }
            return newArray;
        }
        static int[,] swapRows(int[,] array, int swapAt, int swapWith)
        {
            for (int col = 0; col < array.GetLength(1); col++)
            {
                int temp = array[swapAt,col];
                array[swapAt,col] = array[swapWith,col];
                array[swapWith,col] = temp;
            }
            return array;
        }
        static int[,] swapColumns(int[,] array, int swapAt, int swapWith)
        {
            for (int row = 0; row < array.GetLength(0); row++)
            {
                int temp = array[row, swapAt];
                array[row, swapAt] = array[row, swapWith];
                array[row, swapWith] = temp;
            }
            return array;
        }
        static int[,] swapTwoElements(int[,] array, int[] swapAt, int[] swapWith)
        {
            int temp = array[swapAt[1],swapAt[0]];
            array[swapAt[1],swapAt[0]] = array[swapWith[1],swapWith[0]];
            array[swapWith[1],swapWith[0]] = temp;
            return array;
        }
    }
}
