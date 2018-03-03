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
            pictureBox1.Image = Properties.Resources.singapur;
            pictureBox2.Image = pictureBox1.Image;
        }
        #region inverse filter
        private void button6_Click(object sender, EventArgs e)
        {
            var bitmap = (Bitmap)pictureBox1.Image;
            inversionFilter(bitmap);
        }
        private void inversionFilter(Bitmap bmp)
        {
            var temp = bmp;
            for(int y = 0; y < bmp.Height; y++)
            {
                for(int x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    var r = 255 - pixel.R;
                    var g = 255 - pixel.G;
                    var b = 255 - pixel.B;
                    temp.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                }
            }
            pictureBox2.Image = temp;
        }
        #endregion
        #region clearButton
        private void button10_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.singapur; 
        }
        #endregion
        #region brighntess filter
        private void button7_Click(object sender, EventArgs e)
        {
            var bitmap = (Bitmap)pictureBox1.Image;
            var brightnessCoefficient = 30;
            brightnessConvertion(bitmap, brightnessCoefficient);
        }

        private void brightnessConvertion(Bitmap bmp, int brightnessCoefficient)
        {
            var temp = bmp;
            for(int y = 0; y < bmp.Height; y++)
            {
                for(int x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);

                    var r = (pixel.R + brightnessCoefficient < 0) ? 0 : (pixel.R + brightnessCoefficient < 255) ? (pixel.R + brightnessCoefficient) : 255;
                    var g = (pixel.G + brightnessCoefficient < 0) ? 0 : (pixel.G + brightnessCoefficient < 255) ? (pixel.G + brightnessCoefficient) : 255;
                    var b = (pixel.B + brightnessCoefficient < 0) ? 0 : (pixel.B + brightnessCoefficient < 255) ? (pixel.B + brightnessCoefficient) : 255;
                    temp.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                }
            }
            pictureBox2.Image = temp;
        }
        #endregion
        #region contrast filter
        private void button8_Click(object sender, EventArgs e)
        {
            var bitmap = (Bitmap)pictureBox1.Image;
            var contrastCoefficient = 2.0 ;
            contrastFilter(bitmap, contrastCoefficient);
        }
        private void contrastFilter(Bitmap bmp, double contrastCoefficient)
        {
            for(int y = 0; y < bmp.Height; y++)
            {
                for(int x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    var r = (contrastEnchancement((double)pixel.R, contrastCoefficient) < 0) ? 0 : 
                        (contrastEnchancement((double)pixel.R, contrastCoefficient) < 255.0) ? (int)contrastEnchancement((double)pixel.R, contrastCoefficient) : 255;
                    var g = (contrastEnchancement((double)pixel.G, contrastCoefficient) < 0) ? 0 : 
                        (contrastEnchancement((double)pixel.G, contrastCoefficient) < 255.0) ? (int)contrastEnchancement((double)pixel.G, contrastCoefficient) : 255;
                    var b = (contrastEnchancement((double)pixel.B, contrastCoefficient) < 0) ? 0 : 
                        (contrastEnchancement((double)pixel.B, contrastCoefficient) < 255.0) ? (int)contrastEnchancement((double)pixel.B, contrastCoefficient) : 255;
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
        #region grayscale
        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = (Bitmap)pictureBox1.Image;
            grayScale(bitmap);
        }
        private void grayScale(Bitmap bmp)
        {
            Bitmap bitmap = bmp;
            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    var sumOfChannelValues = (pixel.R + pixel.G + pixel.B) / 3;
                    var grayScaleValueOfThePixel = (sumOfChannelValues < 255) ? sumOfChannelValues : 255;
                    bitmap.SetPixel(x, y, Color.FromArgb(grayScaleValueOfThePixel, grayScaleValueOfThePixel, grayScaleValueOfThePixel));
                }
            }
            pictureBox2.Image = bitmap;
        }
        #endregion

        private void button9_Click(object sender, EventArgs e)
        {
            double [,] matrix =
            {
                {0.0,-1.0d / 4.0d ,0.0},
                {-1.0d / 4.0d ,2.0d,-1.0d/4.0d},
                {0.0,-1.0d / 4.0d ,0.0}
            };

            double[,] matrix2 =
            {
                { 1 / 9.0d, 1 / 9.0d, 1 / 9.0d },
                { 1 / 9.0d, 1 / 9.0d, 1 / 9.0d },
                { 1 / 9.0d, 1 / 9.0d, 1 / 9.0d }
            };
            double[,] matrix3 = { { -1.0d, -1.0d, -1.0d }, { -1.0d, 9.0d, -1.0d }, { -1.0d, -1.0d, -1.0d } };
            Bitmap bitmap = (Bitmap)pictureBox1.Image;
            //convolutionFilter(matrix3,bitmap, false,true);
            test(matrix3, true);
        }
        private void convolutionFilter(double [,] matrix, Bitmap bmp, bool ifNeedToBeGrayScaled, bool ifNeedToBeBinarized)
        {
            Bitmap tempBitmap;
            if (ifNeedToBeGrayScaled)
            {
                grayScale(bmp);
            }
            tempBitmap = bmp;
            double sumR = 0.0;
            double sumG = 0.0;
            double sumB = 0.0;
            int r, g, b;
            for(int y = 0; y < bmp.Height - 2; y++){
                for(int x = 0; x< bmp.Width - 2; x++){
                    for(int j = 0; j < 3; j++){
                        for(int i = 0; i < 3; i++) {
                            var pixel = bmp.GetPixel(x + i, y + j);
                            sumR += pixel.R * matrix[i, j];
                            sumG += pixel.G * matrix[i, j];
                            sumB += pixel.B * matrix[i, j];
                        }
                    }
                    sumR = (sumR < 0.0) ? 0.0 : (sumR < 255.0) ? sumR : 255.0;
                    sumG = (sumG < 0.0) ? 0.0 : (sumG < 255.0) ? sumG : 255.0;
                    sumB = (sumB < 0.0) ? 0.0 : (sumB < 255.0) ? sumB : 255.0;
                    tempBitmap.SetPixel(x + 1, y + 1, Color.FromArgb((int)sumR, (int)sumG, (int)sumG));
                    sumR = 0; sumG = 0; sumB = 0;
                }
            }
            if (ifNeedToBeBinarized)
                binarize(tempBitmap);
            pictureBox2.Image = tempBitmap;
        }
        private void binarize(Bitmap bmp)
        {
            Bitmap bitmap = bmp;
            for(var y = 0; y< bmp.Height; y++)
            {
                for(var x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    var value = (pixel.R < 255) ? 0 : 255;
                    bitmap.SetPixel(x, y, Color.FromArgb(pixel.A,value, value, value));
                }
            }
        }
        private void test(double[,] matrix, bool grayscale)
        {
            Bitmap temp;
            var bitmap = new Bitmap(pictureBox1.Image);
            if (grayscale)
            {
                grayScale(bitmap);
            }
            Color color;
            double sumR = 0.0;
            double sumG = 0.0;
            double sumB = 0.0;
            temp = bitmap;
            for (int i = 0; i <= bitmap.Width - 3; i++)
            {
                for (int j = 0; j <= bitmap.Height - 3; j++)
                {
                    for (int x = i; x <= i + 2; x++)
                    {
                        for (int y = j; y <= j + 2; y++)
                        {
                            color = bitmap.GetPixel(x, y);
                            sumR += color.R * matrix[x - i, y - j];
                            sumG += color.G * matrix[x - i, y - j];
                            sumB += color.B * matrix[x - i, y - j];
                        }
                    }
                    sumR = sumR > 0 ? (int)Math.Round(sumR, 10) : 0;
                    sumG = sumG > 0 ? (int)Math.Round(sumG, 10) : 0;
                    sumB = sumB > 0 ? (int)Math.Round(sumB, 10) : 0;

                    temp.SetPixel(i + 1, j + 1, Color.FromArgb(
                             sumR < 255 ? (int)Math.Round(sumR, 10) : 255,
                             sumG < 255 ? (int)Math.Round(sumG, 10) : 255,
                             sumB < 255 ? (int)Math.Round(sumB, 10) : 255));
                    sumR = 0;
                    sumG = 0;
                    sumB = 0;
                }
            }
            bitmap = temp;
            binarize(bitmap);
            pictureBox2.Image = bitmap;
        }
    }
}
