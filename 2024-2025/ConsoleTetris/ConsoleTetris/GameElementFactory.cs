using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    internal abstract class GameElementFactory
    {
        public abstract IGamePiece CreatePiece(int[,] shape, ConsoleColor color);
        public abstract IGameBoard CreateGameBoard(int width, int height, int startX, int startY);
        public abstract PlayerInput CreateInputHandler();
    }
}
