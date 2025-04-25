using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP
{
    internal class Shape
    {
        public int size = 10;
        public int posX;
        public int posY;
        public Pen pen;
        public Shape(int size, int posX, int posY, Pen pen) 
        {
            this.size = size;
            this.posX = posX;
            this.posY = posY;
            this.pen = pen;
        }

        public void changeSize(int change)
        {
            size += change;
            size = Math.Max(size, 5);
        }

        public void draw(Graphics graphics)
        {
            graphics.DrawEllipse(pen, posX, posY, size, size);
        }
    }
}
