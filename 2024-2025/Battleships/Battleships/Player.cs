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

            foreach (int size in shipSizes)
            {
                bool placed = false;
                while (!placed)
                {
                    Console.Clear();
                    Grid.Render(true);

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
                Console.Clear();
                Console.WriteLine("Place your ships: \n");
                Console.WriteLine($"Place a ship of size {shipLength}x1 (Use arrow keys, Spacebar to rotate, Enter to confirm):");
                Grid.RenderInteractive(cursorX, cursorY, shipLength, horizontal, true);

                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow: cursorX = Math.Max(0, cursorX - 1); break;
                    case ConsoleKey.DownArrow: cursorX = Math.Min(Grid.Size - 1, cursorX + 1); break;
                    case ConsoleKey.LeftArrow: cursorY = Math.Max(0, cursorY - 1); break;
                    case ConsoleKey.RightArrow: cursorY = Math.Min(Grid.Size - 1, cursorY + 1); break;
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
                Console.Clear();
                Console.WriteLine("Your Board (with your ships):");
                Grid.Render(true); // Player's board with ships
                Console.WriteLine("\nOpponent's Board (your shooting):");
                opponent.Grid.RenderInteractive(cursorX, cursorY, -1, true, false); // Highlight only on the opponent's board

                Console.WriteLine("\nUse arrow keys to move, Enter to shoot.");

                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow: cursorX = Math.Max(0, cursorX - 1); break;
                    case ConsoleKey.DownArrow: cursorX = Math.Min(Grid.Size - 1, cursorX + 1); break;
                    case ConsoleKey.LeftArrow: cursorY = Math.Max(0, cursorY - 1); break;
                    case ConsoleKey.RightArrow: cursorY = Math.Min(Grid.Size - 1, cursorY + 1); break;
                    case ConsoleKey.Enter: opponent.Grid.Shoot(cursorX, cursorY);
                    return opponent.Grid.AllShipsSunk();
                }
            } while (key != ConsoleKey.Escape);

            return false;
        }
        public bool TakeTurnAI()
        {
            int cursorX = rand.Next(10);
            int cursorY = rand.Next(10);
            opponent.Grid.Shoot(cursorX, cursorY);
            return opponent.Grid.AllShipsSunk();
        }



    }
}
