using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2024-2025.
 * Extended by students.
 */

namespace RockPaperScissors
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rng = new Random();
            Dictionary<String, String> rockPaperScissors = new Dictionary<String, String>
            {
                {"rock","scissors"},
                {"scissors","paper"},
                {"paper","rock"}
            };
            Console.WriteLine("How many rounds do you want to play?");
            int rounds = 0;
            int computerWins = 0;
            int playerWins = 0;
            while (!Int32.TryParse(Console.ReadLine(), out rounds))
            {
                Console.WriteLine("That is not a valid number. Try again.");
            }
            while(rounds > 0)
            {
                Console.WriteLine("\nRock paper scissors shoot!");
                String input = Console.ReadLine().ToLower();
                if (!rockPaperScissors.ContainsKey(input))
                {
                    Console.WriteLine("This input is not allowed");
                    continue;
                }
                String computerPlayed = rockPaperScissors.ElementAt(rng.Next(3)).Value;
                Console.Write("Computer played " + computerPlayed+" - ");
                if (rockPaperScissors[input].Equals(computerPlayed))
                {
                    Console.WriteLine("you get a point!");
                    playerWins++;
                }
                else if (rockPaperScissors[computerPlayed].Equals(input))
                {
                    Console.WriteLine("it gets a point!");
                    computerWins++;
                }
                rounds--;
            }
            Console.WriteLine((playerWins > computerWins) ? "\n\nYou Win!" : (playerWins == computerWins) ? "\n\nIt's a tie!" : "\n\nThe Computer Wins :(");
            Console.ReadKey();
        }
    }
}