using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    internal class PlayerInput
    {
        public void HandleInput(Random rand)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.C:
                        if (Program.canHold)
                        {
                            Piece temp = Program.heldPiece;
                            Program.heldPiece = Program.currentPiece;
                            if (temp == null)
                            {
                                Program.currentPiece = Program.nextPiece;
                                Program.nextPiece = Piece.GenerateRandomPiece(rand);
                            }
                            else
                            {
                                Program.currentPiece = temp;
                            }
                            Program.currentPiece.ResetPosition();
                            Program.heldPiece.ResetPosition();
                            Program.canHold = false;
                        }
                        break;
                    case ConsoleKey.Spacebar:
                        Program.currentPiece.Drop(Program.board);
                        Program.ticks -= Program.ticks % 100;
                        break;
                    case ConsoleKey.LeftArrow:
                        Program.currentPiece.Move(-1, 0, Program.board);
                        break;
                    case ConsoleKey.RightArrow:
                        Program.currentPiece.Move(1, 0, Program.board);
                        break;
                    case ConsoleKey.DownArrow:
                        if (Program.currentPiece.Move(0, 1, Program.board)) {
                            Program.score++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        Program.currentPiece.Rotate(Program.board);
                        break;
                    case ConsoleKey.Q:
                        Environment.Exit(0);
                        break;
                }
                Program.board.DrawGameBoard();
            }
        }
    }
}
