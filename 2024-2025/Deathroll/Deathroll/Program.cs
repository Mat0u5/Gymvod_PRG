using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2024-2025.
 * Extended by students.
 */

namespace Deathroll
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            int gold = 1000;
            while(gold > 0)
            {
                Console.WriteLine("Your balance is " + gold + ".");
                int bet = -1;
                while (bet == -1)
                {
                    Console.WriteLine("Enter your bet.");
                    if (!Int32.TryParse(Console.ReadLine(), out int newBet)) continue;
                    if (newBet > gold) Console.WriteLine("You're too broke for that");
                    else bet = newBet;
                }
                int round = 0;
                int newRoll = bet;
                while (newRoll > 1)
                {
                    round++;
                    newRoll = rnd.Next(1, newRoll);
                    if (round % 2 != 0)
                    {
                        Console.WriteLine("\nPress any key to roll.");
                        Console.ReadKey();
                        Console.WriteLine("You rolled " + newRoll);
                        if (newRoll > 1) continue;
                        Console.WriteLine("You lost " + bet + " gold.");
                        gold -= bet;
                        break;
                    }

                    Console.WriteLine("The computer rolled " + newRoll);
                    if (newRoll > 1) continue;
                    Console.WriteLine("You win " + bet + " gold.");
                    gold += bet;
                }
            }
            Console.WriteLine("Hate to break it to yout but uhhh..... You don't have anything left. See ya later.");
            Console.ReadKey();
        }
    }
}