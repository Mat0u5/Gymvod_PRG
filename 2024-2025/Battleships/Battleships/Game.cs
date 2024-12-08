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
            Console.WriteLine("  ____        _   _   _           _     _           \r\n |  _ \\      | | | | | |         | |   (_)          \r\n | |_) | __ _| |_| |_| | ___  ___| |__  _ _ __  ___ \r\n |  _ < / _` | __| __| |/ _ \\/ __| '_ \\| | '_ \\/ __|\r\n | |_) | (_| | |_| |_| |  __/\\__ \\ | | | | |_) \\__ \\\r\n |____/ \\__,_|\\__|\\__|_|\\___||___/_| |_|_| .__/|___/\r\n                                         | |        \r\n                                         |_|        ");
            Console.WriteLine("\nUse arrow keys to navigate, press Enter to confirm actions.");
            Console.WriteLine("Press enter to continue.");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                Console.WriteLine("Press enter to continue.");
            }

            // Let the player select the grid size
            Console.Clear();
            Console.WriteLine("Type out the your desired grid length (the grid will be length x length), press Enter to confirm.");
            int gridSize = GetIntInput(6, 50);

            //Select opponent difficulty
            Console.Clear();
            Console.WriteLine("Select AI difficulty:");
            Console.WriteLine("1: Easy        (Random shooting)");
            Console.WriteLine("2: Normal      (Random shooting + tries to sink ships when it finds them)");
            Console.WriteLine("3: Hard        (Checkerboard strategy - might be worse than Normal diff, i don't know xD)");
            Console.WriteLine("\n Type out your number, press Enter to confirm.");
            int difficulty = GetIntInput(1, 3);


            // Place ships
            player = new Player("Player", false, null);
            player.opponent.SetDifficulty(difficulty);
            player.Grid.SetSize(gridSize);
            player.opponent.Grid.SetSize(gridSize);
            player.PlaceShips();
            player.opponent.PlaceShips();

            // Info & Render both boards
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Your Board (Opponent's POV):");
            Console.SetCursorPosition(0, gridSize+4);
            Console.WriteLine("Opponent's Board:");
            Console.WriteLine("Use arrow keys to move, press Enter to shoot.");
            player.Grid.Render(true, 0, 2); // Render player's grid
            player.opponent.RenderAIShotOptions();

            // Main game loop
            playerTurn = true;
            while (true)
            {
                if (playerTurn)
                {
                    if (player.TakeTurn())
                    {
                        //TakeTurn() returns true if all enemy ships are sunk
                        Console.SetCursorPosition(0, gridSize * 2 + 6);
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
                        Console.SetCursorPosition(0, gridSize * 2 + 6);
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
        public int GetIntInput(int min, int max)
        {
            while (true)
            {
                if (Int32.TryParse(Console.ReadLine(), out int num))
                {
                    //Make sure its within bounds
                    if (num < min || num > max)
                    {
                        Console.WriteLine("The number must be between "+ min+" and "+max);
                        continue;
                    }
                    return num;
                }
                Console.WriteLine("Invalid input. Try again.");
            }
        }
    }
}
