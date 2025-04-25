using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP
{
    internal class MovableShape : Shape
    {
        public MovableShape(int size, int posX, int posY, Pen pen) : base(size, posX, posY, pen)
        {
        }

        public void move(int moveX)
        {
            posX += moveX;
            posX = Math.Max(posX, 0);
        }
    }
}
