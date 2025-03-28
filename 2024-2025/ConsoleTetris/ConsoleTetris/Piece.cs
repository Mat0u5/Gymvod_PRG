﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    internal class Piece : IGamePiece
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int[,] Shape { get; private set; }
        public ConsoleColor Color { get; private set; }

        private static readonly int[][,] Shapes = new int[][,]
        {
            new int[,] { { 1, 1, 1, 1 } },              // I Piece
            new int[,] { { 1, 1 }, { 1, 1 } },          // O Piece
            new int[,] { { 0, 1, 0 }, { 1, 1, 1 } },    // T Piece
            new int[,] { { 1, 0, 0 }, { 1, 1, 1 } },    // L Piece
            new int[,] { { 0, 0, 1 }, { 1, 1, 1 } },    // J Piece
            new int[,] { { 1, 1, 0 }, { 0, 1, 1 } },    // S Piece
            new int[,] { { 0, 1, 1 }, { 1, 1, 0 } }     // Z Piece
        };

        private static ConsoleColor[] Colors = new ConsoleColor[]
        {
            ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Magenta,
            ConsoleColor.DarkYellow, ConsoleColor.Blue, ConsoleColor.Green, ConsoleColor.Red
        };

        public Piece(int[,] shape, ConsoleColor color)
        {
            Shape = shape;
            Color = color;
            X = 3;
            Y = 0;
        }

        public static IGamePiece GenerateRandomPiece(Random rand)
        {
            int index = rand.Next(Shapes.Length);
            return new Piece(Shapes[index], Colors[index]);
        }

        public bool Move(int dx, int dy, IGameBoard board)
        {
            if (board.IsValidMove(X + dx, Y + dy, Shape))
            {
                X += dx;
                Y += dy;
                return true;
            }
            return false;
        }

        public void Rotate(IGameBoard board)
        {
            int rows = Shape.GetLength(0);
            int cols = Shape.GetLength(1);
            int[,] rotated = new int[cols, rows];

            for (int y = 0; y < rows; y++)
                for (int x = 0; x < cols; x++)
                    rotated[x, rows - 1 - y] = Shape[y, x];

            if (board.IsValidMove(X, Y, rotated))
            {
                //Only rotate if it's a valid move
                Shape = rotated;
            }
        }

        public bool Occupies(int boardX, int boardY) // If the shape intersects the boardX, boardY coordinates.
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
                    //Shapes are stored as arrays, 1 = block, 0 = empty space
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

        public void Drop(IGameBoard board)
        {
            while (Move(0, 1, board)) {} //Drops until it can't anymore
        }
    }
}
