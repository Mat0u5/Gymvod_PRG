using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsIDK
{
    public partial class Form1 : Form
    {
        Rectangle rectangle;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rectangle = new Rectangle(10,20,100,200,Pens.Red,Brushes.Blue,false);
            this.Refresh();
        }

        private void onPaint(object sender, PaintEventArgs e)
        {
            rectangle.draw(e.Graphics);
        }

        private void onKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'f')
            {
                rectangle.filled = !rectangle.filled;
                Refresh();
            }
            if (e.KeyChar == 'g')
            {
                rectangle.width--;
                Refresh();
            }
            if (e.KeyChar == 'h')
            {
                rectangle.width++;
                Refresh();
            }
        }

        private void onMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                rectangle.x = e.X;
                rectangle.y = e.Y;
                Refresh();
            }
            if (e.Button == MouseButtons.Right)
            {
                rectangle.width = e.X- rectangle.x;
                rectangle.height = e.Y- rectangle.y;
                Refresh();
            }
        }
    }
}
