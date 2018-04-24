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
    //TODO: median cut color quantization
    public partial class Form1 : Form
    {
        Bitmap globalBitmap;
        Bitmap canvas;
        public Form1()
        {
            InitializeComponent();
            globalBitmap = Properties.Resources.singapur2;
            pictureBox1.Image = globalBitmap;
            pictureBox2.Image = pictureBox1.Image;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            canvas = new Bitmap(canvasPb.Size.Width, canvasPb.Size.Height);
            canvasPb.Image = canvas;
        }
        #region Lab1
        #region lab1Home
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
            pictureBox2.Image = GrayScale(bitmap, true);
        }
        private Bitmap GrayScale(Bitmap bmp, bool needToSetPicture)
        {
            Bitmap bitmap = bmp;
            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    var sumOfChannelValues = 0.3 * pixel.R + 0.6 * pixel.G + 0.1 * pixel.B;
                    var grayScaleValueOfThePixel = (sumOfChannelValues < 255.0) ? sumOfChannelValues : 255.0;
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
            ConvolutionFilter(matrix, bitmap, divisor, offset);
        }
        private void ConvolutionFilter(int[,] matrix, Bitmap bmp, int divisor, int offset)
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
            processedBitmap = BrightnessConvertion(processedBitmap, 20);
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
            var x = (textBox1.Text != "") ? Int16.Parse(textBox1.Text) : -1;
            var y = (textBox2.Text != "") ? Int16.Parse(textBox2.Text) : -1;
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
                if (canAdd) processedSeries.Points.AddXY(x, y);
            }
            else
            {
                MessageBox.Show("Point coordinates have to be between 0 and 255", "Error");
            }
            processedSeries.Sort(PointSortOrder.Ascending, "X");
            foreach (var p in processedSeries.Points)
                Console.WriteLine(p.ToString());
        }
        //create lookup function
        private void createLookupTableFromFunction(DataPoint startPoint, DataPoint endPoint)
        {
            double coeff = (endPoint.YValues[0] - startPoint.YValues[0]) / (endPoint.XValue - startPoint.XValue);
            double b = startPoint.YValues[0] - coeff * startPoint.XValue;
            int range = (int)endPoint.XValue - (int)startPoint.XValue;
            var x = startPoint.XValue;
            for (var i = 0; i < range + 1; i++)
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
            var bitmap = GrayScale((Bitmap)globalBitmap.Clone(), false);

            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    var colorValue = lookupTable[pixel.R];
                    bitmap.SetPixel(x, y, Color.FromArgb((int)colorValue, (int)colorValue, (int)colorValue));
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
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var pixel = globalBitmap.GetPixel(x, y);
                    double r = pixel.R;
                    var g = pixel.G;
                    var b = pixel.B;

                    var newR = 255.0 * Math.Pow((r / 255.0), gamma);
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
        #endregion
        #region Lab2
        #region labPartL2
        private void button16_Click(object sender, EventArgs e)
        {
            AvarageDithering();
        }
        private void AvarageDithering()
        {
            Bitmap bitmap = GrayScale((Bitmap)globalBitmap.Clone(), false);
            int treshold = CalcualteTreshold(bitmap);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
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
        #region lab2Home
        #region random dithering
        private void button17_Click(object sender, EventArgs e)
        {
            int grayLevel = 2;
            switch (comboBox2.SelectedIndex)
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
            RandomDitheringOptimized(grayLevel);
        }
        private void RandomDitheringOptimized(int levelOfGray)
        {
            Bitmap bitmap = GrayScale((Bitmap)globalBitmap.Clone(), false);
            Random random = new Random();
            int[] boundriesArray = new int[levelOfGray - 1];
            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    var channelValue = pixel.R;
                    for (int i = 0; i < levelOfGray - 1; i++)
                    {
                        double startTempBound = i * 255.0 / (levelOfGray - 1);
                        double endTempBound = (i + 1) * 255.0 / (levelOfGray - 1);
                        boundriesArray[i] = random.Next((int)startTempBound, (int)endTempBound);
                    }
                    for (var i = 0; i < levelOfGray - 1; i++)
                    {
                        if (channelValue >= boundriesArray[levelOfGray - 2])
                        {
                            var boundedGrayLevelChannelValue = 255.0;
                            bitmap.SetPixel(x, y, Color.FromArgb(pixel.A, (int)boundedGrayLevelChannelValue, (int)boundedGrayLevelChannelValue, (int)boundedGrayLevelChannelValue));
                            break;
                        }
                        else if (channelValue < boundriesArray[i])
                        {
                            var boundedGrayLevelChannelValue = 255.0 * i / (levelOfGray - 1);
                            bitmap.SetPixel(x, y, Color.FromArgb(pixel.A, (int)boundedGrayLevelChannelValue, (int)boundedGrayLevelChannelValue, (int)boundedGrayLevelChannelValue));
                            break;
                        }
                    }

                }
            }
            pictureBox1.Image = GrayScale(globalBitmap, false);
            pictureBox2.Image = bitmap;
        }
        #endregion
        #region ordered dithering
        private void button18_Click(object sender, EventArgs e)
        {
            int grayLevel = 2;
            switch (comboBox2.SelectedIndex)
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
            int sizeOfMatrix = 2;
            var matrix = dither2Matrix;
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    sizeOfMatrix = 2;
                    matrix = dither2Matrix;
                    break;
                case 1:
                    sizeOfMatrix = 3;
                    matrix = dither3Matrix;
                    break;
                case 2:
                    sizeOfMatrix = 4;
                    matrix = dither4Matrix;
                    break;
                case 3:
                    sizeOfMatrix = 6;
                    matrix = dither6Matrix;
                    break;
            }
            OrderedDithering(matrix, sizeOfMatrix, grayLevel);
        }
        private void OrderedDithering(int[,] ditherMatix, int ditherMatrixSize, int grayLevel)
        {
            var matrix = ditherMatix;
            //TODO : size of dithermatrix = [2,3,4,6]
            Bitmap bitmap = GrayScale((Bitmap)globalBitmap.Clone(), false);
            int[] levels = new int[grayLevel];
            for (int i = 0; i < grayLevel; i++)
            {
                levels[i] = i * 255 / (grayLevel - 1);
            }
            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var ditherMatrixValue = matrix[x % ditherMatrixSize, y % ditherMatrixSize];
                    double tresholdValue = ditherMatrixValue / ((ditherMatrixSize * ditherMatrixSize) + 1d);
                    var pixel = bitmap.GetPixel(x, y);
                    var pixelIntensity = pixel.R / 255.0;
                    var col = Math.Floor((grayLevel - 1) * pixelIntensity);
                    var reminder = (grayLevel - 1) * pixelIntensity - col;
                    if (reminder >= tresholdValue) ++col;
                    var channelValue = levels[(int)col];
                    bitmap.SetPixel(x, y, Color.FromArgb((int)channelValue, (int)channelValue, (int)channelValue));
                }
            }
            pictureBox1.Image = GrayScale(globalBitmap, false);
            pictureBox2.Image = bitmap;
        }
        #endregion
        #region ditherMainTabs n = 2,3,4,6 + dots
        int[,] dither2Matrix = { { 1, 3 }, { 4, 2 } };
        int[,] dither3Matrix = { { 3, 7, 4 }, { 6, 1, 9 }, { 2, 8, 5 } };
        int[,] dither4Matrix = { { 1, 3, 9, 11 }, { 4, 2, 12, 10 }, { 13, 15, 5, 7 }, { 16, 14, 8, 6 } };
        int[,] dither6Matrix = { { 9, 11, 25, 27, 13, 15 }, { 12, 10, 28, 26, 16, 14 }, { 21, 23, 1, 3, 33, 35 }, { 24, 22, 4, 2, 36, 34 }, { 5, 7, 29, 31, 17, 19 }, { 8, 6, 31, 30, 20, 18 } };
        int[,] ditherDotMatrix = { { 12, 5, 6, 13 }, { 4, 0, 1, 7 }, { 11, 3, 2, 8 }, { 15, 10, 9, 14 } };
        #endregion
        #region uniform color quantization
        private void button19_Click(object sender, EventArgs e)
        {
            int rDivisor = 1;
            int gDivisor = 1;
            int bDivisor = 1;
            if (rDivisorTb.Text != null)
            {
                int value;
                rDivisor = Int32.TryParse(rDivisorTb.Text, out value) ? value : 1;
            }
            if (gDivisorTb.Text != null)
            {
                int value;
                gDivisor = Int32.TryParse(gDivisorTb.Text, out value) ? value : 1;
            }
            if (bDivisorTb.Text != null)
            {
                int value;
                bDivisor = Int32.TryParse(bDivisorTb.Text, out value) ? value : 1;
            }
            if (rDivisorTb.Text == "" || gDivisorTb.Text == "" || bDivisorTb.Text == "")
            {
                MessageBox.Show("Cannot leave empty cells for r,g,b divisors", "Error");
            }
            else
                UniformColorQuantization(rDivisor, gDivisor, bDivisor);
        }
        private void UniformColorQuantization(int rDivisor, int gDivisor, int bDivisor)
        {
            Bitmap bitmap = WindowsFormsApp1.Properties.Resources.singapur2;
            var listOfRIntervals = CreateListWithInervals(rDivisor);
            var listOfGIntervals = CreateListWithInervals(gDivisor);
            var listOfBIntervals = CreateListWithInervals(bDivisor);

            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    int r, g, b = 0;
                    r = GetChannelValue(listOfRIntervals, pixel.R);
                    g = GetChannelValue(listOfGIntervals, pixel.G);
                    b = GetChannelValue(listOfBIntervals, pixel.B);
                    bitmap.SetPixel(x, y, Color.FromArgb(pixel.A, r, g, b));
                }
            }
            pictureBox2.Image = bitmap;
        }

        private int GetChannelValue(List<int> listOfChannelIntervals, int pixelRValue)
        {
            int r = 0;
            foreach (var el in listOfChannelIntervals)
            {
                if (pixelRValue > listOfChannelIntervals.ElementAt(listOfChannelIntervals.Count - 1))
                {
                    r = (listOfChannelIntervals.ElementAt(listOfChannelIntervals.Count - 1) + listOfChannelIntervals.ElementAt(listOfChannelIntervals.Count - 2)) / 2;
                    break;
                }
                else if (pixelRValue < el)
                {
                    var it = listOfChannelIntervals.IndexOf(el);
                    r = (listOfChannelIntervals[it] + listOfChannelIntervals[it - 1]) / 2;
                    break;
                }
            }

            return r;
        }

        private List<int> CreateListWithInervals(int divisor)
        {
            List<int> list = new List<int>();
            for (int i = 0; i <= divisor; i++)
            {
                list.Add(((255 * i) / divisor));
            }
            return list;
        }
        #endregion
        #region Median-Cut algorithm
        private void button20_Click(object sender, EventArgs e)
        {
            var value = 0;
            int numberOfColors = Int32.TryParse(textBox4.Text, out value) ? isPowerOfTwo(value) ? value : ShowMessageBox("Input has to be power of two") : ShowMessageBox("Invalid input");
            if(numberOfColors>0) MedianCutQuantization(numberOfColors);

        }
        private int ShowMessageBox(String message) { MessageBox.Show(message,"Error"); return 0; }
        private bool isPowerOfTwo(int value)
        {
            return (Math.Ceiling(Math.Log(value, 2.0)) == Math.Floor(Math.Log(value, 2.0)));
        }
        private List<Color> RecursivelyDivideListintoParts(int depth, int maxDepth, List<Color> listOfPixels)
        {
            if (depth == maxDepth)
            {
                return listOfPixels;
            }
            else if(depth < maxDepth)
            {
                List<Color> returnList = new List<Color>();
                var priority = GetChannelWithBiggestRange(listOfPixels);
                var sortedList = SortWithChannelPriority(listOfPixels, priority);
                var leftList = sortedList.Take(sortedList.Count / 2).ToList();
                var rightList = sortedList.Skip(sortedList.Count / 2).ToList();
                returnList.AddRange(RecursivelyDivideListintoParts(depth + 1, maxDepth, leftList));
                returnList.AddRange(RecursivelyDivideListintoParts(depth + 1, maxDepth, rightList));
                return returnList;
            }
            else
                return null;

        }
        private void MedianCutQuantization(int numberOfColors)
        {
            Bitmap bitmap = WindowsFormsApp1.Properties.Resources.singapur2;
            var listOfPixels = GetListOfPixels(bitmap);
            int depth = (int)Math.Sqrt(numberOfColors);
            var pixels = RecursivelyDivideListintoParts(0, depth, listOfPixels);
            var sizeOfCuboid = pixels.Count / numberOfColors;
            List<Color> listOfColorsForGivenCuboid = new List<Color>();
            int counter = 0;
            int meanR = 0; int meanG = 0; int meanB = 0;
            foreach(var pixel in pixels) { 
                if (counter < sizeOfCuboid-1)
                {
                    var temp = pixel;
                    meanR += temp.R;
                    meanG += temp.G;
                    meanB += temp.B;
                    counter++;
                }
                else
                {
                    listOfColorsForGivenCuboid.Add(Color.FromArgb(255, meanR / sizeOfCuboid, meanG / sizeOfCuboid, meanB / sizeOfCuboid));
                    meanR = 0; meanG = 0; meanB = 0; counter = 0;
                }
            }
            for(var y = 0; y < bitmap.Height; y++)
            {
                for(var x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    Color color = SetColorForGivenPixelAccordingToProperCuboid(pixel,listOfColorsForGivenCuboid);
                    bitmap.SetPixel(x, y, color);
                }
            }
            pictureBox2.Image = bitmap;
        }

        private Color SetColorForGivenPixelAccordingToProperCuboid(Color pixel, List<Color> listOfColorsForGivenCuboid)
        {
            double smallesDist = Math.Pow(255,3);
            Color colorToSet = Color.FromArgb(255,255,255);
            for(int i = 0; i < listOfColorsForGivenCuboid.Count; i++)
            {
                if(Distance(pixel,listOfColorsForGivenCuboid[i]) < smallesDist)
                {
                    smallesDist = Distance(pixel, listOfColorsForGivenCuboid[i]);
                    colorToSet = listOfColorsForGivenCuboid[i];
                }
            }
            return colorToSet;
        }

        private double Distance(Color pixel, Color color)
        {
            return Math.Sqrt(Math.Pow(pixel.R - color.R, 2) + Math.Pow(pixel.G - color.G, 2) + Math.Pow(pixel.B - color.B, 2));
        }

        private List<Color> SortWithChannelPriority(List<Color> listOfPixels, int priority)
        {
            //0->R | 1->G| 2->B
            switch (priority) {
                case 0:
                    var sortedByR = listOfPixels.OrderBy(pixel => pixel.R).ToList();
                    return sortedByR;
                case 1:
                    var sortedByG = listOfPixels.OrderBy(pixel => pixel.G).ToList();
                    return sortedByG;
                case 2:
                    var sortedByB = listOfPixels.OrderBy(pixel => pixel.B).ToList();
                    return sortedByB;
            }
            return null;
        }

        private List<Color> GetListOfPixels(Bitmap bitmap)
        {
            List<Color> list = new List<Color>();
            for(var y = 0; y < bitmap.Height; y++)
            {
                for(var x = 0; x < bitmap.Width; x++)
                {
                    list.Add(bitmap.GetPixel(x, y));
                }
            }
            return list;
        }

        private int GetChannelWithBiggestRange(List<Color> list)
        {
            int channelCode;
            int minR = 255;
            int minG = 255;
            int minB = 255;
            int maxR = 0;
            int maxG = 0;
            int maxB = 0;
            foreach(var pixel in list) { 
                    if (pixel.R < minR) minR = pixel.R;
                    if (pixel.R > maxR) maxR = pixel.R;
                    if (pixel.G < minG) minG = pixel.G;
                    if (pixel.G > maxG) maxG = pixel.G;
                    if (pixel.B > maxB) maxB = pixel.B;
                    if (pixel.B < minB) minB = pixel.B;
                }
            channelCode = CompareChannels(maxR, maxG, maxB, minR, minG, minB);
            // 0-R | 1-G| 2-B
            return channelCode;
        }

        private int CompareChannels(int maxR, int maxG, int maxB, int minR, int minG, int minB)
        {
            int code = 0;
            int rRange = maxR - minR;
            int gRange = maxG - minG;
            int bRange = maxB - minB;
            if (rRange > gRange && rRange > bRange) code = 0;
            else if (gRange > rRange && gRange > bRange) code = 1;
            else if (bRange > rRange && bRange > gRange) code = 2;
            return code;
        }
        #endregion

        #endregion

        #endregion
        #region Lab3

        bool IsSecondClick = false;
        Color color = Color.FromArgb(255, 0, 255);
        Color bgColor = Color.Turquoise;
        PointsTuple tuple = new PointsTuple();

        private class PointsTuple
        {
            int x1;
            int x2;
            int y1;
            int y2;

            public PointsTuple()
            {
                x1 = 0;
                x2 = 0;
                y1 = 0;
                y2 = 0;
            }
            public void checkAndSwapCoordsIfNecessary()
            {
                if (x1 > x2)
                {
                    var tempX = x2;
                    x2 = x1;
                    x1 = tempX;
                    var tempY = y2;
                    y2 = y1;
                    y1 = tempY;
                }
            }
            public int calculateDistance()
            {
                checkAndSwapCoordsIfNecessary();
                var distance = Math.Sqrt((Math.Pow((x1 - x2),2) + Math.Pow((y1 - y2),2)));
                return (int)distance;
            }
            public int X1 { get => x1; set => x1 = value; }
            public int X2 { get => x2; set => x2 = value; }
            public int Y1 { get => y1; set => y1 = value; }
            public int Y2 { get => y2; set => y2 = value; }
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {

            if (!IsSecondClick)
            {
                tuple.X1 = e.X;
                tuple.Y1 = e.Y;
                IsSecondClick = true;
                Console.WriteLine("x : {0}, y: {1} click1", tuple.X1, tuple.Y1);
            }
            else if (IsSecondClick)
            {
                tuple.X2 = e.X;
                tuple.Y2 = e.Y;
                IsSecondClick = false;
                Console.WriteLine("x : {0}, y: {1} click2", tuple.X2, tuple.Y2);
                tuple.checkAndSwapCoordsIfNecessary();
                switch (comboBox4.SelectedIndex)
                {
                    case 0:
                        Bresenham(tuple.X1, tuple.Y1, tuple.X2, tuple.Y2);
                        break;
                    case 1:
                        int radius = tuple.calculateDistance();
                        MidpointCircle(radius);
                        break;
                    case 2:
                        Xiaolin(tuple.X1, tuple.Y1, tuple.X2, tuple.Y2);
                        break;
                    case 3:
                        int wuRadius = tuple.calculateDistance();
                        XiaolinCircles(wuRadius);
                        break;
                }
            }
        }

        private void XiaolinCircles(int wuRadius)
        {
            int x = wuRadius;
            int y = 0;
            try
            {
                canvas.SetPixel(tuple.X1 + wuRadius, tuple.Y1, color);

                canvas.SetPixel(tuple.X1 - wuRadius, tuple.Y1, color);

                canvas.SetPixel(tuple.X1, tuple.Y1 + wuRadius, color);

                canvas.SetPixel(tuple.X1, tuple.Y1 - wuRadius, color);
            }
            catch (Exception e)
            {
                Console.WriteLine("4 points placing " + e);
            }
            while (x > y)
            {
                y += 1;
                x = (int)Math.Ceiling(Math.Sqrt(wuRadius * wuRadius - y * y));
                float T = D(wuRadius, y);
                var r2 = color.R * (1 - T) + bgColor.R * T;
                var g2 = color.G * (1 - T) + bgColor.G * T;
                var b2 = color.B * (1 - T) + bgColor.B * T;
                var color2 = Color.FromArgb((int)r2, (int)g2, (int)b2);

                var r1 = color.R * T + bgColor.R * (1 - T);
                var g1 = color.G * T + bgColor.G * (1 - T);
                var b1 = color.B * T + bgColor.B * (1 - T);
                var color1 = Color.FromArgb((int)r1, (int)g1, (int)b1);
                try
                {
                    canvas.SetPixel(tuple.X1 + x, tuple.Y1 + y, color2);
                    canvas.SetPixel(tuple.X1 + x - 1, tuple.Y1 + y, color1);

                    canvas.SetPixel(tuple.X1 + y, tuple.Y1 + x, color2);
                    canvas.SetPixel(tuple.X1 + y - 1, tuple.Y1 + x, color1);

                    canvas.SetPixel(tuple.X1 + y, tuple.Y1 - x, color2);
                    canvas.SetPixel(tuple.X1 + y - 1, tuple.Y1 - x, color1);

                    canvas.SetPixel(tuple.X1 + x, tuple.Y1 - y, color2);
                    canvas.SetPixel(tuple.X1 + x - 1, tuple.Y1 - y, color1);

                    canvas.SetPixel(tuple.X1 - x, tuple.Y1 - y, color2);
                    canvas.SetPixel(tuple.X1 - x + 1, tuple.Y1 - y, color1);

                    canvas.SetPixel(tuple.X1 - y, tuple.Y1 - x, color2);
                    canvas.SetPixel(tuple.X1 - y + 1, tuple.Y1 - x, color1);

                    canvas.SetPixel(tuple.X1 - y, tuple.Y1 + x, color2);
                    canvas.SetPixel(tuple.X1 - y + 1, tuple.Y1 + x, color1);

                    canvas.SetPixel(tuple.X1 - x, tuple.Y1 + y, color2);
                    canvas.SetPixel(tuple.X1 - x + 1, tuple.Y1 + y, color1);
                }
                catch (Exception e)
                {
                    Console.WriteLine("16 points placing " + e);
                }

            }
            canvasPb.Image = canvas;
        }

        private float D(int wuRadius, int y)
        {
            return (float)Math.Ceiling(Math.Sqrt(wuRadius * wuRadius - y * y)) - (float)Math.Sqrt(wuRadius * wuRadius - y * y);
        }

        private void Xiaolin(int x1, int y1, int x2, int y2)
        {
            float y = y1;
            float dx = x2 - x1;
            float dy = y2 - y1;
            float m = (dy / dx) > 0.0f ? (dy/dx) : 1.0f;
            for (int x = x1; x <= x2; ++x)
            {
                var r1 = color.R * (1 - modf(y)) + bgColor.R*modf(y);
                var g1 = color.G * (1 - modf(y)) + bgColor.G * modf(y);
                var b1 = color.B * (1 - modf(y)) + bgColor.B * modf(y);
                var color1 = Color.FromArgb((int)r1, (int)g1, (int)b1);

                var r2 = color.R * modf(y) + bgColor.R * (1 - modf(y));
                var g2 = color.G * modf(y) + bgColor.G * (1 - modf(y));
                var b2 = color.B * modf(y) + bgColor.B * (1 - modf(y));
                var color2 = Color.FromArgb((int)r2, (int)g2, (int)b2);
                canvas.SetPixel(x, (int)Math.Floor(y), color1);
                canvas.SetPixel(x, (int)Math.Floor(y) + 1, color2);
                y += m;
            }
            canvasPb.Image = canvas;
        }
        private float modf(float x)
        {
            int y = (int)Math.Floor(x);
            return Math.Abs(x - y);
        }
        private void MidpointCircle(int radius)
        {
            int dE = 3;
            int dSE = 5 - 2 * radius;
            int d = 1 - radius;
            int x = 0;
            int y = radius;
            try
            {
                canvas.SetPixel(tuple.X1 + radius, tuple.Y1, color);

                canvas.SetPixel(tuple.X1 - radius, tuple.Y1, color);

                canvas.SetPixel(tuple.X1 , tuple.Y1 + radius, color);

                canvas.SetPixel(tuple.X1 , tuple.Y1 - radius, color);
            }
            catch (Exception e)
            {
                Console.WriteLine("4 points placing " + e);
            }
            while ( y > x)
            {
                if( d < 0 )
                {
                    d += dE;
                    dE += 2;
                    dSE += 2;
                }
                else
                {
                    d += dSE;
                    dE += 2;
                    dSE += 4;
                    y -= 1;
                }
                x += 1;
                try
                {
                    canvas.SetPixel(tuple.X1 + x, tuple.Y1 + y, color);
                    canvas.SetPixel(tuple.X1 + y, tuple.Y1 + x, color);
                    canvas.SetPixel(tuple.X1 + y, tuple.Y1 - x, color);
                    canvas.SetPixel(tuple.X1 + x, tuple.Y1 - y, color);
                    canvas.SetPixel(tuple.X1 - x, tuple.Y1 - y, color);
                    canvas.SetPixel(tuple.X1 - y, tuple.Y1 - x, color);
                    canvas.SetPixel(tuple.X1 - y, tuple.Y1 + x, color);
                    canvas.SetPixel(tuple.X1 - x, tuple.Y1 + y, color);
                }
                catch (Exception e)
                {
                    Console.WriteLine("8 points placing " + e);
                }
            }
            canvasPb.Image = canvas;
        }
        private void Bresenham(int x1, int y1, int x2, int y2)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int d = 2 * dy - dx;
            int dE = 2 * dy;
            int dNE = 2 * (dy - dx);
            int x = x1, y = y1;

            while(x < x2)
            {
                if(d < 0)
                {
                    d += dE;
                    x += 1;
                }
                else
                {
                    d += dNE;
                    x+=1;
                    y+=1;
                }
                canvas.SetPixel(x, y, color);
            }
            canvasPb.Image = canvas;
        }

        //Clear button
        private void button21_Click(object sender, EventArgs e)
        {
            canvas = new Bitmap(canvasPb.Size.Width, canvasPb.Size.Height);
            canvasPb.Image = canvas;
        }
        #endregion
    }
}