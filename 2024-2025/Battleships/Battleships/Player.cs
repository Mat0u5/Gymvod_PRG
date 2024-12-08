using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class Player
    {
        // Class for the actual Player AND the AI (i was lazy ok)
        public string Name { get; }
        public Grid Grid { get; }
        public Player opponent;
        private bool isAI;
        private Random rand;
        public int cursorX = 0, cursorY = 0;
        private int difficulty = 1;

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
                // Ai places ships randomly
                Grid.RandomPlacement(shipSizes);
                return;
            }

            Console.Clear();
            Console.WriteLine("Place your ships:");
            Console.WriteLine($"Place a ship of sizes 5x1, 4x1, 3x1, 3x1, 2x1 (Use arrow keys, Spacebar to rotate, Enter to confirm):\n");

            foreach (int size in shipSizes)
            {
                // Iterate over all ships and place them interactively
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
            // Placing ships at the beginning of the game.
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
            // Players turn
            ConsoleKey key;

            do
            {
                opponent.Grid.RenderInteractive(cursorX, cursorY, -1, true, false, 0, Grid.Size+6); // Render opponent's grid
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
            if (difficulty == 1) return AITurnEasy();
            else if (difficulty == 2) return AITurnNormal();
            else if (difficulty == 3) return AITurnHard();
            return false;
        }
        public bool AITurnEasy()
        {
            // Random shooting mode
            do
            {
                // Choose random position until you find one that has not been shot yet.
                cursorX = rand.Next(opponent.Grid.Size);
                cursorY = rand.Next(opponent.Grid.Size);
            } while (opponent.Grid.IsShot(cursorX, cursorY));

            opponent.Grid.Shoot(cursorX, cursorY);
            opponent.Grid.Render(true, 0, 2); // Render player's grid
            return opponent.Grid.AllShipsSunk();
        }
        private List<int[]> targets = new List<int[]>(); // For keeping track of potential targets around a hit
        private bool huntingMode = false; // Determines if the AI is currently hunting a ship after a hit
        public bool AITurnNormal()
        {
            int x, y;

            if (targets.Count == 0)
            {
                // No targets to hunt, return to random shots
                huntingMode = false;
            }

            if (!huntingMode)
            {
                // Random shooting mode
                do
                {
                    x = rand.Next(opponent.Grid.Size);
                    y = rand.Next(opponent.Grid.Size);
                } while (opponent.Grid.IsShot(x, y));

                if (opponent.Grid.Shoot(x, y))
                {
                    // If a ship is hit, switch to hunting mode and add adjacent cells to targets
                    huntingMode = true;
                    AddAdjacentTargets(x, y);
                }
            }
            else
            {
                // Hunting mode: focus on adjacent targets
                x = targets[0][0];
                y = targets[0][1];
                targets.RemoveAt(0);

                if (opponent.Grid.IsShot(x,y))
                {
                    return AITurnNormal();
                }
                if (opponent.Grid.Shoot(x, y))
                {
                    // If the shot hits, add more adjacent cells for further hunting
                    AddAdjacentTargets(x, y);
                }
            }

            opponent.Grid.Render(true, 0, 2); // Render player's grid
            return opponent.Grid.AllShipsSunk();
        }
        private List<int[]> checkerboardTargets = new List<int[]>();

        public bool AITurnHard()
        {
            if (checkerboardTargets.Count == 0)
            {
                InitializeCheckerboardTargets();
            }

            int x, y;

            if (targets.Count > 0)
            {
                // Hunting mode: focus on adjacent targets
                x = targets[0][0];
                y = targets[0][1];
                targets.RemoveAt(0);
            }
            else
            {
                // Checkerboard mode: choose the next target
                x = checkerboardTargets[0][0];
                y = checkerboardTargets[0][1];
                checkerboardTargets.RemoveAt(0);
            }

            if (opponent.Grid.IsShot(x, y))
            {
                return AITurnHard();
            }
            if (opponent.Grid.Shoot(x, y))
            {
                // If a ship is hit, switch to hunting mode and add adjacent cells to targets
                AddAdjacentTargets(x, y);
            }

            opponent.Grid.Render(true, 0, 2); // Render player's grid
            return opponent.Grid.AllShipsSunk();
        }
        private void AddAdjacentTargets(int x, int y)
        {
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
                int newX = x + entry.Key;
                int newY = y + entry.Value;
                if (newX >= 0 && newX < opponent.Grid.Size && newY >= 0 && newY < opponent.Grid.Size && !opponent.Grid.IsShot(newX, newY))
                {
                    targets.Add(new int[] { newX, newY });
                }
            }
        }
        private void InitializeCheckerboardTargets()
        {
            checkerboardTargets.Clear();
            for (int i = 0; i < opponent.Grid.Size; i++)
            {
                for (int j = 0; j < opponent.Grid.Size; j++)
                {
                    // Only target cells in a checkerboard pattern
                    if ((i + j) % 2 == 0)
                    {
                        checkerboardTargets.Add(new int[] { i, j });
                    }
                }
            }
        }
        public void SetDifficulty(int diff)
        {
            difficulty = diff;
        }
    }
}
