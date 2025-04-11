using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsPaint
{
    internal class Painter
    {
        private Graphics panelGraphics;
        private Graphics bitmapGraphics;
        private Bitmap currentBitmap;
        public Panel paintPanel;

        public float LastMouseX { get; private set; }
        public float LastMouseY { get; private set; }
        public float StartStrokeX { get; private set; } = float.MinValue;
        public float StartStrokeY { get; private set; } = float.MinValue;
        public float Thickness { get; private set; }
        public Color SelectedColor { get; private set; }
        public string CurrentPaintMode { get; set; } = "pen";
        public MouseButtons LastMouseButton { get; private set; }
        public bool IsStrokeStarted => StartStrokeX != float.MinValue && StartStrokeY != float.MinValue;

        private float[] lastThicknesses;

        public Painter(Panel panel)
        {
            paintPanel = panel;
        }

        public void Initialize()
        {
            currentBitmap = new Bitmap(paintPanel.Width, paintPanel.Height);
            bitmapGraphics = Graphics.FromImage(currentBitmap);
            panelGraphics = paintPanel.CreateGraphics();

            ClearCanvas();
        }

        public void ClearCanvas()
        {
            panelGraphics.Clear(Color.White);
            bitmapGraphics.Clear(Color.White);
        }

        public void SetThickness(float newThickness)
        {
            Thickness = newThickness;
            lastThicknesses = new float[] { Thickness, Thickness, Thickness, Thickness, Thickness };
        }

        public void SetColor(Color newColor)
        {
            SelectedColor = newColor;
        }

        public void StartStroke(float x, float y)
        {
            StartStrokeX = x;
            StartStrokeY = y;
        }

        public void EndStroke()
        {
            LastMouseX = -1;
            LastMouseY = -1;
            StartStrokeX = float.MinValue;
            StartStrokeY = float.MinValue;
            lastThicknesses = new float[] { Thickness, Thickness, Thickness, Thickness, Thickness };
        }

        public void UpdateLastPosition(float x, float y, MouseButtons button)
        {
            LastMouseX = x;
            LastMouseY = y;
            LastMouseButton = button;
        }

        public void DrawLine(float toX, float toY)
        {
            if (LastMouseX < 0 || LastMouseY < 0) return;

            float currentThickness = Thickness;

            if (CurrentPaintMode == "caligraphyPen")
            {
                float[] moveVector = { LastMouseX - toX, LastMouseY - toY };
                float vectorSize = GetVectorSize(moveVector);
                if (vectorSize < 1) vectorSize = 1;

                currentThickness = ((Thickness / (vectorSize / 4) * 16) + lastThicknesses[4] * 8 +
                                   lastThicknesses[3] * 4 + lastThicknesses[2] * 2 + lastThicknesses[1] * 1) / 31;

                if (currentThickness < 2) currentThickness = 2;
                if (currentThickness > Thickness) currentThickness = Thickness;

                for (int i = 1; i < lastThicknesses.Length; i++)
                {
                    lastThicknesses[i - 1] = lastThicknesses[i];
                }

                lastThicknesses[4] = currentThickness;
            }

            Pen pen = new Pen(SelectedColor, currentThickness);
            SolidBrush brush = new SolidBrush(SelectedColor);

            panelGraphics.DrawLine(pen, LastMouseX, LastMouseY, toX, toY);
            bitmapGraphics.DrawLine(pen, LastMouseX, LastMouseY, toX, toY);
            panelGraphics.FillEllipse(brush, toX - currentThickness / 2, toY - currentThickness / 2,
                                    currentThickness, currentThickness);
            bitmapGraphics.FillEllipse(brush, toX - currentThickness / 2, toY - currentThickness / 2,
                                     currentThickness, currentThickness);
        }

        public void DrawSpecificLine(float fromX, float fromY, float toX, float toY)
        {
            Pen pen = new Pen(SelectedColor, Thickness);
            panelGraphics.DrawLine(pen, fromX, fromY, toX, toY);
            bitmapGraphics.DrawLine(pen, fromX, fromY, toX, toY);
        }

        public void RedrawBitmap(Bitmap bitmap)
        {
            if (bitmap == null) return;

            panelGraphics.Clear(Color.White);
            panelGraphics.DrawImage(bitmap, new Point(0, 0));
            currentBitmap = new Bitmap(bitmap);
            bitmapGraphics = Graphics.FromImage(currentBitmap);
        }

        public Bitmap GetCurrentBitmap()
        {
            return currentBitmap;
        }

        public Bitmap CreateBitmapCopy()
        {
            if (currentBitmap == null) return null;
            return new Bitmap(currentBitmap);
        }

        public void FloodFill(Point startPoint)
        {
            Color targetColor = currentBitmap.GetPixel(startPoint.X, startPoint.Y);
            if (targetColor.ToArgb() == SelectedColor.ToArgb()) return;

            List<Point> pointQueue = new List<Point>();
            pointQueue.Add(startPoint);

            while (pointQueue.Count > 0)
            {
                Point current = pointQueue[0];
                pointQueue.RemoveAt(0);
                int leftEdgeX = current.X;
                int pointY = current.Y;

                while (leftEdgeX > 0 && currentBitmap.GetPixel(leftEdgeX - 1, pointY) == targetColor)
                {
                    leftEdgeX--;
                }

                while (leftEdgeX < currentBitmap.Width && currentBitmap.GetPixel(leftEdgeX, pointY) == targetColor)
                {
                    currentBitmap.SetPixel(leftEdgeX, pointY, SelectedColor);

                    if (pointY > 0 && currentBitmap.GetPixel(leftEdgeX, pointY - 1) == targetColor)
                    {
                        pointQueue.Add(new Point(leftEdgeX, pointY - 1));
                    }
                    if (pointY < currentBitmap.Height - 1 && currentBitmap.GetPixel(leftEdgeX, pointY + 1) == targetColor)
                    {
                        pointQueue.Add(new Point(leftEdgeX, pointY + 1));
                    }
                    leftEdgeX++;
                }
            }

            RedrawBitmap(currentBitmap);
        }

        private float GetVectorSize(float[] vector)
        {
            return (float)Math.Sqrt(Math.Pow(vector[0], 2) + Math.Pow(vector[1], 2));
        }
    }
}
