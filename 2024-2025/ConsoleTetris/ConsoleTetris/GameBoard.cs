using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    internal class GameBoard : IGameBoard
    {
        public static Boolean drawing = false;
        public static Boolean drawingPiece = false;
        const int borderWidth = 1;

        private int width, height, startX, startY;
        private int[,] board;

        public int getWidth()
        {
            return width;
        }
        public int getHeight()
        {
            return height;
        }
        public int getStartX()
        {
            return startX;
        }
        public int getStartY()
        {
            return startY;
        }

        public GameBoard(int w, int h, int x, int y)
        {
            width = w;
            height = h;
            startX = x;
            startY = y;
            board = new int[height, width];
        }

        public bool IsValidMove(int x, int y, int[,] shape)
        {
            //Checks if the new position is in bounds of the board
            for (int row = 0; row < shape.GetLength(0); row++)
                for (int col = 0; col < shape.GetLength(1); col++)
                    if (shape[row, col] == 1)
                    {
                        int newX = x + col;
                        int newY = y + row;
                        if (newX < 0 || newX >= width || 
                            newY >= height || 
                            (newY >= 0 && board[newY, newX] == 1))
                        {
                            return false;
                        }
                    }
            return true;
        }

        public void PlacePiece(IGamePiece piece)
        {
            for (int row = 0; row < piece.Shape.GetLength(0); row++)
                for (int col = 0; col < piece.Shape.GetLength(1); col++)
                {
                    if (piece.Shape[row, col] == 1)
                    {
                        board[piece.Y + row, piece.X + col] = 1;
                    }
                }
        }

        public int ClearLines()
        {
            //Checks if any lines can be cleared, and clears them if it can find any
            int linesCleared = 0;
            for (int y = height - 1; y >= 0; y--)
            {
                //Goes from top to bottom of the board
                bool full = true;
                for (int x = 0; x < width; x++)
                {
                    //Checks if any blocks in the current row are empty
                    if (board[y, x] == 0)
                    {
                        full = false;
                    }
                }
                if (full)
                {
                    // If no empty blocks are found in this row, it can be cleared -> moves the board down
                    linesCleared++;
                    for (int yy = y; yy > 0; yy--)
                        for (int x = 0; x < width; x++)
                            board[yy, x] = board[yy - 1, x];
                    y++;
                }
            }
            return linesCleared;
        }

        public void DrawStaticElements()
        {
            Console.Clear();
            DrawLogo();
            Console.SetCursorPosition(startX + width / 2 - 3, startY - 2);

            // Draw board border
            Console.SetCursorPosition(startX - borderWidth, startY - borderWidth);
            for (int x = -1; x < width; x++) Console.Write("██");
            for (int y = 0; y < height; y++)
            {
                Console.SetCursorPosition(startX - borderWidth, startY + y);
                Console.Write("█");
                Console.SetCursorPosition(startX + width * 2, startY + y);
                Console.Write("█");
            }
            Console.SetCursorPosition(startX - borderWidth, startY + height);
            for (int x = -1; x < width; x++) Console.Write("██");

            // Draw held piece box
            Console.SetCursorPosition(4, 8);
            Console.WriteLine("Held:");
            DrawBox(2, 9, 8, 4);

            // Draw next piece box
            Console.SetCursorPosition(37, 8);
            Console.WriteLine("Next:");
            DrawBox(36, 9, 8, 4);

            // Draw controls helper
            Console.SetCursorPosition(2, startY + height + 4);
            Console.WriteLine("Controls: Arrow Keys = Move, Up = Rotate, C = Hold, Space = Drop, Q = Quit");
        }

        public void DrawGameBoard()
        {
            for (int y = 0; y < height; y++)
            {
                Console.SetCursorPosition(startX, startY + y);
                for (int x = 0; x < width; x++)
                {
                    bool isPartOfPiece = Program.currentPiece.Occupies(x, y);
                    //Draws pieces / empty space.
                    Console.ForegroundColor = isPartOfPiece ? Program.currentPiece.Color : ConsoleColor.DarkGray;
                    Console.Write(isPartOfPiece || board[y, x] == 1 ? "██" : " .");
                }
            }
            Console.SetCursorPosition(12, startY + height + 2);
            Console.WriteLine($"Score: {Program.score}    High Score: {Program.highScore}");

            //Clear boxes where held piece and next piece is
            ClearBox(3, 10, 8, 3);
            ClearBox(37, 10, 8, 3);
            // Draw held piece
            if (Program.heldPiece != null)
            {
                Program.heldPiece.Draw(3, 10);
            }

            // Draw next piece
            Program.nextPiece.Draw(37, 10);
        }

        private void DrawBox(int x, int y, int width, int height)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("+" + new string('-', width) + "+");
            for (int i = 1; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("|" + new string(' ', width) + "|");
            }
            Console.SetCursorPosition(x, y + height);
            Console.Write("+" + new string('-', width) + "+");
        }

        private void ClearBox(int x, int y, int width, int height)
        {
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(new string(' ', width));
            }
        }

        private void DrawLogo()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("   _______ ______ _______ _____  _____  _____ \r\n  |__   __|  ____|__   __|  __ \\|_   _|/ ____|\r\n     | |  | |__     | |  | |__) | | | | (___  \r\n     | |  |  __|    | |  |  _  /  | |  \\___ \\ \r\n     | |  | |____   | |  | | \\ \\ _| |_ ____) |\r\n     |_|  |______|  |_|  |_|  \\_\\_____|_____/ \r\n                                              \r\n                                              ");
        }

        public void DrawGameOver()
        {
            ClearBox(0, 0, 20, 6);
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(" _____ ____  _      _____   ____  _     _____ ____ \r\n/  __//  _ \\/ \\__/|/  __/  /  _ \\/ \\ |\\/  __//  __\\\r\n| |  _| / \\|| |\\/|||  \\    | / \\|| | //|  \\  |  \\/|\r\n| |_//| |-||| |  |||  /_   | \\_/|| \\// |  /_ |    /\r\n\\____\\\\_/ \\|\\_/  \\|\\____\\  \\____/\\__/  \\____\\\\_/\\_\\\r\n                                                   ");
        }
    }
}
