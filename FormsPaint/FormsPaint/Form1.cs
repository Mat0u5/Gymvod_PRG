using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace FormsPaint
{
    public partial class Form1 : Form
    {
        Graphics panelGraphics;
        Graphics bitmapGraphics;
        float lastMouseX;
        float lastMouseY;
        float startStrokeMouseX = float.MinValue;
        float startStrokeMouseY = float.MinValue;
        float thickness;
        Color selectedColor;
        Bitmap currentBitmap;
        List<Bitmap> panelBitmapHistory = new List<Bitmap>();
        MouseButtons lastMouseMoveButton;
        string currentPaintMode = "pen";
        List<Point> floodFillQueue = new List<Point>();


        public Form1()
        {
            InitializeComponent();
        }


        private void onLoad(object sender, EventArgs e)
        {
            currentBitmap = new Bitmap(paintPanel.Width, paintPanel.Height);
            bitmapGraphics = Graphics.FromImage(currentBitmap);
            panelGraphics = paintPanel.CreateGraphics();
            saveBitmapToList();
            changeThickness(15);
            changeColor(Color.Blue);
            panelGraphics.Clear(Color.White);
            bitmapGraphics.Clear(Color.White);
        }
        private void pictureBoxClickEvent(object sender, EventArgs e)
        {
            PictureBox picturebox = (PictureBox)sender;
            if (picturebox.Name.Contains("_"))
            {
                if (currentPaintMode == "eraser" && picturebox.Name.Split('_')[1] != "eraser")
                {
                    Console.WriteLine("noer");
                    selectedColor = currentColorBox.BackColor;
                }
                currentPaintMode = picturebox.Name.Split('_')[1];
                if (currentPaintMode == "eraser")
                {
                    Console.WriteLine("er");
                    selectedColor = Color.White;
                }
            }
            if (picturebox.Equals(loadImageBox))
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Title = "Load Image";
                dialog.Filter = "Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap image = new Bitmap(dialog.FileName);
                    float scaleX = (float)image.Width / (float)paintPanel.Width;
                    float scaleY = (float)image.Height / (float)paintPanel.Height;
                    float scale = Math.Max(scaleX, scaleY);
                    Console.WriteLine(scaleX);
                    Console.WriteLine(scaleY);
                    Console.WriteLine(scale);

                    panelGraphics.DrawImage(image,new Rectangle(0,0, Convert.ToInt32(image.Width/ scale), Convert.ToInt32(image.Height / scale)));
                    bitmapGraphics.DrawImage(image, new Rectangle(0, 0, Convert.ToInt32(image.Width / scale), Convert.ToInt32(image.Height / scale)));
                }
            }
            if (picturebox.Equals(saveFileBox))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "formsPaintSave";
                dialog.DefaultExt = "bmp";
                dialog.ValidateNames = true;
                dialog.Filter = "Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf";


                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.FileName.EndsWith(".bmp")) currentBitmap.Save(dialog.FileName, ImageFormat.Bmp);
                    if (dialog.FileName.EndsWith(".jpeg")) currentBitmap.Save(dialog.FileName, ImageFormat.Jpeg);
                    if (dialog.FileName.EndsWith(".png")) currentBitmap.Save(dialog.FileName, ImageFormat.Png);
                    if (dialog.FileName.EndsWith(".tiff")) currentBitmap.Save(dialog.FileName, ImageFormat.Tiff);
                    if (dialog.FileName.EndsWith(".wmf")) currentBitmap.Save(dialog.FileName, ImageFormat.Wmf);
                }
            }
            if (picturebox.Equals(clearCanvasBox))
            {
                panelGraphics.Clear(Color.White);
                bitmapGraphics.Clear(Color.White);
                saveBitmapToList();
            }
            if (picturebox.Equals(undoBox))
            {
                redrawBitmapToPanel(getLastBitmap(true));
            }
            if (picturebox.Equals(currentColorBox))
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    changeColor(colorDialog.Color);
                }
            }
        }

        private void saveBitmapToList()
        {
            Bitmap bitmap = new Bitmap(currentBitmap);
            panelBitmapHistory.Add(bitmap);
        }
        private void redrawBitmapToPanel(Bitmap bmap)
        {
            if (bmap == null) return;
            panelGraphics.Clear(Color.White);
            panelGraphics.DrawImage(bmap, new Point(0, 0));
            currentBitmap = new Bitmap(bmap);
            bitmapGraphics = Graphics.FromImage(currentBitmap);

        }
        private Bitmap getLastBitmap(bool deleteAccessedMap)
        {
            if (panelBitmapHistory.Count < 2) return null;
            Bitmap lastBitmap = panelBitmapHistory[panelBitmapHistory.Count - 2];
            if (deleteAccessedMap) panelBitmapHistory.RemoveAt(panelBitmapHistory.Count - 1);
            Console.WriteLine("Maps Saved: " + panelBitmapHistory.Count);
            return lastBitmap;
        }
        private void panelMouseDrawEvent(object sender, MouseEventArgs e)
        {
            float mouseX = e.Location.X;
            float mouseY = e.Location.Y;
            if (e.Button == MouseButtons.Left)
            {
                if (startStrokeMouseX == float.MinValue && startStrokeMouseY == float.MinValue)
                {

                    startStrokeMouseX = mouseX;
                    startStrokeMouseY = mouseY;
                    if (currentPaintMode == "flood")
                    {
                        scanlineFill(new Point((int)mouseX, (int)mouseY));
                        redrawBitmapToPanel(currentBitmap);
                    }
                }

                if (currentPaintMode == "pen" || currentPaintMode == "eraser") drawLine(mouseX, mouseY, "lineEllipse");
                if (currentPaintMode == "ellipse" || currentPaintMode == "rectangle" || currentPaintMode == "line")
                {
                    drawShape(mouseX, mouseY, currentPaintMode, true, false);
                }
            }
            if (e.Button != MouseButtons.Left && lastMouseMoveButton == MouseButtons.Left)
            {
                if (currentPaintMode == "ellipse" || currentPaintMode == "rectangle" || currentPaintMode == "line") drawShape(mouseX, mouseY, currentPaintMode, false, false);
                Console.WriteLine("adding...");
                saveBitmapToList();
                lastMouseX = -1;
                lastMouseY = -1;
                startStrokeMouseX = float.MinValue;
                startStrokeMouseY = float.MinValue;
            }
            lastMouseMoveButton = e.Button;
            lastMouseX = mouseX;
            lastMouseY = mouseY;
        }
        private void drawShape(float mouseX, float mouseY, String shape, bool isShadow, bool onlyRegular)
        {
            float startStrokeX = Math.Min(mouseX, startStrokeMouseX);
            float startStrokeY = Math.Min(mouseY, startStrokeMouseY);
            float endStrokeX = Math.Max(mouseX, startStrokeMouseX);
            float endStrokeY = Math.Max(mouseY, startStrokeMouseY);
            if (isShadow)
            {
                /*Rectangle imageBounds = new Rectangle((int)startStrokeX, (int)startStrokeY, (int)endStrokeX - (int)startStrokeX, (int)endStrokeY - (int)startStrokeY);
                panelGraphics.DrawImage(Properties.Resources.pngfind_com_dotted_circle_png_65610, imageBounds);*/

                redrawBitmapToPanel(currentBitmap);
                Pen pen = new Pen(Color.Gray, 2);
                pen.DashPattern = new float[] { 2, 1 };
                if (shape == "ellipse") panelGraphics.DrawEllipse(pen, startStrokeMouseX, startStrokeMouseY, mouseX - startStrokeMouseX, mouseY - startStrokeMouseY);
                if (shape == "rectangle") panelGraphics.DrawRectangle(pen, startStrokeX, startStrokeY, endStrokeX - startStrokeX, endStrokeY - startStrokeY);
                if (shape == "line") panelGraphics.DrawLine(pen, startStrokeMouseX, startStrokeMouseY, mouseX, mouseY);

            }
            else
            {
                redrawBitmapToPanel(currentBitmap);
                Pen pen = new Pen(selectedColor, thickness);
                if (shape == "ellipse")
                {
                    panelGraphics.DrawEllipse(pen, startStrokeMouseX, startStrokeMouseY, mouseX - startStrokeMouseX, mouseY - startStrokeMouseY);
                    bitmapGraphics.DrawEllipse(pen, startStrokeMouseX, startStrokeMouseY, mouseX - startStrokeMouseX, mouseY - startStrokeMouseY);
                }
                if (shape == "rectangle")
                {
                    panelGraphics.DrawRectangle(pen, startStrokeX, startStrokeY, endStrokeX - startStrokeX, endStrokeY - startStrokeY);
                    bitmapGraphics.DrawRectangle(pen, startStrokeX, startStrokeY, endStrokeX - startStrokeX, endStrokeY - startStrokeY);
                }
                if (shape == "line")
                {
                    drawLine(startStrokeMouseX, startStrokeMouseY, mouseX, mouseY, "onlyLine");
                }
            }

        }
        private void drawLine(float mouseX, float mouseY, String drawType)
        {
            drawLine(lastMouseX, lastMouseY, mouseX, mouseY, drawType);
        }//calls the drawLine with more parameters
        private void drawLine(float fromX, float fromY, float toX, float toY, String drawType)
        {
            Pen pen = new Pen(selectedColor, thickness);
            SolidBrush brush = new SolidBrush(selectedColor);
            if (drawType == "lineEllipse") lineEllipse();
            if (drawType == "onlyEllipse") onlyEllipse();
            if (drawType == "onlyLine") onlyLine();
            void lineEllipse()
            {
                panelGraphics.DrawLine(pen, fromX, fromY, toX, toY);
                panelGraphics.FillEllipse(brush, toX - thickness / 2, toY - thickness / 2, thickness, thickness);
                bitmapGraphics.DrawLine(pen, fromX, fromY, toX, toY);
                bitmapGraphics.FillEllipse(brush, toX - thickness / 2, toY - thickness / 2, thickness, thickness);
            }
            void onlyLine()
            {
                panelGraphics.DrawLine(pen, fromX, fromY, toX, toY);
                bitmapGraphics.DrawLine(pen, fromX, fromY, toX, toY);
            }
            void onlyEllipse()
            {
                float[] vector = { fromX - toX, fromY - toY };
                float vectorSize = getVectorSize(vector);
                if (vectorSize == 0) vectorSize = 1;
                float[] stepVector = { vector[0] / vectorSize, vector[1] / vectorSize };
                for (int i = 0; i < Math.Ceiling(vectorSize); i++)
                {
                    float currentX = toX + stepVector[0] * i;
                    float currentY = toY + stepVector[1] * i;
                    panelGraphics.FillEllipse(brush, currentX - thickness / 2, currentY - thickness / 2, thickness, thickness);
                }
            }
        }
        private float getVectorSize(float[] vector)
        {
            return (float)Math.Sqrt(Math.Pow(vector[0], 2) + Math.Pow(vector[1], 2));
        }
        private void sliderEvent(object sender, EventArgs e)
        {
            Console.WriteLine("slider");
            System.Windows.Forms.TrackBar slider = (System.Windows.Forms.TrackBar)sender;
            if (slider.Equals(thicknessSlider))
            {
                changeThickness(slider.Value);
            }
        }
        private void scanlineFill(Point startPoint)
        {
            Color targetColor = currentBitmap.GetPixel(startPoint.X, startPoint.Y);
            if (targetColor.ToArgb() == selectedColor.ToArgb()) return;//prevent loop

            List<Point> pointQueue = new List<Point>();
            pointQueue.Add(startPoint);

            while (pointQueue.Count > 0)
            {
                Point current = pointQueue[0];
                pointQueue.RemoveAt(0);
                int leftEdgeX = current.X;
                int pointY = current.Y;

                //find the left edge
                while (leftEdgeX > 0 && currentBitmap.GetPixel(leftEdgeX - 1, pointY) == targetColor)
                {
                    leftEdgeX--;
                }

                //fill the line
                while (leftEdgeX < currentBitmap.Width && currentBitmap.GetPixel(leftEdgeX, pointY) == targetColor)
                {
                    currentBitmap.SetPixel(leftEdgeX, pointY, selectedColor);

                    //check neighboring pixels
                    if (pointY > 0 && currentBitmap.GetPixel(leftEdgeX, pointY - 1) == targetColor)
                    {
                        pointQueue.Add(new Point(leftEdgeX, pointY - 1)); //pixel above
                    }
                    if (pointY < currentBitmap.Height - 1 && currentBitmap.GetPixel(leftEdgeX, pointY + 1) == targetColor)
                    {
                        pointQueue.Add(new Point(leftEdgeX, pointY + 1)); //pixel below
                    }
                    leftEdgeX++;
                }
            }
        }//my floodFill algoritgh was too slow so i had to google a faster way
        private void drawQueuedFlood()
        {
            while(floodFillQueue.Count > 0)
            {
                Point fill = floodFillQueue[0];
                currentBitmap.SetPixel(fill.X, fill.Y, selectedColor);
                floodFillQueue.RemoveAt(0);
            }
        }
        private bool isInsidePanel(Point point)
        {
            return point.X >= 0 && point.X < paintPanel.Width && point.Y >= 0 && point.Y < paintPanel.Height;
        }
        private void changeThickness(float newSize)
        {
            thickness = newSize;
            //thicknessTimesSqrtTwo = newSize;
            if (thicknessSlider.Value != newSize)
            {
                if (newSize >= thicknessSlider.Minimum && newSize <= thicknessSlider.Maximum) thicknessSlider.Value = (int)newSize;
                if (newSize < thicknessSlider.Minimum) thicknessSlider.Value = thicknessSlider.Minimum;
                if (newSize > thicknessSlider.Maximum) thicknessSlider.Value = thicknessSlider.Maximum;
            }
            thicknessTextBox.Text = Convert.ToString(newSize);
        }
        private void changeColor(Color newColor)
        {
            selectedColor = newColor;
            colorDialog.Color = newColor;
            if (currentColorBox.BackColor != newColor) currentColorBox.BackColor = newColor;
        }

        private void thicknessTextBoxChange(object sender, EventArgs e)
        {
            Console.WriteLine("box");
            bool isNum = Int32.TryParse(thicknessTextBox.Text, out int newThickness);
            if (!isNum) thicknessTextBox.Text = "";
            changeThickness(newThickness);
        }
    }
}
