using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class Grid
    {
        public int Size = 10;
        private const char Water = '░';
        private const char Cursor = '█';
        private const char Ship = '█';
        private const char Hit = 'X';
        private const char Miss = 'O';
        private char[,] cells;
        private Random rand;
        private List<List<int[]>> ships = new List<List<int[]>>(); // Track all ships by their coordinates


        public Grid()
        {
            rand = new Random();
            InitializeGrid();
        }
        public void InitializeGrid()
        {
            cells = new char[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    cells[i, j] = Water;
                }
            }
        }
        public void Render(bool showShips = false, int offsetX = 0, int offsetY = 0)
        {
            Console.SetCursorPosition(offsetX, offsetY);
            RenderLetters();

            for (int i = 0; i < Size; i++)
            {
                Console.SetCursorPosition(offsetX, offsetY + i + 1);
                RenderNumbers(i+1);
                for (int j = 0; j < Size; j++)
                {
                    char cell = cells[i, j];
                    if (cell == Ship && !showShips)
                    {
                        cell = Water;
                    }

                    SetCellColor(cell);
                    Console.Write($"{cell}{cell}");
                    Console.ResetColor();
                }
            }
        }

        public void RenderInteractive(int cursorX = -1, int cursorY = -1, int shipLength = 0, bool horizontal = true, bool showShips = false, int offsetX = 0, int offsetY = 0, int selectedShot = -1)
        {
            Console.SetCursorPosition(offsetX, offsetY);
            RenderLetters();

            for (int i = 0; i < Size; i++)
            {
                Console.SetCursorPosition(offsetX, offsetY + i + 1);
                RenderNumbers(i+1);
                for (int j = 0; j < Size; j++)
                {
                    char cell = cells[i, j];

                    // Highlight ship placement if cursor is active
                    bool isCursor = false;
                    if (selectedShot > 0)
                    {
                        if (selectedShot == 1)
                        {
                            isCursor = i == cursorX && j == cursorY;
                        }
                        else if (selectedShot == 2)
                        {
                            isCursor = Math.Abs(i - cursorX) <= 1 && Math.Abs(j - cursorY) <= 1;
                        }
                        else if (selectedShot == 3)
                        {
                            if (horizontal)
                            {
                                isCursor = i == cursorX && Math.Abs(j - cursorY) <= 1;
                            }
                            else
                            {
                                isCursor = Math.Abs(i - cursorX) <= 1 && j == cursorY;
                            }
                        }
                    }
                    bool isHighlighted = cursorX >= 0 && cursorY >= 0 && shipLength > 0 &&
                        ((horizontal && i == cursorX && j >= cursorY && j < cursorY + shipLength) ||
                         (!horizontal && j == cursorY && i >= cursorX && i < cursorX + shipLength));

                    if (isHighlighted)
                    {
                        if (IsValidPlacement(cursorX, cursorY, shipLength, horizontal))
                            Console.ForegroundColor = ConsoleColor.Green; // Valid placement
                        else
                            Console.ForegroundColor = ConsoleColor.Red; // Invalid placement
                        Console.Write("██");
                        Console.ResetColor();
                        continue;
                    }

                    if (cell == Ship && !showShips)
                    {
                        cell = Water;
                    }

                    SetCellColor(cell);

                    if (isCursor && shipLength == -1)
                    {
                        // Only Cursor
                        cell = Cursor;
                        if (!IsShot(i, j))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                    }
                    Console.Write($"{cell}{cell}");
                    Console.ResetColor();
                }
            }
        }
        public void RenderLetters()
        {
            if (Size < 10) Console.Write("   ");
            else Console.Write("    ");

            for (int i = 0; i < Size; i++)
            {
                //Writes the i-th character
                Console.Write($"{(char)('A' + i)} ");
            }
            Console.WriteLine();
        }
        public void RenderNumbers(int i)
        {
            if (Size < 10)
            {
                Console.Write($" {i} ");
            }
            else
            {
                if (i < 10) Console.Write($"  {i} ");
                else Console.Write($" {i} ");
            }
        }
        public bool IsShot(int i, int j)
        {
            char cell = cells[i, j];
            return cell == Hit || cell == Miss;
        }

        private void SetCellColor(char cell)
        {
            switch (cell)
            {
                case Hit:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Miss:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case Ship:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
            }
        }
        public bool PlaceShip(int x, int y, int length, bool horizontal)
        {
            if (!IsValidPlacement(x, y, length, horizontal)) return false;

            List<int[]> shipCoordinates = new List<int[]>();
            for (int i = 0; i < length; i++)
            {
                int shipX = x + (horizontal ? 0 : i);
                int shipY = y + (horizontal ? i : 0);
                cells[shipX, shipY] = Ship;
                shipCoordinates.Add(new int[] { shipX, shipY });
            }
            ships.Add(shipCoordinates);
            return true;
        }

        public bool IsValidPlacement(int x, int y, int length, bool horizontal)
        {
            for (int i = 0; i < length; i++)
            {
                int placementX = x + (horizontal ? 0 : i);
                int placementY = y + (horizontal ? i : 0);

                // Ensure its within bounds of grid
                if (placementX < 0 || placementX >= Size || placementY < 0 || 
                    placementY >= Size || cells[placementX, placementY] != Water)
                {
                    return false;
                }

                // Save neighbors as List of KeyValuePairs, SINCE I CANT USE A DICT because of same keys lol
                List<KeyValuePair<int, int>> neighbors = new List<KeyValuePair<int, int>>()
                {
                    {new KeyValuePair<int, int>(1, 0)},
                    {new KeyValuePair<int, int>(-1, 0)},
                    {new KeyValuePair<int, int>(0, 1)},
                    {new KeyValuePair<int, int>(0, -1)}
                };
                // Checking neighbors
                foreach (KeyValuePair<int, int> entry in neighbors)
                {
                    int adjX = placementX + entry.Key, adjY = placementY + entry.Value;
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
        public bool ShootArea(int centerX, int centerY, int width, int height)
        {
            bool hit = false;
            for (int x = Math.Max(0, centerX - height / 2); x <= Math.Min(Size - 1, centerX + height / 2); x++)
            {
                for (int y = Math.Max(0, centerY - width / 2); y <= Math.Min(Size - 1, centerY + width / 2); y++)
                {
                    if (!IsShot(x, y) && Shoot(x, y))
                    {
                        hit = true;
                    }
                }
            }
            return hit;
        }

        public bool AllShipsSunk()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (cells[i, j] == Ship) return false;
                }
            }

            return true;
        }

        public void RandomPlacement(int[] shipSizes)
        {
            int maxIterations = Size * Size * Size;
            bool maxReached = false;
            foreach (int size in shipSizes)
            {
                if (maxReached) break;
                int currentIter = 0;
                bool placed = false;
                while (!placed)
                {
                    currentIter++;
                    // Since it's possible to choose ship locations in games with a low grid size, we set a maxIterations variable,
                    //  if we then reach that number, we throw away the current ship positions and start over.
                    if (currentIter > maxIterations)
                    {
                        maxReached = true;
                        break;
                    }
                    int x = rand.Next(Size);
                    int y = rand.Next(Size);
                    bool horizontal = rand.Next(2) == 0;
                    placed = PlaceShip(x, y, size, horizontal); // Only proceed if the placement succeeds
                }
            }
            if (maxReached)
            {
                //Reset all ship locations and start over.
                InitializeGrid();
                RandomPlacement(shipSizes);
            }
        }
        public void SetSize(int size)
        {
            Size = size;
            InitializeGrid();
        }
    }
}