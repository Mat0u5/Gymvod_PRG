using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP
{
    public partial class Form1 : Form
    {
        Shape circle;
        MovableShape movableCircle;
        public Form1()
        {
            InitializeComponent();
        }

        private void onLoad(object sender, EventArgs e)
        {
            circle = new Shape(20, 0, 0, Pens.Blue);
            movableCircle = new MovableShape(50, 0, 100, Pens.Red);
        }

        private void onPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            circle.draw(g);
            movableCircle.draw(g);
        }

        private void onKeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void drawShapes()
        {

        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                circle.changeSize(3);
                movableCircle.changeSize(3);
                Refresh();
            }
            else if (e.KeyCode == Keys.Down)
            {
                circle.changeSize(-3);
                movableCircle.changeSize(-3);
                Refresh();
            }
            else if (e.KeyCode == Keys.Left)
            {
                movableCircle.move(-10);
                Refresh();
            }
            else if (e.KeyCode == Keys.Right)
            {
                movableCircle.move(10);
                Refresh();
            }
        }
    }
}
