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
        Bitmap globalBitmap;
        public Form1()
        {
            InitializeComponent();
            globalBitmap = Properties.Resources.singapur2;
            pictureBox1.Image = globalBitmap;
            pictureBox2.Image = pictureBox1.Image;
            comboBox1.SelectedIndex = 0;
        }
        #region inverse filter
        private void button6_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = (Bitmap)globalBitmap.Clone();
            pictureBox2.Image = getInversionFilteredBitmap(bitmap);
        }
        private Bitmap getInversionFilteredBitmap(Bitmap bitmap)
        {
            var bmp = bitmap;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    var r = 255 - pixel.R;
                    var g = 255 - pixel.G;
                    var b = 255 - pixel.B;
                    bmp.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                }
            }
            return bmp;
        }
        #endregion
        #region clearButton
        private void button10_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.singapur2;
            pictureBox1.Image = pictureBox2.Image;
        }
        #endregion
        #region brighntess filter
        private void button7_Click(object sender, EventArgs e)
        {
            var bitmap = new Bitmap(globalBitmap);
            var brightnessCoefficient = 30;
            pictureBox2.Image = brightnessConvertion(bitmap, brightnessCoefficient);
        }

        private Bitmap brightnessConvertion(Bitmap bmp, int brightnessCoefficient)
        {
            var temp = bmp;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);

                    var r = (pixel.R + brightnessCoefficient < 0) ? 0 : (pixel.R + brightnessCoefficient < 255) ? (pixel.R + brightnessCoefficient) : 255;
                    var g = (pixel.G + brightnessCoefficient < 0) ? 0 : (pixel.G + brightnessCoefficient < 255) ? (pixel.G + brightnessCoefficient) : 255;
                    var b = (pixel.B + brightnessCoefficient < 0) ? 0 : (pixel.B + brightnessCoefficient < 255) ? (pixel.B + brightnessCoefficient) : 255;
                    temp.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                }
            }
            return temp;
        }
        #endregion
        #region contrast filter
        private void button8_Click(object sender, EventArgs e)
        {
            var contrastCoefficient = 2.0;
            pictureBox2.Image = contrastFilter(contrastCoefficient);
        }
        private Bitmap contrastFilter(double contrastCoefficient)
        {
            var bitmap = new Bitmap(globalBitmap);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    var r = (contrastEnchancement((double)pixel.R, contrastCoefficient) < 0) ? 0 :
                        (contrastEnchancement((double)pixel.R, contrastCoefficient) < 255.0) ? (int)contrastEnchancement((double)pixel.R, contrastCoefficient) : 255;
                    var g = (contrastEnchancement((double)pixel.G, contrastCoefficient) < 0) ? 0 :
                        (contrastEnchancement((double)pixel.G, contrastCoefficient) < 255.0) ? (int)contrastEnchancement((double)pixel.G, contrastCoefficient) : 255;
                    var b = (contrastEnchancement((double)pixel.B, contrastCoefficient) < 0) ? 0 :
                        (contrastEnchancement((double)pixel.B, contrastCoefficient) < 255.0) ? (int)contrastEnchancement((double)pixel.B, contrastCoefficient) : 255;
                    bitmap.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                }
            }
            return bitmap;
        }
        private double contrastEnchancement(double channelValue, double contrastCoefficient)
        {
            return ((((channelValue / 255.0) - 0.5) * contrastCoefficient) + 0.5) * 255.0;
        }
        #endregion
        #region grayscale
        private void button1_Click(object sender, EventArgs e)
        {
            var bitmap = new Bitmap(globalBitmap);
            pictureBox2.Image = grayScale(bitmap,true);
        }
        private Bitmap grayScale(Bitmap bmp,bool needToSetPicture)
        {
            Bitmap bitmap = bmp;
            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    var sumOfChannelValues = (pixel.R + pixel.G + pixel.B) / 3;
                    var grayScaleValueOfThePixel = (sumOfChannelValues < 255) ? sumOfChannelValues : 255;
                    bitmap.SetPixel(x, y, Color.FromArgb(pixel.A, grayScaleValueOfThePixel, grayScaleValueOfThePixel, grayScaleValueOfThePixel));
                }
            }
            return bitmap;
        }
        #endregion
        #region convolution filters
        private void button9_Click(object sender, EventArgs e)
        {
            //default value for divisor
            int divisor = 1;
            //indentify filter
            int[,] matrix = { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };
            Bitmap bitmap = new Bitmap(globalBitmap);
            //blur
            if (comboBox1.SelectedIndex == 0) {
                divisor = 9;
                matrix = new int[,]{
                        { 1,1,1 },
                        { 1,1,1 },
                        { 1,1,1 }
                     };
            }
            //gausian blur
            else if (comboBox1.SelectedIndex == 1)
            {
                divisor = 8;
                matrix = new int[,]{
                        { 0,1,0 },
                        { 1,4,1 },
                        { 0,1,0 }
                     };
            }
            // sharpener
            else if (comboBox1.SelectedIndex == 2)
            {
                matrix = new int[,]{
                        { 0,-1,0 },
                        { -1,5,-1 },
                        { 0,-1,0 }
                     };
            }
            //mean removal
            else if (comboBox1.SelectedIndex == 3)
            {
                matrix = new int[,]{
                        { -1,-1,-1 },
                        { -1,9,-1 },
                        { -1,-1,-1 }
                     };
            }
            //edge detection vertical
            else if (comboBox1.SelectedIndex == 4)
            {
                matrix = new int[,]{
                        { 0,0,0},
                        { -1,1,0},
                        { 0,0,0}
                     };
            }
            //east embos
            else if (comboBox1.SelectedIndex == 5)
            {
                matrix = new int[,]{
                        { -1,-1,-1},
                        { 0,1,0},
                        { 1,1,1}
                     };
            }
            convolutionFilter(matrix, bitmap, divisor);
        }
        private void convolutionFilter(int[,] matrix, Bitmap bmp, int divisor)
        {
            Bitmap tempBitmap = new Bitmap(globalBitmap);
            double sumR = 0.0;
            double sumG = 0.0;
            double sumB = 0.0;
            int r, g, b;
            for (int y = 0; y < bmp.Height - 2; y++) {
                for (int x = 0; x < bmp.Width - 2; x++) {
                    sumR = 0; sumG = 0; sumB = 0;
                    for (int j = 0; j < 3; j++) {
                        for (int i = 0; i < 3; i++) {
                            var pixel = globalBitmap.GetPixel(x + i, y + j);
                            sumR += pixel.R * matrix[j, i];
                            sumG += pixel.G * matrix[j, i];
                            sumB += pixel.B * matrix[j, i];
                        }
                    }
                    sumR /= divisor; sumG /= divisor; sumB /= divisor;
                    sumR = (sumR < 0.0) ? 0.0 : (sumR < 255.0) ? sumR : 255.0;
                    sumG = (sumG < 0.0) ? 0.0 : (sumG < 255.0) ? sumG : 255.0;
                    sumB = (sumB < 0.0) ? 0.0 : (sumB < 255.0) ? sumB : 255.0;
                    tempBitmap.SetPixel(x + 1, y + 1, Color.FromArgb((int)sumR, (int)sumG, (int)sumB));
                    sumR = 0; sumG = 0; sumB = 0;
                }
            }
            pictureBox2.Image = tempBitmap;
        }
        #endregion
        //greyscale image, dla pixeli o wartosci x pokazuje jak wyglada ich wartosc 
        private void getMouseClick()
        {
            
        }
        private void chartSetup()
        {
            Bitmap grayScaleBitmap = (Bitmap)globalBitmap.Clone();
            Bitmap processedBitmap = (Bitmap)globalBitmap.Clone();
            processedBitmap = contrastFilter(1.6);
            pictureBox2.Image = processedBitmap;
            this.chart1.Series.Add("Function");
            chart1.Series.Add("Original");
            var origianlSeries = this.chart1.Series["Original"];
            var processedSeries = this.chart1.Series["Function"];
            processedSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            origianlSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Interval = 5;
            chart1.ChartAreas[0].AxisY.Maximum = 255;
            chart1.ChartAreas[0].AxisX.Maximum = 255;
            chart1.ChartAreas[0].AxisY.Interval = 5;
            for (var y = 0; y < globalBitmap.Height; y++)
            {
                for(var x = 0; x < globalBitmap.Width; x++)
                {
                    var basePixel = globalBitmap.GetPixel(x, y);
                    var grayScalePixel = grayScaleBitmap.GetPixel(x, y);
                    var processedPixel = processedBitmap.GetPixel(x, y);
                    processedSeries.Points.AddXY(grayScalePixel.R, processedPixel.R);
                    origianlSeries.Points.AddXY(basePixel.R, basePixel.R);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chartSetup();
        }
    }
}