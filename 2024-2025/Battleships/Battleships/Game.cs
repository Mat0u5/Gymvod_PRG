using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            player = new Player("Player", false, null);

            player.PlaceShips();

            player.opponent.PlaceShips();

            Console.WriteLine("\nStarting the game!");
            playerTurn = true;

            // Main game loop
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Your Board (with your ships):");
                player.Grid.Render(true);
                Console.WriteLine("\nYour Shooting Board (opponent's view):");
                player.opponent.Grid.Render();

                if (playerTurn)
                {
                    Console.WriteLine("\nYour turn! Use arrow keys and Enter to shoot.");
                    if (player.TakeTurn())
                    {
                        Console.WriteLine("You won! All enemy ships have been sunk!");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("\nAI's turn...");
                    if (player.opponent.TakeTurnAI())
                    {
                        Console.WriteLine("You lost! All your ships have been sunk!");
                        break;
                    }
                }

                playerTurn = !playerTurn;
            }
        }

    }
}
