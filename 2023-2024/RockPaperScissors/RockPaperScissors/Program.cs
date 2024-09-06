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
            while (int.TryParse(Console.ReadLine(), out roundsNum) != true)
            {
                Console.WriteLine("That's not a number. Try again.");
                continue;
            }
            Dictionary<String, String> evaluateTable = new Dictionary<String, String>()
            //The evaluate table keys are the only allowed inputs in the game
            //The key beats the value    -   {"rock","scissors"}  -> rock beats scissors
                {
                    { "rock","scissors"},{"paper","rock"},{"scissors","paper"}
                };
            int playerWins = 0;
            int computerWins = 0;
            //print the allowed inputs (all the keys from the evaluateTable)
            Console.WriteLine("ALLOWED INPUTS: " + string.Join(",", evaluateTable.Keys.ToList()));
            Random rnd = new Random();
            while (playerWins+computerWins < roundsNum)
            {
                Console.WriteLine("Enter your input:");
                String playerInput = Console.ReadLine();
                //check if player input is valid
                if (!evaluateTable.ContainsKey(playerInput)) continue;
                //pick a random computer input (random key from evaluateTable)
                String computerInput = evaluateTable.Keys.ToArray()[rnd.Next(0, evaluateTable.Keys.Count)];
                Console.WriteLine("   Computer played " + computerInput);
                if (evaluateTable[playerInput].Equals(computerInput))
                {
                    //if the player input matches a key and the computer input matches a value for that key, the player wins
                    // - {"rock","scissors"}  -> if player plays rock and computer plays scissors, the player wins
                    Console.WriteLine("   ---You Won---");
                    playerWins++;
                }
                else if (evaluateTable[computerInput].Equals(playerInput))
                {
                    //if the computer input matches a key and the player input matches a value for that key, the computer wins
                    // - {"rock","scissors"}  -> if computer plays rock and player plays scissors, the computer wins
                    Console.WriteLine("   ---The computer Won---");
                    computerWins++;
                }
                else Console.WriteLine("   ---Draw---");
            }
            String roundsResult = "("+ playerWins  + ", " + computerWins+ ")";
            if (playerWins == computerWins) Console.WriteLine("\n---\nITS A DRAW! "+ roundsResult + "\n---");
            if (playerWins > computerWins) Console.WriteLine("\n---\nYOU WON! "+ roundsResult + "\n---");
            if (playerWins < computerWins) Console.WriteLine("\n---\nYOU LOST! "+ roundsResult + "\n---");
            Console.ReadKey();
        }
    }
}
