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

        private void DrawShadow(float mouseX, float mouseY, string shapeType)
        {
            Graphics panelGraphics = Graphics.FromImage(painter.GetCurrentBitmap());
            painter.RedrawBitmap(painter.GetCurrentBitmap());

            Pen pen = new Pen(Color.Gray, 2);
            pen.DashPattern = new float[] { 2, 1 };

            if (shapeType == "ellipse")
            {
                panelGraphics.DrawEllipse(pen, painter.StartStrokeX, painter.StartStrokeY,
                                        mouseX - painter.StartStrokeX, mouseY - painter.StartStrokeY);
            }
            else if (shapeType == "rectangle")
            {
                float startX = Math.Min(mouseX, painter.StartStrokeX);
                float startY = Math.Min(mouseY, painter.StartStrokeY);
                float endX = Math.Max(mouseX, painter.StartStrokeX);
                float endY = Math.Max(mouseY, painter.StartStrokeY);

                panelGraphics.DrawRectangle(pen, startX, startY, endX - startX, endY - startY);
            }
            else if (shapeType == "line")
            {
                panelGraphics.DrawLine(pen, painter.StartStrokeX, painter.StartStrokeY, mouseX, mouseY);
            }
        }

        private void DrawFinalShape(float mouseX, float mouseY, float startX, float startY,
                                  float endX, float endY, string shapeType)
        {
            painter.RedrawBitmap(painter.GetCurrentBitmap());

            if (shapeType == "ellipse")
            {
                Pen pen = new Pen(painter.SelectedColor, painter.Thickness);
                Graphics panelGraphics = Graphics.FromImage(painter.GetCurrentBitmap());

                panelGraphics.DrawEllipse(pen, painter.StartStrokeX, painter.StartStrokeY,
                                        mouseX - painter.StartStrokeX, mouseY - painter.StartStrokeY);
            }
            else if (shapeType == "rectangle")
            {
                Pen pen = new Pen(painter.SelectedColor, painter.Thickness);
                Graphics panelGraphics = Graphics.FromImage(painter.GetCurrentBitmap());

                panelGraphics.DrawRectangle(pen, startX, startY, endX - startX, endY - startY);
            }
            else if (shapeType == "line")
            {
                painter.DrawSpecificLine(painter.StartStrokeX, painter.StartStrokeY, mouseX, mouseY);
            }
        }
    }
}
