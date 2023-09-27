using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2023-2024.
 * Extended by students.
 */

namespace RockPaperScissors
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("How many round do you want to play?");
            int roundsNum = 0;
            while (int.TryParse(Console.ReadLine(), out roundsNum) != true) continue;
            String[] options = new String[] { "rock", "paper", "scissors" };
            Dictionary<String, String> evaluateTable = new Dictionary<String, String>()
                {
                    { "rock","scissors"},{"paper","rock"},{"scissors","paper"}
                };
            int playerWins = 0;
            int computerWins = 0;
            Console.WriteLine("ALLOWED INPUTS: rock, paper, scissors");
            while (playerWins+computerWins < roundsNum)
            {
                Console.WriteLine("Enter your input:");
                String playerInput = Console.ReadLine();
                if (evaluateTable.ContainsKey(playerInput))
                {
                    Random rnd = new Random();
                    String computerInput = options[rnd.Next(0, options.Length)];
                    Console.WriteLine("   Computer played " + computerInput);
                    if (playerInput.Equals(computerInput)) Console.WriteLine("   ---Draw---");
                    if (evaluateTable[playerInput].Equals(computerInput))
                    {
                        Console.WriteLine("   ---You Won---");
                        playerWins++;
                    }
                    if (evaluateTable[computerInput].Equals(playerInput))
                    {
                        Console.WriteLine("   ---Computer Won---");
                        computerWins++;
                    }
                }
            }
            String result = "("+ playerWins  + ", " + computerWins+ ")";
            if (playerWins == computerWins) Console.WriteLine("\n---\nITS A DRAW! "+ result + "\n---");
            if (playerWins > computerWins) Console.WriteLine("\n---\nYOU WON! "+ result + "\n---");
            if (playerWins < computerWins) Console.WriteLine("\n---\nYOU LOST "+ result + "\n---");
            Console.ReadKey();
        }
    }
}
