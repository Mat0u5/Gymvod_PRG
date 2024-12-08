using System;

namespace Battleships
{
    internal class Game
    {
        private Player player;
        private bool playerTurn;

        public void Start()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Battleships!");
            Console.WriteLine("Use arrow keys to navigate, Enter to confirm actions.");
            Console.WriteLine("Press enter to continue.");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Console.WriteLine("Press enter to continue.");
            }

            // Let the player select the grid size
            int gridSize = GetDimensionInput();
            // Place ships
            player = new Player("Player", false, null);
            player.Grid.SetSize(gridSize);
            player.opponent.Grid.SetSize(gridSize);
            player.PlaceShips();
            player.opponent.PlaceShips();

            // Info & Render both boards
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Your Board (AI's POV):");
            Console.SetCursorPosition(0, 14);
            Console.WriteLine("Opponent's Board:");
            Console.WriteLine("Use arrow keys to move, Enter to shoot.");
            player.Grid.Render(true, 0, 2); // Render player's grid

            // Main game loop
            playerTurn = true;
            while (true)
            {
                if (playerTurn)
                {
                    if (player.TakeTurn())
                    {
                        //TakeTurn() returns true if all enemy ships are sunk
                        Console.SetCursorPosition(0, 26);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n\nYou won! All enemy ships have been sunk!");
                        break;
                    }
                }
                else
                {
                    if (player.opponent.TakeTurnAI())
                    {
                        //TakeTurn() returns true if all enemy ships are sunk
                        Console.SetCursorPosition(0, 26);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\nYou lost! All your ships have been sunk!");
                        break;
                    }
                }

                playerTurn = !playerTurn;
            }
            Console.ResetColor();
            Console.WriteLine("\nPress SPACEBAR to start a new game.");
            while (Console.ReadKey().Key != ConsoleKey.Spacebar);
        }
        public int GetDimensionInput()
        {
            Console.WriteLine("Type out the your desired size (square of length):");
            while (true)
            {
                if (Int32.TryParse(Console.ReadLine(), out int size))
                {
                    //Make sure its within bounds
                    if (size < 6 || size > 50)
                    {
                        Console.WriteLine("The size must be between 6 and 50");
                        continue;
                    }
                    return size;
                }
                Console.WriteLine("Invalid input. Try again.");
            }
        }
    }
}
