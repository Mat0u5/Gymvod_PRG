using FormsPaint.Properties;
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
        private Painter painter;
        private CanvasUndo undoManager;
        private ShapeDrawer shapeDrawer;
        private FileManager fileManager;
        private PictureBox selectedPictureBox;
        private const string PEN_MODE = "pen";
        private const string ERASER_MODE = "eraser";
        private const string CALLIGRAPHY_MODE = "caligraphyPen";
        private const string ELLIPSE_MODE = "ellipse";
        private const string RECTANGLE_MODE = "rectangle";
        private const string LINE_MODE = "line";
        private const string FLOOD_MODE = "flood";

        public Form1()
        {
            InitializeComponent();
        }

        private void onLoad(object sender, EventArgs e)
        {
            //Initializes all the variables
            painter = new Painter(paintPanel);
            undoManager = new CanvasUndo(painter);
            shapeDrawer = new ShapeDrawer(painter);
            fileManager = new FileManager(painter);
            
            painter.Initialize();
            painter.SetThickness(15);
            painter.SetColor(Color.Blue);

            thicknessSlider.Value = 15;
            selectedPictureBox = penBox_pen;
        }

        private void pictureBoxClickEvent(object sender, EventArgs e)
        {
            //Clicked on a "button" that changes the draw behaviour
            PictureBox pictureBox = (PictureBox)sender;
            
            if (pictureBox.Name.Contains("_"))
            {
                // I made all the "paint" type buttons start with "_" so that i don't have to check for each button name individually
                string mode = pictureBox.Name.Split('_')[1];
                if (painter.CurrentPaintMode == ERASER_MODE && mode != ERASER_MODE)
                {
                    painter.SetColor(currentColorBox.BackColor);
                }
                
                painter.CurrentPaintMode = mode;
                
                if (mode == ERASER_MODE)
                {
                    painter.SetColor(Color.White);
                }
                if (selectedPictureBox != null) selectedPictureBox.BackgroundImage = null;
                pictureBox.BackgroundImage = Resources.selected;
                selectedPictureBox = pictureBox;
            }
            
            if (pictureBox.Equals(loadImageBox))
            {
                fileManager.LoadFile();
            }
            else if (pictureBox.Equals(saveFileBox))
            {
                fileManager.SaveFile();
            }
            else if (pictureBox.Equals(clearCanvasBox))
            {
                painter.ClearCanvas();
                undoManager.SaveState();
            }
            else if (pictureBox.Equals(undoBox))
            {
                undoManager.Undo();
            }
            else if (pictureBox.Equals(currentColorBox))
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    painter.SetColor(colorDialog.Color);
                    currentColorBox.BackColor = colorDialog.Color;
                    colorDialog.Color = painter.SelectedColor;
                }
            }
        }

        private void panelMouseDrawEvent(object sender, MouseEventArgs e)
        {
            // Mouse move event
            float mouseX = e.Location.X;
            float mouseY = e.Location.Y;
            
            if (e.Button == MouseButtons.Left)
            {
                //Left mouse button pressed
                if (!painter.IsStrokeStarted)
                {
                    //Starts the stroke, or fills if the fill bucket is selected
                    painter.StartStroke(mouseX, mouseY);
                    
                    if (painter.CurrentPaintMode == FLOOD_MODE)
                    {
                        painter.FloodFill(new Point((int)mouseX, (int)mouseY));
                    }
                }

                if (painter.CurrentPaintMode == PEN_MODE || painter.CurrentPaintMode == ERASER_MODE || painter.CurrentPaintMode == CALLIGRAPHY_MODE)
                {
                    painter.DrawLine(mouseX, mouseY);
                }
                else if (painter.CurrentPaintMode == ELLIPSE_MODE || painter.CurrentPaintMode == RECTANGLE_MODE || painter.CurrentPaintMode == LINE_MODE)
                {
                    shapeDrawer.DrawShape(mouseX, mouseY, painter.CurrentPaintMode, true);
                }
            }
            
            if (e.Button != MouseButtons.Left && painter.LastMouseButton == MouseButtons.Left)
            {
                //End of a stroke - draws the shape if selected, and saves the current canvas to be able to undo the move
                if (painter.CurrentPaintMode == ELLIPSE_MODE || painter.CurrentPaintMode == RECTANGLE_MODE || painter.CurrentPaintMode == LINE_MODE)
                {
                    shapeDrawer.DrawShape(mouseX, mouseY, painter.CurrentPaintMode, false);
                }
                
                undoManager.SaveState();
                painter.EndStroke();
            }
            
            painter.UpdateLastPosition(mouseX, mouseY, e.Button);
        }

        private void sliderEvent(object sender, EventArgs e)
        {
            //Thickness slider changed value - gotta match the text box thickness display
            System.Windows.Forms.TrackBar slider = (System.Windows.Forms.TrackBar)sender;
            
            if (slider.Equals(thicknessSlider))
            {
                painter.SetThickness(slider.Value);
                thicknessTextBox.Text = slider.Value.ToString();
            }
        }

        private void thicknessTextBoxChange(object sender, EventArgs e)
        {
            //Text box thicknesschanged value - gotta match the slider thickness
            if (int.TryParse(thicknessTextBox.Text, out int newThickness))
            {
                painter.SetThickness(newThickness);
                
                if (newThickness >= thicknessSlider.Minimum && newThickness <= thicknessSlider.Maximum)
                {
                    thicknessSlider.Value = newThickness;
                }
                else if (newThickness < thicknessSlider.Minimum)
                {
                    thicknessSlider.Value = thicknessSlider.Minimum;
                }
                else if (newThickness > thicknessSlider.Maximum)
                {
                    thicknessSlider.Value = thicknessSlider.Maximum;
                }
            }
            else
            {
                thicknessTextBox.Text = string.Empty;
            }
        }
    }
}
