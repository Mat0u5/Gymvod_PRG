using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace UserInterface
{
    public partial class Form1 : Form
    {
        private static int length = 10;
        private static Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void onValueChanged(object sender, EventArgs e)
        {
            length = hScrollBar1.Value;
            label1.Text = "Length: " + length;
        }

        private void onClick(object sender, EventArgs e)
        {
            List<int> possible = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                if (checkedListBox1.GetItemChecked(i)) possible.Add(i);
            }
            String password = "";

            for (int i = 0; i < length; i++)
            {
                if (possible.Count <= 0) continue;
                int selected = possible[rnd.Next(possible.Count)];
                if (selected == 0)
                {
                    char letter = (char)('a'+ rnd.Next(26));
                    password += letter;
                }
                else if (selected == 1)
                {
                    //String letter = Convert.ToString((char)rnd.Next(65, 91));
                    //password += letter.ToUpper();
                    char letter = (char)('A' + rnd.Next(26));
                    password += letter;
                }
                else if (selected == 2)
                {
                    password += Convert.ToString(rnd.Next(10));
                }
                else if (selected == 3)
                {
                    List<String> list = new List<String>() { "+", "-", "*", "/", "\\", "=" } ;
                    password += list[rnd.Next(list.Count)];
                }
            }
            textBox1.Text = password;
        }
    }
}
