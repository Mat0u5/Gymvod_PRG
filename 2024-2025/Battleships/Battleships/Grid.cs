using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class Grid
    {
        public const int Size = 10;
        private const char Water = '░';
        private const char Ship = '█';
        private const char Hit = 'X';
        private const char Miss = 'O';
        private char[,] cells;
        private Random rand;

        public Grid()
        {
            cells = new char[Size, Size];
            rand = new Random();

            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    cells[i, j] = Water;
        }
        public void Render(bool showShips = false)
        {
            Console.WriteLine("   0 1 2 3 4 5 6 7 8 9");
            string[] rowMarkers = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            for (int i = 0; i < Size; i++)
            {
                Console.Write($" {rowMarkers[i]} ");
                for (int j = 0; j < Size; j++)
                {
                    char cell = cells[i, j];
                    if (cell == Ship && !showShips)
                    {
                        cell = Water;
                    }

                    if (cell == Hit)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (cell == Miss)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (cell == Ship)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }

                    Console.Write($"{cell}{cell}");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        public void RenderInteractive(int cursorX = -1, int cursorY = -1, int shipLength = 0, bool horizontal = true, bool showShips = false)
        {
            Console.WriteLine("   0 1 2 3 4 5 6 7 8 9");
            string[] rowMarkers = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            for (int i = 0; i < Size; i++)
            {
                Console.Write($" {rowMarkers[i]} ");
                for (int j = 0; j < Size; j++)
                {
                    char cell = cells[i, j];

                    // Highlight ship placement if cursor is active
                    bool atCursor = i == cursorX && j == cursorY;
                    bool isHighlighted = false;
                    if (cursorX >= 0 && cursorY >= 0 && shipLength > 0 &&
                        ((horizontal && i == cursorX && j >= cursorY && j < cursorY + shipLength) || 
                        (!horizontal && j == cursorY && i >= cursorX && i < cursorX + shipLength)))
                    {
                        isHighlighted = true;
                        if (IsValidPlacement(cursorX, cursorY, shipLength, horizontal))
                        {
                            Console.ForegroundColor = ConsoleColor.Green; // Valid placement
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Invalid placement
                        }
                    }
                    else if (cell == Ship && !showShips)
                    {
                        cell = Water;
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (cell == Hit)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (cell == Miss)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (cell == Ship)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }

                    if (atCursor && shipLength == -1)
                    {
                        // No ship, just showing cursor
                        Console.ForegroundColor = ConsoleColor.Green;
                        cell = '█';
                    }
                    Console.Write(isHighlighted ? "██" : $"{cell}{cell}");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
        public bool PlaceShip(int x, int y, int length, bool horizontal)
        {
            if (!IsValidPlacement(x, y, length, horizontal)) return false;

            for (int i = 0; i < length; i++)
            {
                int nx = x + (horizontal ? 0 : i);
                int ny = y + (horizontal ? i : 0);
                cells[nx, ny] = Ship;
            }
            return true;
        }

        public bool IsValidPlacement(int x, int y, int length, bool horizontal)
        {
            for (int i = 0; i < length; i++)
            {
                int nx = x + (horizontal ? 0 : i);
                int ny = y + (horizontal ? i : 0);

                // Ensure within bounds
                if (nx < 0 || nx >= Size || ny < 0 || ny >= Size || cells[nx, ny] != Water)
                    return false;

                // Check orthogonal neighbors only (no diagonal checks)
                foreach (var (dx, dy) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
                {
                    int adjX = nx + dx, adjY = ny + dy;
                    if (adjX >= 0 && adjX < Size && adjY >= 0 && adjY < Size && cells[adjX, adjY] == Ship)
                        return false;
                }
            }
            return true;
        }


        public bool Shoot(int x, int y)
        {
            if (cells[x, y] == Water)
            {
                cells[x, y] = Miss;
                return false;
            }

            if (cells[x, y] == Ship)
            {
                cells[x, y] = Hit;
                return true;
            }

            return false;
        }

        public bool AllShipsSunk()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    if (cells[i, j] == Ship) return false;

            return true;
        }

        public void RandomPlacement(int[] shipSizes)
        {
            foreach (int size in shipSizes)
            {
                bool placed = false;
                while (!placed)
                {
                    int x = rand.Next(Size);
                    int y = rand.Next(Size);
                    bool horizontal = rand.Next(2) == 0;
                    placed = PlaceShip(x, y, size, horizontal); // Only proceed if the placement succeeds
                }
            }
        }

    }
}