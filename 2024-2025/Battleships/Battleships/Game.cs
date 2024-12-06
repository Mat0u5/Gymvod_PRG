using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            playerTurn = true;

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Your Board (with your ships):");
            Console.SetCursorPosition(0, 14);
            Console.WriteLine("Opponent's Board (your shooting):");
            Console.WriteLine("Use arrow keys to move, Enter to shoot.");
            player.Grid.Render(true, 0, 2); // Render player's grid

            // Main game loop
            while (true)
            {
                if (playerTurn)
                {
                    if (player.TakeTurn())
                    {
                        Console.WriteLine("\n\nYou won! All enemy ships have been sunk!");
                        break;
                    }
                }
                else
                {
                    if (player.opponent.TakeTurnAI())
                    {
                        Console.WriteLine("\n\nYou lost! All your ships have been sunk!");
                        break;
                    }
                }

                playerTurn = !playerTurn;
            }
            Console.ReadKey();
        }

    }
}
