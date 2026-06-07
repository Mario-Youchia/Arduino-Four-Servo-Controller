using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServosTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        Rectangle ImageArea(PictureBox pbox)
        {
            Size si = pbox.Image.Size;
            Size sp = pbox.ClientSize;

            if (pbox.SizeMode == PictureBoxSizeMode.StretchImage) return pbox.ClientRectangle;
            if (pbox.SizeMode == PictureBoxSizeMode.Normal ||
               pbox.SizeMode == PictureBoxSizeMode.AutoSize) return new Rectangle(Point.Empty, si);
            if (pbox.SizeMode == PictureBoxSizeMode.CenterImage)
                return new Rectangle(new Point((sp.Width - si.Width) / 2,
                                    (sp.Height - si.Height) / 2), si);

            //  PictureBoxSizeMode.Zoom
            float ri = 1f * si.Width / si.Height;
            float rp = 1f * sp.Width / sp.Height;
            if (rp > ri)
            {
                int width = si.Width * sp.Height / si.Height;
                int left = (sp.Width - width) / 2;
                return new Rectangle(left, 0, width, sp.Height);
            }
            else
            {
                int height = si.Height * sp.Width / si.Width;
                int top = (sp.Height - height) / 2;
                return new Rectangle(0, top, sp.Width, height);
            }
        }
        bool firstTime = true;
        private void Form2_Load(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(ImageArea(pictureBox1).Width, ImageArea(pictureBox1).Height);
            ClientSize = new Size(ImageArea(pictureBox1).Width, ImageArea(pictureBox1).Height);
            pictureBox1.Location = new Point(0, 0);
            firstTime = false;
        }

        private void Form2_SizeChanged(object sender, EventArgs e)
        {
            if (!firstTime)
            {
                pictureBox1.Size = new Size(Size.Width, Size.Height);
                pictureBox1.Size = new Size(ImageArea(pictureBox1).Width, ImageArea(pictureBox1).Height);
                pictureBox1.Location = new Point(0, 0);
            }
        }

        private void Form2_ResizeEnd(object sender, EventArgs e)
        {
            ClientSize = new Size(ImageArea(pictureBox1).Width, ImageArea(pictureBox1).Height);
        }
    }
}
