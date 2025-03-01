using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    internal interface IGamePiece
    {
        // I know the interfaces are essentially useless the way i'm using them, but i was SUPPOSED to use them so whatever.
        int X { get; set; }
        int Y { get; set; }
        ConsoleColor Color { get; }
        int[,] Shape { get; }

        bool Move(int dx, int dy, IGameBoard board);
        void Rotate(IGameBoard board);
        bool Occupies(int boardX, int boardY);
        void Draw(int startX, int startY);
        void ResetPosition();
        void Drop(IGameBoard board);
    }
}
