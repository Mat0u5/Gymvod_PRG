using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grafika
{
    public partial class Form1 : Form
    {
        public int size = 50;
        public Pen pen = new Pen(Color.Red, 3);
        public Form1()
        {
            InitializeComponent();
            label1.Text = size.ToString();
        }

        private void onPaint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            int halfsize = size / 2;
            graphics.DrawLine(pen, 100 + halfsize, 100, 100 + size, 100 + size);
            graphics.DrawLine(pen, 100 + halfsize, 100, 100, 100 + size);
            graphics.DrawRectangle(pen, 100, 100+size, size, size);
            graphics.DrawRectangle(pen, 100 + (int)(size * 0.6), 100 + (int)(size * 1.5), (int)(size*0.3), halfsize);
        }

        private void onScroll(object sender, ScrollEventArgs e)
        {
            size = e.NewValue;
            label1.Text = size.ToString();
            Refresh();
        }

        private void onListChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index == 0) pen = Pens.Red;
            else if (index == 1) pen = Pens.Green;
            else if (index == 2) pen = Pens.Blue;
            else pen = Pens.Red;
            pen = new Pen(pen.Color, 3);
            Refresh();
        }

        private void onMouseMove(object sender, MouseEventArgs e)
        {
        }

        private int lastX = -1;
        private int lastY = -1;
        private int x = -1;
        private int y = -1;
        private void onPanelPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.LightGray);
        }

        private void onPanelMouseMove(object sender, MouseEventArgs e)
        {
            x = e.Location.X;
            y = e.Location.Y;
            if (e.Button == MouseButtons.Left)
            {
                if (pen != null && lastX != -1 && lastY != -1 && x != -1 && y != -1)
                {
                    panel1.CreateGraphics().DrawLine(pen, lastX, lastY, x, y);
                }
            }
            lastX = x;
            lastY = y;
        }
    }
}
