using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsPaint
{
    internal class ShapeDrawer
    {
        private Painter painter;

        public ShapeDrawer(Painter painter)
        {
            this.painter = painter;
        }

        public void DrawShape(float mouseX, float mouseY, string shapeType, bool isShadow)
        {
            float startX = Math.Min(mouseX, painter.StartStrokeX);
            float startY = Math.Min(mouseY, painter.StartStrokeY);
            float endX = Math.Max(mouseX, painter.StartStrokeX);
            float endY = Math.Max(mouseY, painter.StartStrokeY);

            if (isShadow)
            {
                DrawShadow(mouseX, mouseY, shapeType);
            }
            else
            {
                DrawFinalShape(mouseX, mouseY, startX, startY, endX, endY, shapeType);
            }
        }

        int shadowLastRedraw = 0;
        private void DrawShadow(float mouseX, float mouseY, string shapeType)
        {
            //Draws a dotted object "shadow"
            shadowLastRedraw--;
            if (shadowLastRedraw <= 0)
            {
                painter.DrawBitmap(painter.GetCurrentBitmap());
                shadowLastRedraw = 5;
            }

            Pen pen = new Pen(Color.Gray, 2);
            pen.DashPattern = new float[] { 2, 1 };

            if (shapeType == "ellipse")
            {
                painter.panelGraphics.DrawEllipse(pen, painter.StartStrokeX, painter.StartStrokeY, mouseX - painter.StartStrokeX, mouseY - painter.StartStrokeY);
            }
            else if (shapeType == "rectangle")
            {
                float startX = Math.Min(mouseX, painter.StartStrokeX);
                float startY = Math.Min(mouseY, painter.StartStrokeY);
                float endX = Math.Max(mouseX, painter.StartStrokeX);
                float endY = Math.Max(mouseY, painter.StartStrokeY);

                painter.panelGraphics.DrawRectangle(pen, startX, startY, endX - startX, endY - startY);
            }
            else if (shapeType == "line")
            {
                painter.panelGraphics.DrawLine(pen, painter.StartStrokeX, painter.StartStrokeY, mouseX, mouseY);
            }
        }

        private void DrawFinalShape(float mouseX, float mouseY, float startX, float startY, float endX, float endY, string shapeType)
        {
            //Draws the final object
            painter.DrawBitmap(painter.GetCurrentBitmap());

            if (shapeType == "ellipse")
            {
                Pen pen = new Pen(painter.SelectedColor, painter.Thickness);
                DrawSpecificEllipse(pen, painter.StartStrokeX, painter.StartStrokeY, mouseX - painter.StartStrokeX, mouseY - painter.StartStrokeY);
            }
            else if (shapeType == "rectangle")
            {
                Pen pen = new Pen(painter.SelectedColor, painter.Thickness);
                DrawSpecificRectangle(pen, startX, startY, endX - startX, endY - startY);
            }
            else if (shapeType == "line")
            {
                Pen pen = new Pen(painter.SelectedColor, painter.Thickness);
                DrawSpecificLine(pen, painter.StartStrokeX, painter.StartStrokeY, mouseX, mouseY);
            }
        }

        public void DrawSpecificLine(Pen pen, float fromX, float fromY, float toX, float toY)
        {
            painter.panelGraphics.DrawLine(pen, fromX, fromY, toX, toY);
            painter.bitmapGraphics.DrawLine(pen, fromX, fromY, toX, toY);
        }

        public void DrawSpecificEllipse(Pen pen, float x, float y, float width, float height)
        {
            painter.panelGraphics.DrawEllipse(pen, x, y, width, height);
            painter.bitmapGraphics.DrawEllipse(pen, x, y, width, height);
        }

        public void DrawSpecificRectangle(Pen pen, float x, float y, float width, float height)
        {
            painter.panelGraphics.DrawRectangle(pen, x, y, width, height);
            painter.bitmapGraphics.DrawRectangle(pen, x, y, width, height);
        }
    }
}
