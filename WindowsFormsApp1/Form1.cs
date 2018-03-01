using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = WindowsFormsApp1.Properties.Resources.falcon2;
            pictureBox2.Image = WindowsFormsApp1.Properties.Resources.falcon2;
        }
        #region inverse filter
        private void button6_Click(object sender, EventArgs e)
        {
            inversionFilter((Bitmap)pictureBox1.Image);
        }
        private void inversionFilter(Bitmap bmp)
        {
            for(int y = 0; y < bmp.Height; y++)
            {
                for(int x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    var r = 255 - pixel.R;
                    var g = 255 - pixel.G;
                    var b = 255 - pixel.B;
                    bmp.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                }
            }
            pictureBox2.Image = bmp;
        }
        #endregion
        //clear image
        private void button10_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = WindowsFormsApp1.Properties.Resources.falcon2;
        }
        #region brighntess filter
        private void button7_Click(object sender, EventArgs e)
        {
            var brightnessCoefficient = -30;
            brightnessConvertion((Bitmap)pictureBox1.Image, brightnessCoefficient);
        }

        private void brightnessConvertion(Bitmap bmp, int brightnessCoefficient)
        {
            for(int y = 0; y < bmp.Height; y++)
            {
                for(int x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);

                    var r = 0;
                    if (pixel.R + brightnessCoefficient < 0) r = 0;
                    else r = (pixel.R + brightnessCoefficient < 255) ? pixel.R + brightnessCoefficient : 255;

                    var g = 0;
                    if (pixel.G + brightnessCoefficient < 0) g = 0;
                    else g = (pixel.G + brightnessCoefficient < 255) ? pixel.G + brightnessCoefficient : 255;

                    var b = 0;
                    if (pixel.B + brightnessCoefficient < 0) b = 0;
                    else b = (pixel.B + brightnessCoefficient < 255) ? pixel.B + brightnessCoefficient : 255;

                    bmp.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                }
            }
            pictureBox2.Image = bmp;
        }
        #endregion
        #region contrast filter
        private void button8_Click(object sender, EventArgs e)
        {
            var contrastCoefficient = 2.0 ;
            contrastFilter((Bitmap)pictureBox2.Image, contrastCoefficient);
        }
        private void contrastFilter(Bitmap bmp, double contrastCoefficient)
        {
            for(int y = 0; y < bmp.Height; y++)
            {
                for(int x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    var r = 0;
                    if (contrastEnchancement((double)pixel.R, contrastCoefficient) < 0) r = 0;
                    else r = (contrastEnchancement((double)pixel.R, contrastCoefficient) < 255.0) ? (int)contrastEnchancement((double)pixel.R, contrastCoefficient) : 255;

                    var g = 0;
                    if (contrastEnchancement((double)pixel.G, contrastCoefficient) < 0) g = 0;
                    else g = (contrastEnchancement((double)pixel.G, contrastCoefficient) < 255.0) ? (int)contrastEnchancement((double)pixel.G, contrastCoefficient) : 255;

                    var b = 0;
                    if (contrastEnchancement((double)pixel.B, contrastCoefficient) < 0) b = 0;
                    else b = (contrastEnchancement((double)pixel.B, contrastCoefficient) < 255.0) ? (int)contrastEnchancement((double)pixel.B, contrastCoefficient) : 255;

                    bmp.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                }
            }
            pictureBox2.Image = bmp;
        }
        private double contrastEnchancement(double channelValue, double contrastCoefficient)
        {
            return ((((channelValue / 255.0) - 0.5) * contrastCoefficient) + 0.5) * 255.0;
        }
        #endregion

        private void button9_Click(object sender, EventArgs e)
        {
           // convolutionFilter( matrix, offset);
        }
        private void convolutionFilter(int [,] matrix, int offset)
        {

        }
    }
}
