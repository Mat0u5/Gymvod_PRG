using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2023-2024.
 * Extended by students.
 */

namespace Deathroll
{
    internal class Program
    {
        static int coins = 1000;
        static int coinsBet = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("How many coins do you want to bet?  (You currently have {0} coins)", Convert.ToString(coins));
            while (true)
            {
                //check if input (amount of coins you want to bet) is a number
                if (int.TryParse(Console.ReadLine(), out coinsBet) != true)
                {
                    Console.WriteLine("That's not a number. Try again.");
                    continue;
                }
                //check you have enough coins to bet that amount
                if (coinsBet > coins)
                {
                    Console.WriteLine("You don't have that many coins. Try again.");
                    continue;
                }
                //play the game
                gameLoop();
                //if you have 0 coins, the game ends, else you can play again
                if (coins == 0)
                {
                    Console.WriteLine("You can't bet anymore, you have 0 coins!");
                    break;
                }
                else
                {
                    Console.WriteLine("How many coins do you want to bet?  (You currently have {0} coins)", Convert.ToString(coins));
                }
            }
            Console.ReadKey();
        }
        static void gameLoop()
        {
            Console.WriteLine("Start rolling! (Your bet: {0} coins" +
                ")",Convert.ToString(coinsBet));
            Random rnd = new Random();
            while (true)
            {
                String input = Console.ReadLine().Replace("/roll ", "").Replace("roll ","");
                //check if the input is a number
                if (!int.TryParse(input, out int upperBound)) continue;

                //the player rolls from 1 to the input number
                int playerRoll = rnd.Next(1, upperBound);
                //the computer rolls from 1 to the player roll number
                int computerRoll = rnd.Next(1, playerRoll);
                Console.WriteLine("You rolled " + playerRoll + " (1-" + upperBound + ")");
                if (playerRoll == 1)
                {
                    //if the player rolls 1, the game end and the player loses
                    Console.WriteLine("You Lost! (- {0}coins)\n", Convert.ToString(coinsBet));
                    coins -= coinsBet;
                    break;
                }
                Console.WriteLine("Computer rolled " + computerRoll + " (1-" + playerRoll + ")");
                if (computerRoll == 1)
                {
                    //if the computer rolls 1, the game end and the player wins
                    Console.WriteLine("You Won! (+ {0} coins)\n", Convert.ToString(coinsBet));
                    coins += coinsBet;
                    break;
                }
                //if noone rolls 1, the game waits for another player input
            }
        }
    }
}
