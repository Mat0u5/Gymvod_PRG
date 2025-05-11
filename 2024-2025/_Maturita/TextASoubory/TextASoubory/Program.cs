using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextASoubory
{
    internal class Program
    {
        private static Random rnd = new Random();
        static void Main(string[] args)
        {
            String();
            Files();
            Encoding();
        }
        static void String()
        {
            // String a zpracování
            String input = "5561126";
            int numberInput = Convert.ToInt32(input);
            numberInput += 5;
            String output = Convert.ToString(numberInput);
            Console.WriteLine("\"" + input + "\" was converted to \"" + output + "\"");
            String multiline = "Test\n\tTest2";

            Console.WriteLine("Multiline: " + multiline + "\n");

            String example = "Computer";
            Console.WriteLine("Operations with: " + example);
            char firstLetter = example[0];
            char randomLetter = example[rnd.Next(example.Length - 1)];
            Console.WriteLine("First letter: " + firstLetter);
            Console.WriteLine("Random letter: " + randomLetter);
            Console.WriteLine();
            foreach (char letter in example)
            {
                Console.WriteLine(letter);
            }

            Console.WriteLine();

            Console.WriteLine("Length: " + example.Length);
            int halfwayPoint = example.Length/2;
            Console.WriteLine("First half: " + example.Substring(0, halfwayPoint));
            Console.WriteLine("Second half: " + example.Substring(halfwayPoint));
            Console.WriteLine("No middle: " + example.Remove(2,3)); //Co mpu ter
            Console.WriteLine("Replaced: " + example.Replace("a", "_").Replace("e", "_").Replace("i", "_").Replace("o", "_").Replace("u", "_"));


            Console.ReadKey();
        }
        static void Files()
        {
            // Čtení a zapisování do souborů
            String file = "./text.txt";

            if (!File.Exists(file))
            {
                File.Create(file);
            }

            while (true)
            {
                Console.Clear();

                StreamReader reader = new StreamReader(file);
                String contents = reader.ReadToEnd();
                reader.Close();
                Console.WriteLine("File Contents:\n\n" + contents);
                Console.WriteLine("\n\nEnter a string to add to the text file");

                String add = Console.ReadLine();
                if (add == "stop") break;
                StreamWriter writer = new StreamWriter(file);
                writer.Write(contents);
                writer.WriteLine(add);
                writer.Close();
            }

        }
        static void Encoding()
        {
            //Encoding
            //ASCII - 128 znaků (americká abeceda) -> 7bit
            // Unicode - univerzální znaková sada která může být kódována množstvím způsobů
            //Windows-1250, UTF-8, ..


            // Random Character
            char randomLowercaseLetter = (char)('a' + rnd.Next(26));
            char randomUppercaseLetter = (char)('A' + rnd.Next(26));
            Console.WriteLine("Random lowercase: " + randomLowercaseLetter);
            Console.WriteLine("Random uppercase: " + randomUppercaseLetter);
            Console.ReadKey();
        }
    }
}
