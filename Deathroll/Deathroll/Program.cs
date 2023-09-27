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
        static void Main(string[] args)
        {
            while (true)
            {
                int roundCount = 0;
                String input = Console.ReadLine().Replace("/roll ", "").Replace("roll ","");
                if (int.TryParse(input, out int upperBound))
                {
                    Random rnd = new Random();
                    roundCount++;
                    int playerRoll = rnd.Next(1, upperBound);
                    int computerRoll = rnd.Next(1, playerRoll);
                    Console.WriteLine("You rolled " + playerRoll + " (1-" + upperBound + ")");
                    if (playerRoll == 1)
                    {
                        Console.WriteLine("You Lost!\n");
                        break;
                    }
                    Console.WriteLine("Computer rolled " + computerRoll + " (1-" + playerRoll + ")");
                    if (computerRoll == 1)
                    {
                        Console.WriteLine("You Won!\n");
                        break;
                    }

                }
            }
            Console.ReadKey();
        }
    }
}
