using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2023-2024.
 * Extended by students.
 */

namespace _2D_Array_Playground
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //TODO 1: Vytvoř integerové 2D pole velikosti 5 x 5, naplň ho čísly od 1 do 25 a vypiš ho do konzole (5 řádků po 5 číslech).
            int[][] array = new int[5][] {new int[5], new int[5], new int[5], new int[5], new int[5]};
            int column = 0;
            for (int i = 1; i <= 25; i++) 
            {
                if ((i-1) % 5 == 0 && i != 1) column++;
                int row = i - 1 - column * 5;
                array[column][row] = i;
            }

            //TODO 2: Vypiš do konzole n-tý řádek pole, kde n určuje proměnná nRow.
            int nRow = 0;
            Console.WriteLine(String.Join(", ", array[nRow]));

            //TODO 3: Vypiš do konzole n-tý sloupec pole, kde n určuje proměnná nColumn.
            int nColumn = 0;
            for (int i = 0; i < 5; i++)
            {
                Console.Write(array[i][nColumn]);
                if (i != 4) Console.Write(", ");
                else Console.Write("\n\n");
            }

            //TODO 4: Prohoď prvek na souřadnicích [xFirst, yFirst] s prvkem na souřadnicích [xSecond, ySecond] a vypiš celé pole do konzole po prohození.
            //Nápověda: Budeš potřebovat proměnnou navíc, do které si uložíš první z prvků před tím, než ho přepíšeš druhým, abys hodnotou prvního prvku potom mohl přepsat druhý
            int xFirst = 0, yFirst= 1, xSecond = 4, ySecond = 4;
            int temp = array[yFirst][xFirst];
            array[yFirst][xFirst] = array[ySecond][xSecond];
            array[ySecond][xSecond] = temp;
            for (int i = 0; i <5; i++)  Console.WriteLine(String.Join(", ", array[i]));
            Console.WriteLine("");
            //TODO 5: Prohoď n-tý řádek v poli s m-tým řádkem (n je dáno proměnnou nRowSwap, m mRowSwap) a vypiš celé pole do konzole po prohození.
            int nRowSwap = 0;
            int mRowSwap = 1;
            int[] tempRowSwap = array[nRowSwap];
            array[nRowSwap] = array[mRowSwap];
            array[mRowSwap] = tempRowSwap;
            for (int i = 0; i < 5; i++) Console.WriteLine(String.Join(", ", array[i]));
            Console.WriteLine("");

            //TODO 6: Prohoď n-tý sloupec v poli s m-tým sloupcem (n je dáno proměnnou nColSwap, m mColSwap) a vypiš celé pole do konzole po prohození.
            int nColSwap = 0;
            int mColSwap = 1;
            for (int i = 0; i < 5; i++)
            {
                int tempColSwap = array[i][nColSwap];
                array[i][nColSwap] = array[i][mColSwap];
                array[i][mColSwap] = tempColSwap;
            }
            for (int i = 0; i < 5; i++) Console.WriteLine(String.Join(", ", array[i]));
            Console.WriteLine("");

            //TODO 7: Otoč pořadí prvků na hlavní diagonále (z levého horního rohu do pravého dolního rohu) a vypiš celé pole do konzole po otočení.

            for (int col = 0; col < 5; col++)
            {
                for (int row = 0; row < 5; row++)
                {
                    if (col <= row) continue;
                    int tempNum = array[col][row];
                    array[col][row] = array[row][col];
                    array[row][col] = tempNum;
                }
            }
            for (int i = 0; i < 5; i++) Console.WriteLine(String.Join(", ", array[i]));
            Console.WriteLine("");
            //TODO 8: Otoč pořadí prvků na vedlejší diagonále (z pravého horního rohu do levého dolního rohu) a vypiš celé pole do konzole po otočení.
            List<int> list = new List<int>() {0,1,2,3};
            int swapRow = 0;
            while (list.Count > 0)
            {
                foreach (int swapColumn in list)
                {
                    int maxIndex = (swapColumn > swapRow) ? swapColumn : swapRow;
                    int moveBy = 4 - maxIndex;
                    int tempMove = array[swapRow][swapColumn];
                    array[swapRow][swapColumn] = array[swapRow+ moveBy][swapColumn+ moveBy];
                    array[swapRow + moveBy][swapColumn + moveBy] = tempMove;
                }
                list.RemoveAt(list.Count-1);
                swapRow++;
            }
            for (int i = 0; i < 5; i++) Console.WriteLine(String.Join(", ", array[i]));
            Console.WriteLine("");

            Console.ReadKey();
        }
    }
}
