using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ServosTest
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            ClientSize = new Size(label3.Size.Width + 35, ClientSize.Height);
            panel1.Width = ClientSize.Width;
            button1.Location = new Point(ClientSize.Width - 50 - button1.Size.Width, button1.Location.Y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
