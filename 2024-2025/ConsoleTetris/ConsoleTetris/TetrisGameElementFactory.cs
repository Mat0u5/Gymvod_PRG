using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    internal class TetrisGameElementFactory : GameElementFactory
    {
        public override IGamePiece CreatePiece(int[,] shape, ConsoleColor color)
        {
            return new Piece(shape, color);
        }

        public override IGameBoard CreateGameBoard(int width, int height, int startX, int startY)
        {
            return new GameBoard(width, height, startX, startY);
        }

        public override PlayerInput CreateInputHandler()
        {
            return new PlayerInput();
        }
    }
}
