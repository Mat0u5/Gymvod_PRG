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
            //array = transposeMain(array);
            //printArray(array, true);

            //TODO 8: Otoč pořadí prvků na vedlejší diagonále (z pravého horního rohu do levého dolního rohu) a vypiš celé pole do konzole po otočení.
            //array = transposeSecondary(array);
            //printArray(array, true);

            Console.ReadKey();
        }
        static int[,] createAndFillArray(int width, int height)
        {
            int[,] array = new int[height, width];
            int column = 0;
            for (int i = 1; i <= width * height; i++)
            {
                if ((i - 1) % width == 0 && i != 1) column++;
                int row = i - 1 - column * width;
                array[column, row] = i;
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
            for (int i = 0; i < array.GetLength(0); i++)
            {
                Console.Write("    ");
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    String addToEnd = (j != array.GetLength(1) - 1) ? ", " : "";
                    String printElement = Convert.ToString(array[i, j]) + addToEnd;
                    try
                    {
                        //if the elements from the lastArray (array before last change) dont match, print with red color
                        if (array[i, j] == lastArray[i, j]) Console.Write(printElement);
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
            for (int i = 0; i < array.GetLength(1); i++)
            {
                Console.Write(array[row, i]);
                if (i != array.GetLength(1) - 1) Console.Write(", ");
                else Console.Write("\n\n");
            }
        }
        static void printArrayColumn(int[,] array, int col)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                Console.Write(array[i, col]);
                if (i != array.GetLength(0) - 1) Console.Write(", ");
                else Console.Write("\n\n");
            }
        }
        static int[,] transposeMain(int[,] array)
        {
            for (int col = 0; col < 5; col++)
            {
                for (int row = 0; row < 5; row++)
                {
                    if (col <= row) continue;
                    int temp = array[col, row];
                    array[col, row] = array[row,col];
                    array[row, col] = temp;
                }
            }
            return array;
        }
        static int[,] transposeSecondary(int[,] array)
        {
            List<int> list = new List<int>() { 0, 1, 2, 3 };
            int row = 0;
            while (list.Count > 0)
            {
                foreach (int col in list)
                {
                    int maxIndex = (col > row) ? col : row;
                    int moveBy = 4 - maxIndex;
                    int tempMove = array[row, col];
                    array[row, col] = array[row + moveBy, col + moveBy];
                    array[row + moveBy, col + moveBy] = tempMove;
                }
                list.RemoveAt(list.Count - 1);
                row++;
            }
            return array;
        }
        static int[,] swapRows(int[,] array, int swapAt, int swapWith)
        {
            for (int i = 0; i < array.GetLength(1); i++)
            {
                int temp = array[swapAt,i];
                array[swapAt,i] = array[swapWith,i];
                array[swapWith,i] = temp;
            }
            return array;
        }
        static int[,] swapColumns(int[,] array, int swapAt, int swapWith)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                int temp = array[i, swapAt];
                array[i, swapAt] = array[i, swapWith];
                array[i, swapWith] = temp;
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
