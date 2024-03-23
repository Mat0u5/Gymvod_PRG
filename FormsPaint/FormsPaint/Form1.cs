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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        public Form1()
        {
            InitializeComponent();
            /*
            PictureBox picturebox = new PictureBox();
            picturebox.Name = Convert.ToString(square);
            picturebox.BackColor = squareColor;
            picturebox.Size = new Size(squareSize, squareSize);
            picturebox.Location = new Point(x, y);
            picturebox.SizeMode = PictureBoxSizeMode.StretchImage;
            picturebox.Click += (sender, e) => {
                onClick(Convert.ToInt32(picturebox.Name));
            };
            if (piece == "br") picturebox.Image = Properties.Resources.br;*/
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
            testLabel.Text = picturebox.Name;
            if (picturebox.Equals(paintBrushBox))
            {
                if (currentPaintMode == "pen") currentPaintMode = "circle";
                else if (currentPaintMode == "circle") currentPaintMode = "pen";
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
                panelBitmapHistory = new List<Bitmap>();
                saveBitmapToList();
            }
            if (picturebox.Equals(undoBox))
            {
                restoreBitmapToPanel(getLastBitmap(true));
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
        private void restoreBitmapToPanel(Bitmap bmap)
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
                }

                if (currentPaintMode == "pen") drawLine(mouseX, mouseY, "lineEllipse");
                if (currentPaintMode == "circle")
                {
                    drawCircle(mouseX, mouseY, true);
                }
            }
            if (e.Button != MouseButtons.Left && lastMouseMoveButton == MouseButtons.Left)
            {
                if (currentPaintMode == "circle") drawCircle(mouseX, mouseY, false);
                Console.WriteLine("adding...");
                saveBitmapToList();
                lastMouseX = -1;
                lastMouseY = -1;
            }
            lastMouseMoveButton = e.Button;
            lastMouseX = mouseX;
            lastMouseY = mouseY;
        }
        private void drawCircle(float mouseX, float mouseY, bool isShadow)
        {
            float radius = Math.Min(mouseX - startStrokeMouseX, mouseY - startStrokeMouseY);
            
            if (isShadow)
            {
                restoreBitmapToPanel(getLastBitmap(false));
                Pen pen = new Pen(Color.Gray, 2);
                panelGraphics.DrawEllipse(pen, startStrokeMouseX - radius, startStrokeMouseY - radius, radius * 2, radius * 2);
            }
            else
            {
                restoreBitmapToPanel(getLastBitmap(false));
                Pen pen = new Pen(selectedColor, thickness);
                panelGraphics.DrawEllipse(pen, startStrokeMouseX - radius, startStrokeMouseY - radius, radius * 2, radius * 2);
                bitmapGraphics.DrawEllipse(pen, startStrokeMouseX - radius, startStrokeMouseY - radius, radius * 2, radius * 2);
            }
           
        }
        private void drawLine(float mouseX, float mouseY, String drawType)
        {
            Pen pen = new Pen(selectedColor, thickness);
            SolidBrush brush = new SolidBrush(selectedColor);
            if (drawType == "lineEllipse") lineEllipse();
            if (drawType == "customLineEllipse") customLineEllipse();
            if (drawType == "onlyEllipse") onlyEllipse();
            void lineEllipse()
            {
                panelGraphics.DrawLine(pen, lastMouseX, lastMouseY, mouseX, mouseY);
                panelGraphics.FillEllipse(brush, mouseX - thickness / 2, mouseY - thickness / 2, thickness, thickness);
                bitmapGraphics.DrawLine(pen, lastMouseX, lastMouseY, mouseX, mouseY);
                bitmapGraphics.FillEllipse(brush, mouseX - thickness / 2, mouseY - thickness / 2, thickness, thickness);
            }
            void customLineEllipse()
            {
                float[] vector = { lastMouseX - mouseX, lastMouseY - mouseY };
                float[] perpendicularVector = { -vector[1], vector[0] };
                float vectorSize = getVectorSize(vector);
                float perpendicularVectorSize = getVectorSize(perpendicularVector);
                if (vectorSize == 0)
                {
                    panelGraphics.FillEllipse(brush, mouseX - thickness / 2, mouseY - thickness / 2, thickness, thickness);
                    return;
                }
                float multiplyVectorBy = (thickness / 2) / vectorSize;
                perpendicularVector[0] *= multiplyVectorBy;
                perpendicularVector[1] *= multiplyVectorBy;
                Point[] points = {new Point(Convert.ToInt32(lastMouseX + perpendicularVector[0]), Convert.ToInt32(lastMouseY + perpendicularVector[1]))
                        , new Point(Convert.ToInt32(lastMouseX - perpendicularVector[0]),Convert.ToInt32(lastMouseY - perpendicularVector[1]))
                        , new Point(Convert.ToInt32(mouseX - perpendicularVector[0]), Convert.ToInt32(mouseY - perpendicularVector[1]))
                        , new Point(Convert.ToInt32(mouseX + perpendicularVector[0]), Convert.ToInt32(mouseY + perpendicularVector[1])) };
                panelGraphics.FillPolygon(brush, points);
                GraphicsState lastPanelGraphics = panelGraphics.Save();
                panelGraphics.Restore(lastPanelGraphics);
            }
            void onlyEllipse()
            {
                float[] vector = { lastMouseX - mouseX, lastMouseY - mouseY };
                float vectorSize = getVectorSize(vector);
                if (vectorSize == 0) vectorSize = 1;
                float[] stepVector = { vector[0] / vectorSize, vector[1] / vectorSize };
                for (int i = 0; i < Math.Ceiling(vectorSize); i++)
                {
                    float currentX = mouseX + stepVector[0] * i;
                    float currentY = mouseY + stepVector[1] * i;
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
            System.Windows.Forms.TrackBar slider = (System.Windows.Forms.TrackBar)sender;
            if (slider.Equals(thicknessSlider))
            {
                changeThickness(slider.Value);
            }
        }

        private void changeThickness(float newSize)
        {

            thickness = newSize;
            //thicknessTimesSqrtTwo = newSize;
            if (thicknessSlider.Value != newSize) thicknessSlider.Value = (int)newSize;
            thicknessTextBox.Text = Convert.ToString(newSize);
        }
        private void changeColor(Color newColor)
        {
            selectedColor = newColor;
            if (currentColorBox.BackColor != newColor) currentColorBox.BackColor = newColor;
        }
    }
}
