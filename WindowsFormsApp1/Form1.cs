using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    //TODO: random dildering ordered dildering uniform color quantization median cut color quantization
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
            comboBox2.SelectedIndex = 0;
        }
        #region inverse filter
        private void button6_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = (Bitmap)globalBitmap.Clone();
            pictureBox2.Image = InversionFilter(bitmap);
        }
        private Bitmap InversionFilter(Bitmap bitmap)
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
            pictureBox2.Image = BrightnessConvertion(bitmap, brightnessCoefficient);
        }

        private Bitmap BrightnessConvertion(Bitmap bmp, int brightnessCoefficient)
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
                    var r = (ContrastEnchancement((double)pixel.R, contrastCoefficient) < 0) ? 0 :
                        (ContrastEnchancement((double)pixel.R, contrastCoefficient) < 255.0) ? (int)ContrastEnchancement((double)pixel.R, contrastCoefficient) : 255;
                    var g = (ContrastEnchancement((double)pixel.G, contrastCoefficient) < 0) ? 0 :
                        (ContrastEnchancement((double)pixel.G, contrastCoefficient) < 255.0) ? (int)ContrastEnchancement((double)pixel.G, contrastCoefficient) : 255;
                    var b = (ContrastEnchancement((double)pixel.B, contrastCoefficient) < 0) ? 0 :
                        (ContrastEnchancement((double)pixel.B, contrastCoefficient) < 255.0) ? (int)ContrastEnchancement((double)pixel.B, contrastCoefficient) : 255;
                    bitmap.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                }
            }
            return bitmap;
        }
        private double ContrastEnchancement(double channelValue, double contrastCoefficient)
        {
            return ((((channelValue / 255.0) - 0.5) * contrastCoefficient) + 0.5) * 255.0;
        }
        #endregion
        #region grayscale
        private void button1_Click(object sender, EventArgs e)
        {
            var bitmap = new Bitmap(globalBitmap);
            pictureBox2.Image = GrayScale(bitmap,true);
        }
        private Bitmap GrayScale(Bitmap bmp,bool needToSetPicture)
        {
            Bitmap bitmap = bmp;
            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    var sumOfChannelValues = 0.3 * pixel.R + 0.6 * pixel.G + 0.1 * pixel.B;
                    var grayScaleValueOfThePixel = (sumOfChannelValues < 255) ? sumOfChannelValues : 255;
                    bitmap.SetPixel(x, y, Color.FromArgb(pixel.A, (int)grayScaleValueOfThePixel, (int)grayScaleValueOfThePixel, (int)grayScaleValueOfThePixel));
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
            int offset = 0;
            //indentify filter
            int[,] matrix = { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };
            Bitmap bitmap = new Bitmap(globalBitmap);
            //blur
            if (comboBox1.SelectedIndex == 0)
            {
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
                offset = 127;
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
            ConvolutionFilter(matrix, bitmap, divisor,offset);
        }
        private void ConvolutionFilter(int[,] matrix, Bitmap bmp, int divisor,int offset)
        {
            //uwzglednic border pixele
            Bitmap tempBitmap = new Bitmap(globalBitmap);
            double sumR = 0.0;
            double sumG = 0.0;
            double sumB = 0.0;
            var startX = 1;
            var startY = 1;
            for (int y = 0; y < bmp.Height; y++) {
                for (int x = 0; x < bmp.Width; x++) {
                    sumR = 0; sumG = 0; sumB = 0;
                    for (int j = 0; j < 3; j++) {
                        for (int i = 0; i < 3; i++) {

                            var mx = x + i - startX;
                            var my = y + j - startY;

                            mx = (mx < 0) ? 0 : (mx >= bmp.Width) ? bmp.Width - 1 : mx;
                            my = (my < 0) ? 0 : (my >= bmp.Height) ? bmp.Height - 1 : my;

                            var pixel = globalBitmap.GetPixel(mx, my);
                            
                            sumR += pixel.R * matrix[j, i];
                            sumG += pixel.G * matrix[j, i];
                            sumB += pixel.B * matrix[j, i];
                        }
                    }
                    sumR /= divisor; sumG /= divisor; sumB /= divisor;
                    sumR += offset; sumG += offset; sumB += offset;
                    sumR = (sumR < 0.0) ? 0.0 : (sumR < 255.0) ? sumR : 255.0;
                    sumG = (sumG < 0.0) ? 0.0 : (sumG < 255.0) ? sumG : 255.0;
                    sumB = (sumB < 0.0) ? 0.0 : (sumB < 255.0) ? sumB : 255.0;
                    tempBitmap.SetPixel(x, y, Color.FromArgb((int)sumR, (int)sumG, (int)sumB));
                    sumR = 0; sumG = 0; sumB = 0;
                }
            }
            pictureBox2.Image = tempBitmap;
        }
        #endregion
        #region chart
        private void ChartSetup()
        {
            //TODO: implement to select given filter and then perform operation based on it
            Bitmap grayScaleBitmap = (Bitmap)globalBitmap.Clone();
            Bitmap processedBitmap = (Bitmap)globalBitmap.Clone();
            processedBitmap = BrightnessConvertion(processedBitmap,20);
            this.chart1.Series.Add("Function");
            var processedSeries = this.chart1.Series["Function"];
            processedSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line ;
            processedSeries.MarkerStyle = MarkerStyle.Circle;
            processedSeries.MarkerColor = Color.Red;
            processedSeries.MarkerSize = 5;
            processedSeries.BorderWidth = 5;
            chart1.ChartAreas[0].AxisX.Minimum = -1;
            chart1.ChartAreas[0].AxisY.Minimum = -1;
            chart1.ChartAreas[0].AxisX.Interval = 32;
            chart1.ChartAreas[0].AxisY.Maximum = 255;
            chart1.ChartAreas[0].AxisX.Maximum = 255;
            chart1.ChartAreas[0].AxisY.Interval = 32;
            processedSeries.ToolTip = "X =#VALX, Y =#VALY";

 
            processedSeries.Points.AddXY(0, 0);
            processedSeries.Points.AddXY(255, 255);
        }
        //Class level variables holding moveable parts
        DataPoint currentPoint = null;
        //Generate graph button
        private void button2_Click(object sender, EventArgs e)
        {
            ChartSetup();
            button5.Enabled = true;
            button2.Enabled = false;
            button4.Enabled = true;
            var processedSeries = this.chart1.Series["Function"];
            processedSeries.Sort(PointSortOrder.Ascending, "X");
            foreach (var p in processedSeries.Points)
                Console.WriteLine(p.ToString());
        }
        //global variable for edit mode
        bool editEnabled = false;
        #region drag and drop on chart
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left) && editEnabled)
            {
                ChartArea chartArea = chart1.ChartAreas[0];
                Axis xAxis = chartArea.AxisX;
                Axis yAxis = chartArea.AxisY;
                HitTestResult hit = chart1.HitTest(e.X, e.Y);
                if (hit.PointIndex >= 0)
                {
                    if (hit.PointIndex > 255 || hit.PointIndex < 0)
                    {
                        MessageBox.Show("Value on x and y axis have to be between 0 and 255");
                    }
                    else
                    {
                        currentPoint = hit.Series.Points[hit.PointIndex];
                    }
                }
                if (currentPoint != null && currentPoint.XValue != 0 && currentPoint.XValue != 255)
                {
                    Series series = hit.Series;
                    int dx = (int)xAxis.PixelPositionToValue(e.X);
                    int dy = (int)yAxis.PixelPositionToValue(e.Y);
                    currentPoint.XValue = dx;
                    currentPoint.YValues[0] = dy;
                }
                else if (currentPoint != null && currentPoint.XValue != 0 && currentPoint.XValue == 255)
                {
                    Series series = hit.Series;
                    int dx = (int)xAxis.PixelPositionToValue(e.X);
                    int dy = (int)yAxis.PixelPositionToValue(e.Y);
                    currentPoint.XValue = 255;
                    currentPoint.YValues[0] = dy;
                }
                else if (currentPoint != null && currentPoint.XValue == 0 && currentPoint.XValue != 255)
                {
                    Series series = hit.Series;
                    int dx = (int)xAxis.PixelPositionToValue(e.X);
                    int dy = (int)yAxis.PixelPositionToValue(e.Y);
                    currentPoint.XValue = 0;
                    currentPoint.YValues[0] = dy;
                }
            }
        }

        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            currentPoint = null;
        }
        #endregion
        //Clean chart button
        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap processedBitmap = (Bitmap)globalBitmap.Clone();
            pictureBox3.Image = processedBitmap;
            chart1.Series.Clear();
            this.chart1.Series.Add("Function");
            var processedSeries = this.chart1.Series["Function"];
            processedSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            processedSeries.MarkerStyle = MarkerStyle.Circle;
            processedSeries.MarkerColor = Color.Red;
            processedSeries.MarkerSize = 5;
            processedSeries.BorderWidth = 5;
            chart1.ChartAreas[0].AxisX.Minimum = -1;
            chart1.ChartAreas[0].AxisY.Minimum = -1;
            chart1.ChartAreas[0].AxisX.Interval = 32;
            chart1.ChartAreas[0].AxisY.Maximum = 255;
            chart1.ChartAreas[0].AxisX.Maximum = 255;
            chart1.ChartAreas[0].AxisY.Interval = 32;
            processedSeries.Points.AddXY(0, 0);
            processedSeries.Points.AddXY(255, 255);
        }
        //Edit chart button
        private void button5_Click(object sender, EventArgs e)
        {
            var processedSeries = this.chart1.Series["Function"];
            processedSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            button12.Enabled = true;
            button11.Enabled = true;
            editEnabled = true;
            button4.Enabled = false;
            button13.Enabled = true;
        }
        //Apply changes button
        private void button11_Click(object sender, EventArgs e)
        {
            var processedSeries = this.chart1.Series["Function"];
            processedSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            processedSeries.Sort(PointSortOrder.Ascending, "X");
            foreach (var p in processedSeries.Points)
                Console.WriteLine(p.ToString());
            button12.Enabled = false;
            button11.Enabled = false;
            editEnabled = false;
            button4.Enabled = true;
            button13.Enabled = false;
        }
        //Add point button
        private void button12_Click(object sender, EventArgs e)
        {
            var processedSeries = this.chart1.Series["Function"];
            var x = (textBox1.Text != "" ) ? Int16.Parse(textBox1.Text) : -1;
            var y = (textBox2.Text != "" ) ? Int16.Parse(textBox2.Text) : -1;
            bool canAdd = true;
            if (x == -1 || y == -1) MessageBox.Show("Cannot add point without values", "Error");
            else if (x >= 0 && x < 256 && y >= 0 && y < 256)
            {
                foreach (var point in processedSeries.Points)
                {
                    if (point.XValue == x)
                    {
                        MessageBox.Show("Two points cannot have the same X value", "Error");
                        canAdd = false;
                    }
                }
                if(canAdd)processedSeries.Points.AddXY(x, y);
            }
            else
            {
                MessageBox.Show("Point coordinates have to be between 0 and 255", "Error");
            }
            processedSeries.Sort(PointSortOrder.Ascending,"X");
            foreach (var p in processedSeries.Points)
                Console.WriteLine(p.ToString());
        }
        //create lookup function
        private void createLookupTableFromFunction(DataPoint startPoint, DataPoint endPoint)
        {
            double coeff = (endPoint.YValues[0] - startPoint.YValues[0])/(endPoint.XValue - startPoint.XValue);
            double b = startPoint.YValues[0] - coeff*startPoint.XValue;
            int range = (int)endPoint.XValue - (int)startPoint.XValue;
            var x = startPoint.XValue;
            for (var i = 0; i < range+1; i++)
            {
                double ax = coeff * x;

                lookupTable[(int)x] = Math.Round(Math.Ceiling((ax + b) * 100) / 100);
                x++;
            }
        }
        double[] lookupTable = new double[256];
        //apply filter button
        private void button4_Click(object sender, EventArgs e)
        {
            int numberOfPoints = chart1.Series["Function"].Points.Count;
            for (int i = 0; i < numberOfPoints - 1; i++)
            {
                createLookupTableFromFunction(chart1.Series["Function"].Points[i], chart1.Series["Function"].Points[i + 1]);
            }
            var bitmap = GrayScale((Bitmap)globalBitmap.Clone(),false);
            
            for (var y = 0; y < bitmap.Height; y++)
            {
                for(var x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    var colorValue = lookupTable[pixel.R];
                    bitmap.SetPixel(x, y, Color.FromArgb((int)colorValue,(int)colorValue,(int)colorValue));
                }
            }
            pictureBox3.Image = bitmap;
        }
        //Delete point
        private void button13_Click(object sender, EventArgs e)
        {
            var processedSeries = this.chart1.Series["Function"];
            var x = (textBox1.Text != "") ? Int16.Parse(textBox1.Text) : -1;
            var y = (textBox2.Text != "") ? Int16.Parse(textBox2.Text) : -1;
            if (x == -1 || y == -1) MessageBox.Show("Cannot add point without values", "Error");
            else if (x >= 0 && x < 256 && y >= 0 && y < 256)
            {
                for (int i = processedSeries.Points.Count - 1; i >= 0; i--)
                {
                    var point = processedSeries.Points.ElementAt(i);
                    if (point.XValue == x)
                        if (point.YValues[0] == y)
                            if ((x != 0 && y != 0) && (x != 255 && y != 255))
                            {
                                processedSeries.Points.Remove(point);
                            }
                            else
                            {
                                MessageBox.Show("Cannot remove end points ", "Error");
                            }
                }
            }
            else
            {
                MessageBox.Show("Point coordinates have to be between 0 and 255", "Error");
            }
            processedSeries.Sort(PointSortOrder.Ascending, "X");
            foreach (var p in processedSeries.Points)
                Console.WriteLine(p.ToString());
        }
        #endregion
        #region labPartL1
        private void button14_Click(object sender, EventArgs e)
        {
            double coeff = Double.Parse(textBox3.Text);
            gammaCorrection(coeff);
        }
        private void gammaCorrection(double coeff)
        {
            var gamma = coeff;
            Bitmap bitmap = (Bitmap)globalBitmap.Clone();
            for(int y = 0; y < bitmap.Height; y++)
            {
                for(int x = 0; x < bitmap.Width; x++)
                {
                    var pixel = globalBitmap.GetPixel(x, y);
                    double r = pixel.R;
                    var g = pixel.G;
                    var b = pixel.B;

                    var newR = 255.0 * Math.Pow((r/255.0),gamma);
                    var newG = 255.0 * Math.Pow((g / 255.0), gamma);
                    var newB = 255.0 * Math.Pow((b / 255.0), gamma);

                    newR = (newR > 255.0) ? 255.0 : newR;
                    newG = (newG > 255.0) ? 255.0 : newG;
                    newB = (newB > 255.0) ? 255.0 : newB;

                    bitmap.SetPixel(x, y, Color.FromArgb(pixel.A, (int)newR, (int)newG, (int)newB));
                }
            }
            pictureBox2.Image = bitmap;
        }
        #endregion
        #region opend dialog file
        private void button15_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "All files |*.*";

                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    pictureBox1.Image = new Bitmap(dlg.FileName);
                    pictureBox2.Image = new Bitmap(dlg.FileName);
                    globalBitmap = new Bitmap(dlg.FileName);
                }
            }
        }
        #endregion
        #region labPartL2
        private void button16_Click(object sender, EventArgs e)
        {
            AvarageDithering();
        }
        private void AvarageDithering()
        {
            Bitmap bitmap =  GrayScale((Bitmap)globalBitmap.Clone(),false);
            int treshold = CalcualteTreshold(bitmap);
            for (int y =0; y < bitmap.Height; y++)
            {
                for(int x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    var avgValue = (pixel.R < treshold) ? 0 : 255;
                    bitmap.SetPixel(x, y, Color.FromArgb(pixel.A, avgValue, avgValue, avgValue));
                }
            }
            pictureBox2.Image = bitmap;
            pictureBox1.Image = GrayScale((Bitmap)globalBitmap.Clone(), false);

        }
        private int CalcualteTreshold(Bitmap bitmap)
        {
            int treshold = 0;
            int sumOfIntensities = 0;
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    sumOfIntensities += pixel.R;
                }
            }
            treshold = sumOfIntensities / (bitmap.Width * bitmap.Height);
            return treshold;
        }
        #endregion

        private void button17_Click(object sender, EventArgs e)
        {
            int grayLevel = 2;
            switch(comboBox2.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    grayLevel = 4;
                    break;
                case 2:
                    grayLevel = 8;
                    break;
                case 3:
                    grayLevel = 16;
                    break;
            }
            RandomDithering(grayLevel);
        }
        private void RandomDithering(int grayLevel)
        {
            Bitmap bitmap = GrayScale((Bitmap)globalBitmap.Clone(), false);
            Random random = new Random();
            for (var y = 0; y < bitmap.Height; y++)
            {
                for(var x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    double value = 0;
                    switch (grayLevel)
                    {
                        case 2:
                            value = (pixel.R < random.Next(0,256)) ? 0 : 255;
                            break;
                        case 4:
                            var treshold1_4 = random.Next(0, 256);
                            var treshold2_4 = random.Next(0, treshold1_4);
                            var treshold3_4 = random.Next(treshold1_4, 256);
                            var r_4 = pixel.R;
                            if (r_4 < treshold2_4) value = 0.0;
                            else if (treshold2_4 < r_4 && r_4 <= treshold1_4) value = (1.0 / 3.0) * 255.0;
                            else if (treshold1_4 < r_4 && r_4 <= treshold3_4) value = (2.0 / 3.0) * 255.0;
                            else value = 255.0;
                            break;
                        case 8:
                            var treshold1_8 = random.Next(0, 256);
                            var treshold2_8 = random.Next(0, treshold1_8);
                            var treshold3_8 = random.Next(treshold1_8, 256);
                            var treshold4_8 = random.Next(treshold2_8, treshold1_8);
                            var treshold5_8 = random.Next(treshold1_8, treshold3_8);
                            var treshold6_8 = random.Next(0, treshold2_8);
                            var treshold7_8 = random.Next(treshold3_8, 256);
                            var r_8 = pixel.R;
                            if (r_8 < treshold6_8) value = 0.0;
                            else if (treshold6_8 < r_8 && r_8 <= treshold2_8) value = (1.0 / 7.0) * 255.0;
                            else if (treshold2_8 < r_8 && r_8 <= treshold4_8) value = (2.0 / 7.0) * 255.0;
                            else if (treshold4_8 < r_8 && r_8 <= treshold1_8) value = (3.0 / 7.0) * 255.0;
                            else if (treshold1_8 < r_8 && r_8 <= treshold5_8) value = (4.0 / 7.0) * 255.0;
                            else if (treshold5_8 < r_8 && r_8 <= treshold3_8) value = (5.0 / 7.0) * 255.0;
                            else if (treshold3_8 < r_8 && r_8 <= treshold7_8) value = (6.0 / 7.0) * 255.0;
                            else value = 255.0;
                            break;
                        case 16:
                            var treshold1_16 = random.Next(0, 256);
                            var treshold2_16 = random.Next(0, treshold1_16);
                            var treshold3_16 = random.Next(treshold1_16, 256);
                            var treshold4_16 = random.Next(0, treshold2_16);
                            var treshold5_16 = random.Next(treshold3_16, 256);
                            var treshold6_16 = random.Next(treshold2_16, treshold1_16);
                            var treshold7_16 = random.Next(treshold1_16, treshold3_16);
                            var treshold8_16 = random.Next(0, treshold4_16);
                            var treshold9_16 = random.Next(treshold5_16, 256);
                            var treshold10_16 = random.Next(treshold4_16, treshold2_16);
                            var treshold11_16 = random.Next(treshold3_16, treshold5_16);
                            var treshold12_16 = random.Next(treshold2_16, treshold6_16);
                            var treshold13_16 = random.Next(treshold7_16, treshold3_16);
                            var treshold14_16 = random.Next(treshold6_16, treshold1_16);
                            var treshold15_16 = random.Next(treshold1_16, treshold7_16);

                            var r_16 = pixel.R;
                            if (r_16 < treshold8_16) value = 0.0;
                            else if (treshold8_16 < r_16 && r_16 <= treshold4_16) value = (1.0 / 15.0) * 255.0;
                            else if (treshold4_16 < r_16 && r_16 <= treshold10_16) value = (2.0 / 15.0) * 255.0;
                            else if (treshold10_16 < r_16 && r_16 <= treshold2_16) value = (3.0 / 15.0) * 255.0;
                            else if (treshold2_16 < r_16 && r_16 <= treshold12_16) value = (4.0 / 15.0) * 255.0;
                            else if (treshold12_16 < r_16 && r_16 <= treshold6_16) value = (5.0 / 15.0) * 255.0;
                            else if (treshold6_16 < r_16 && r_16 <= treshold14_16) value = (6.0 / 15.0) * 255.0;
                            else if (treshold14_16 < r_16 && r_16 <= treshold1_16) value = (7.0 / 15.0) * 255.0;
                            else if (treshold1_16 < r_16 && r_16 <= treshold15_16) value = (8.0 / 15.0) * 255.0;
                            else if (treshold15_16 < r_16 && r_16 <= treshold7_16) value = (9.0 / 15.0) * 255.0;
                            else if (treshold7_16 < r_16 && r_16 <= treshold13_16) value = (10.0 / 15.0) * 255.0;
                            else if (treshold13_16 < r_16 && r_16 <= treshold3_16) value = (11.0 / 15.0) * 255.0;
                            else if (treshold3_16 < r_16 && r_16 <= treshold11_16) value = (12.0 / 15.0) * 255.0;
                            else if (treshold11_16 < r_16 && r_16 <= treshold5_16) value = (13.0 / 15.0) * 255.0;
                            else if (treshold5_16 < r_16 && r_16 <= treshold9_16) value = (14.0 / 15.0) * 255.0;
                            else value = 255.0;
                            break;
                    }
                    bitmap.SetPixel(x, y, Color.FromArgb(pixel.A, (int)value, (int)value, (int)value));
                }
            }
            pictureBox1.Image = GrayScale(globalBitmap, false);
            pictureBox2.Image = bitmap;
        }
    }
}