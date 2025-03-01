using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    internal interface IGameBoard
    {
        // I know the interfaces are essentially useless the way i'm using them, but i was SUPPOSED to use them so whatever.
        int getWidth();
        int getHeight();
        int getStartX();
        int getStartY();
        bool IsValidMove(int x, int y, int[,] shape);
        void PlacePiece(IGamePiece piece);
        int ClearLines();
        void DrawStaticElements();
        void DrawGameBoard();
        void DrawGameOver();
    }
}
