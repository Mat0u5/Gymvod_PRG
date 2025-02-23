﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    internal class Program
    {
        public static GameBoard board;
        public static Piece currentPiece, nextPiece, heldPiece;
        public static bool canHold;
        public static PlayerInput input = new PlayerInput();
        public static Random rand = new Random();
        public static int score = 0;
        public static int highScore = 0;
        static int speed = 500;
        static bool gameOver = false;
        public static int ticks = 0;
        static void Main(string[] args)
        {
            while (true)
            {
                Console.CursorVisible = false;
                Console.Clear();
                board = new GameBoard(10, 20, 14, 8);
                board.DrawStaticElements();
                score = 0;
                speed = 500;
                ticks = 0;
                gameOver = false;
                canHold = true;
                currentPiece = Piece.GenerateRandomPiece(rand);
                nextPiece = Piece.GenerateRandomPiece(rand);
                heldPiece = null;
                while (!gameOver && !GameBoard.drawing && !GameBoard.drawingPiece)
                {
                    Thread.Sleep(5);
                    input.HandleInput(rand);
                    if (ticks % 100 == 0)
                    {
                        GameLoop();
                    }
                    ticks++;
                }
                Console.SetCursorPosition(15, board.startY + board.height + 4);
                board.DrawGameOver();
                Console.ReadKey();
            }
        }
        static void GameLoop()
        {
            GameBoard.drawing = true;
            if (!currentPiece.Move(0, 1, board))
            {
                board.PlacePiece(currentPiece);
                score += board.ClearLines() * 100;
                if (score > highScore) highScore = score;
                speed = Math.Max(100, 500 - (score / 500) * 50);
                currentPiece = nextPiece;
                nextPiece = Piece.GenerateRandomPiece(rand);
                canHold = true;
                if (!board.IsValidMove(currentPiece.X, currentPiece.Y, currentPiece.Shape))
                {
                    gameOver = true;
                }
            }
            board.DrawGameBoard();
            GameBoard.drawing = false;
        }
    }
}
