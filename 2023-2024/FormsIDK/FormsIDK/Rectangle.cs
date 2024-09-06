using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsIDK
{
    internal class Rectangle
    {
        public int x, y, width, height;
        public Brush brush;
        public Pen pen;
        public bool filled;
        public Rectangle(int x, int y, int width, int height, Pen pen, Brush brush, bool filled)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.pen = pen;
            this.brush = brush;
            this.filled = filled;
        }
        public void draw(Graphics graphics)
        {
            if (this.filled)
            {
                 graphics.FillRectangle(this.brush, this.x, this.y, this.width, this.height);
            }
            else
            {
                graphics.DrawRectangle(this.pen, this.x, this.y, this.width, this.height);
            }
        }
    }
}
