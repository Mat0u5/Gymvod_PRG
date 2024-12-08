using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class Player
    {
        public string Name { get; }
        public Grid Grid { get; }
        public Player opponent;
        private bool isAI;
        private Random rand;
        public int cursorX = 0, cursorY = 0;

        public Player(string name, bool isAI, Player opponent)
        {
            Name = name;
            this.isAI = isAI;
            Grid = new Grid();
            if (!isAI)
            {
                this.opponent = new Player("AI", true,  this);
            }
            else
            {
                this.opponent = opponent;
            }
            rand = new Random();
        }

        public void PlaceShips()
        {
            int[] shipSizes = { 5, 4, 3, 3, 2 };
            if (isAI)
            {
                Grid.RandomPlacement(shipSizes);
                return;
            }

            Console.Clear();
            Console.WriteLine("Place your ships:");
            Console.WriteLine($"Place a ship of sizes 5x1, 4x1, 3x1, 3x1, 2x1 (Use arrow keys, Spacebar to rotate, Enter to confirm):\n");

            foreach (int size in shipSizes)
            {
                bool placed = false;
                while (!placed)
                {
                    placed = PlaceShipInteractively(size);
                }
            }
            cursorX = 0;
            cursorY = 0;
        }

        public bool PlaceShipInteractively(int shipLength)
        {
            bool horizontal = true;
            ConsoleKey key;

            do
            {
                //Clamp the cursor+ship position so that its within the grid.
                cursorX = Math.Max(0, cursorX);
                cursorX = Math.Min(Grid.Size - (horizontal ? 1 : shipLength), cursorX);
                cursorY = Math.Max(0, cursorY);
                cursorY = Math.Min(Grid.Size - (horizontal ? shipLength : 1), cursorY);

                Grid.RenderInteractive(cursorX, cursorY, shipLength, horizontal, true, 0, 3);

                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    // Movement, placing conformation.
                    case ConsoleKey.UpArrow: cursorX = cursorX - 1; break;
                    case ConsoleKey.DownArrow: cursorX = cursorX + 1; break;
                    case ConsoleKey.LeftArrow: cursorY = cursorY - 1; break;
                    case ConsoleKey.RightArrow: cursorY = cursorY + 1; break;
                    case ConsoleKey.Spacebar: horizontal = !horizontal; break;
                    case ConsoleKey.Enter:
                        if (Grid.PlaceShip(cursorX, cursorY, shipLength, horizontal))
                            return true;
                        break;
                }
            } while (key != ConsoleKey.Escape);

            return false;
        }

        public bool TakeTurn()
        {
            ConsoleKey key;

            do
            {
                opponent.Grid.RenderInteractive(cursorX, cursorY, -1, true, false, 0, 16); // Render opponent's grid
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    // Movement, shooting conformation.
                    case ConsoleKey.UpArrow: cursorX = Math.Max(0, cursorX - 1); break;
                    case ConsoleKey.DownArrow: cursorX = Math.Min(Grid.Size - 1, cursorX + 1); break;
                    case ConsoleKey.LeftArrow: cursorY = Math.Max(0, cursorY - 1); break;
                    case ConsoleKey.RightArrow: cursorY = Math.Min(Grid.Size - 1, cursorY + 1); break;
                    case ConsoleKey.Enter:
                        if (opponent.Grid.IsShot(cursorX, cursorY)) break;
                        opponent.Grid.Shoot(cursorX, cursorY);
                        return opponent.Grid.AllShipsSunk();
                }
            } while (key != ConsoleKey.Escape);

            return false;
        }
        public bool TakeTurnAI()
        {
            int cursorX = 0;
            int cursorY = 0;
            do
            {
                // Choose random position until you find one that has not been shot yet.
                cursorX = rand.Next(10);
                cursorY = rand.Next(10);
            } while (opponent.Grid.IsShot(cursorX, cursorY));
            opponent.Grid.Shoot(cursorX, cursorY);
            opponent.Grid.Render(true, 0, 2); // Render player's grid
            return opponent.Grid.AllShipsSunk();
        }
    }
}
