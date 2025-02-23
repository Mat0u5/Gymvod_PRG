using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    internal class Piece
    {
        public int X;
        public int Y;
        public int[,] Shape;
        public ConsoleColor Color;

        private static readonly int[][,] Shapes = new int[][,]
        {
            new int[,] { { 1, 1, 1, 1 } }, // I
            new int[,] { { 1, 1 }, { 1, 1 } }, // O
            new int[,] { { 0, 1, 0 }, { 1, 1, 1 } }, // T
            new int[,] { { 1, 0, 0 }, { 1, 1, 1 } }, // L
            new int[,] { { 0, 0, 1 }, { 1, 1, 1 } }, // J
            new int[,] { { 1, 1, 0 }, { 0, 1, 1 } }, // S
            new int[,] { { 0, 1, 1 }, { 1, 1, 0 } }  // Z
        };

        private static ConsoleColor[] Colors = new ConsoleColor[]
        {
            ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Magenta,
            ConsoleColor.DarkYellow, ConsoleColor.Blue, ConsoleColor.Green,ConsoleColor.Red
        };

        public Piece(int[,] shape, ConsoleColor color)
        {
            Shape = shape;
            Color = color;
            X = 3;
            Y = 0;
        }

        public static Piece GenerateRandomPiece(Random rand)
        {
            int index = rand.Next(Shapes.Length);
            return new Piece(Shapes[index], Colors[index]);
        }

        public bool Move(int dx, int dy, GameBoard board)
        {
            if (board.IsValidMove(X + dx, Y + dy, Shape))
            {
                X += dx;
                Y += dy;
                return true;
            }
            return false;
        }

        public void Rotate(GameBoard board)
        {
            int rows = Shape.GetLength(0);
            int cols = Shape.GetLength(1);
            int[,] rotated = new int[cols, rows];

            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                    rotated[x, rows - 1 - y] = Shape[y, x];

            if (board.IsValidMove(X, Y, rotated))
                Shape = rotated;
        }

        public bool Occupies(int boardX, int boardY)
        {
            for (int row = 0; row < Shape.GetLength(0); row++)
                for (int col = 0; col < Shape.GetLength(1); col++)
                    if (Shape[row, col] == 1 && X + col == boardX && Y + row == boardY)
                        return true;
            return false;
        }

        public void Draw(int startX, int startY)
        {
            GameBoard.drawingPiece = true;
            for (int y = 0; y < Shape.GetLength(0); y++)
            {
                Console.SetCursorPosition(startX, startY + y);
                for (int x = 0; x < Shape.GetLength(1); x++)
                {
                    Console.ForegroundColor = Color;
                    Console.Write(Shape[y, x] == 1 ? "██" : "  ");
                }
            }
            Console.ResetColor();
            GameBoard.drawingPiece = false;
        }

        public void ResetPosition()
        {
            X = 3;
            Y = 1;
        }

        public void Drop(GameBoard board)
        {
            while (Move(0, 1, board)) { }
        }
    }
}
