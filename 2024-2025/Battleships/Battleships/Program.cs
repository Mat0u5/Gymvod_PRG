using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            while(true)
            {
                // Loop allows you to start a new game after one ends.
                Game game = new Game();
                game.Start();
            }
        }
    }
}
