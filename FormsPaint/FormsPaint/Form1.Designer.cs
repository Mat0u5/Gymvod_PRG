namespace FormsPaint
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.paintPanel = new System.Windows.Forms.Panel();
            this.thicknessSlider = new System.Windows.Forms.TrackBar();
            this.thicknessLabel = new System.Windows.Forms.Label();
            this.thicknessTextBox = new System.Windows.Forms.TextBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.floodBox_flood = new System.Windows.Forms.PictureBox();
            this.loadImageBox = new System.Windows.Forms.PictureBox();
            this.eraserBox_eraser = new System.Windows.Forms.PictureBox();
            this.lineBox_line = new System.Windows.Forms.PictureBox();
            this.rectangleBox_rectangle = new System.Windows.Forms.PictureBox();
            this.ellipseBox_ellipse = new System.Windows.Forms.PictureBox();
            this.penBox_pen = new System.Windows.Forms.PictureBox();
            this.saveFileBox = new System.Windows.Forms.PictureBox();
            this.clearCanvasBox = new System.Windows.Forms.PictureBox();
            this.undoBox = new System.Windows.Forms.PictureBox();
            this.currentColorBox = new System.Windows.Forms.PictureBox();
            this.paintBrushBox_brush = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.thicknessSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.floodBox_flood)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eraserBox_eraser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineBox_line)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rectangleBox_rectangle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ellipseBox_ellipse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.penBox_pen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveFileBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clearCanvasBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.undoBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paintBrushBox_brush)).BeginInit();
            this.SuspendLayout();
            // 
            // paintPanel
            // 
            this.paintPanel.BackColor = System.Drawing.Color.White;
            this.paintPanel.Location = new System.Drawing.Point(40, 34);
            this.paintPanel.Name = "paintPanel";
            this.paintPanel.Size = new System.Drawing.Size(699, 544);
            this.paintPanel.TabIndex = 1;
            this.paintPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelMouseDrawEvent);
            this.paintPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelMouseDrawEvent);
            this.paintPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelMouseDrawEvent);
            // 
            // thicknessSlider
            // 
            this.thicknessSlider.Cursor = System.Windows.Forms.Cursors.No;
            this.thicknessSlider.Location = new System.Drawing.Point(793, 310);
            this.thicknessSlider.Maximum = 50;
            this.thicknessSlider.Minimum = 1;
            this.thicknessSlider.Name = "thicknessSlider";
            this.thicknessSlider.Size = new System.Drawing.Size(171, 45);
            this.thicknessSlider.TabIndex = 3;
            this.thicknessSlider.Value = 1;
            this.thicknessSlider.ValueChanged += new System.EventHandler(this.sliderEvent);
            // 
            // thicknessLabel
            // 
            this.thicknessLabel.AutoSize = true;
            this.thicknessLabel.Location = new System.Drawing.Point(790, 286);
            this.thicknessLabel.Name = "thicknessLabel";
            this.thicknessLabel.Size = new System.Drawing.Size(56, 13);
            this.thicknessLabel.TabIndex = 4;
            this.thicknessLabel.Text = "Thickness";
            // 
            // thicknessTextBox
            // 
            this.thicknessTextBox.Location = new System.Drawing.Point(885, 284);
            this.thicknessTextBox.Name = "thicknessTextBox";
            this.thicknessTextBox.Size = new System.Drawing.Size(68, 20);
            this.thicknessTextBox.TabIndex = 5;
            this.thicknessTextBox.TextChanged += new System.EventHandler(this.thicknessTextBoxChange);
            // 
            // floodBox_flood
            // 
            this.floodBox_flood.Image = global::FormsPaint.Properties.Resources.bucket;
            this.floodBox_flood.Location = new System.Drawing.Point(885, 108);
            this.floodBox_flood.Name = "floodBox_flood";
            this.floodBox_flood.Size = new System.Drawing.Size(45, 44);
            this.floodBox_flood.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.floodBox_flood.TabIndex = 17;
            this.floodBox_flood.TabStop = false;
            this.floodBox_flood.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // loadImageBox
            // 
            this.loadImageBox.Image = global::FormsPaint.Properties.Resources.loadImage;
            this.loadImageBox.Location = new System.Drawing.Point(943, 48);
            this.loadImageBox.Name = "loadImageBox";
            this.loadImageBox.Size = new System.Drawing.Size(45, 44);
            this.loadImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.loadImageBox.TabIndex = 16;
            this.loadImageBox.TabStop = false;
            this.loadImageBox.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // eraserBox_eraser
            // 
            this.eraserBox_eraser.Image = global::FormsPaint.Properties.Resources.eraser;
            this.eraserBox_eraser.Location = new System.Drawing.Point(943, 108);
            this.eraserBox_eraser.Name = "eraserBox_eraser";
            this.eraserBox_eraser.Size = new System.Drawing.Size(45, 44);
            this.eraserBox_eraser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.eraserBox_eraser.TabIndex = 15;
            this.eraserBox_eraser.TabStop = false;
            this.eraserBox_eraser.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // lineBox_line
            // 
            this.lineBox_line.Image = global::FormsPaint.Properties.Resources.line;
            this.lineBox_line.Location = new System.Drawing.Point(921, 204);
            this.lineBox_line.Name = "lineBox_line";
            this.lineBox_line.Size = new System.Drawing.Size(45, 44);
            this.lineBox_line.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.lineBox_line.TabIndex = 14;
            this.lineBox_line.TabStop = false;
            this.lineBox_line.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // rectangleBox_rectangle
            // 
            this.rectangleBox_rectangle.Image = global::FormsPaint.Properties.Resources.rectangle;
            this.rectangleBox_rectangle.Location = new System.Drawing.Point(854, 204);
            this.rectangleBox_rectangle.Name = "rectangleBox_rectangle";
            this.rectangleBox_rectangle.Size = new System.Drawing.Size(45, 44);
            this.rectangleBox_rectangle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.rectangleBox_rectangle.TabIndex = 13;
            this.rectangleBox_rectangle.TabStop = false;
            this.rectangleBox_rectangle.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // ellipseBox_ellipse
            // 
            this.ellipseBox_ellipse.Image = global::FormsPaint.Properties.Resources.circle;
            this.ellipseBox_ellipse.Location = new System.Drawing.Point(784, 204);
            this.ellipseBox_ellipse.Name = "ellipseBox_ellipse";
            this.ellipseBox_ellipse.Size = new System.Drawing.Size(45, 44);
            this.ellipseBox_ellipse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ellipseBox_ellipse.TabIndex = 12;
            this.ellipseBox_ellipse.TabStop = false;
            this.ellipseBox_ellipse.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // penBox_pen
            // 
            this.penBox_pen.Image = global::FormsPaint.Properties.Resources.pen;
            this.penBox_pen.Location = new System.Drawing.Point(756, 108);
            this.penBox_pen.Name = "penBox_pen";
            this.penBox_pen.Size = new System.Drawing.Size(45, 44);
            this.penBox_pen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.penBox_pen.TabIndex = 10;
            this.penBox_pen.TabStop = false;
            this.penBox_pen.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // saveFileBox
            // 
            this.saveFileBox.Image = global::FormsPaint.Properties.Resources.save;
            this.saveFileBox.Location = new System.Drawing.Point(885, 48);
            this.saveFileBox.Name = "saveFileBox";
            this.saveFileBox.Size = new System.Drawing.Size(45, 44);
            this.saveFileBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.saveFileBox.TabIndex = 9;
            this.saveFileBox.TabStop = false;
            this.saveFileBox.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // clearCanvasBox
            // 
            this.clearCanvasBox.Image = global::FormsPaint.Properties.Resources.delete;
            this.clearCanvasBox.Location = new System.Drawing.Point(819, 48);
            this.clearCanvasBox.Name = "clearCanvasBox";
            this.clearCanvasBox.Size = new System.Drawing.Size(45, 44);
            this.clearCanvasBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.clearCanvasBox.TabIndex = 8;
            this.clearCanvasBox.TabStop = false;
            this.clearCanvasBox.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // undoBox
            // 
            this.undoBox.Image = global::FormsPaint.Properties.Resources.undo;
            this.undoBox.Location = new System.Drawing.Point(756, 48);
            this.undoBox.Name = "undoBox";
            this.undoBox.Size = new System.Drawing.Size(45, 44);
            this.undoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.undoBox.TabIndex = 7;
            this.undoBox.TabStop = false;
            this.undoBox.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // currentColorBox
            // 
            this.currentColorBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.currentColorBox.Location = new System.Drawing.Point(810, 406);
            this.currentColorBox.Name = "currentColorBox";
            this.currentColorBox.Size = new System.Drawing.Size(64, 60);
            this.currentColorBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.currentColorBox.TabIndex = 6;
            this.currentColorBox.TabStop = false;
            this.currentColorBox.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // paintBrushBox_brush
            // 
            this.paintBrushBox_brush.Image = global::FormsPaint.Properties.Resources.icons8_paint_100;
            this.paintBrushBox_brush.Location = new System.Drawing.Point(819, 108);
            this.paintBrushBox_brush.Name = "paintBrushBox_brush";
            this.paintBrushBox_brush.Size = new System.Drawing.Size(45, 44);
            this.paintBrushBox_brush.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.paintBrushBox_brush.TabIndex = 0;
            this.paintBrushBox_brush.TabStop = false;
            this.paintBrushBox_brush.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1103, 680);
            this.Controls.Add(this.floodBox_flood);
            this.Controls.Add(this.loadImageBox);
            this.Controls.Add(this.eraserBox_eraser);
            this.Controls.Add(this.lineBox_line);
            this.Controls.Add(this.rectangleBox_rectangle);
            this.Controls.Add(this.ellipseBox_ellipse);
            this.Controls.Add(this.penBox_pen);
            this.Controls.Add(this.saveFileBox);
            this.Controls.Add(this.clearCanvasBox);
            this.Controls.Add(this.undoBox);
            this.Controls.Add(this.currentColorBox);
            this.Controls.Add(this.thicknessTextBox);
            this.Controls.Add(this.thicknessLabel);
            this.Controls.Add(this.thicknessSlider);
            this.Controls.Add(this.paintPanel);
            this.Controls.Add(this.paintBrushBox_brush);
            this.Name = "Form1";
            this.Text = " ";
            this.Load += new System.EventHandler(this.onLoad);
            ((System.ComponentModel.ISupportInitialize)(this.thicknessSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.floodBox_flood)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eraserBox_eraser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineBox_line)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rectangleBox_rectangle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ellipseBox_ellipse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.penBox_pen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveFileBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clearCanvasBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.undoBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paintBrushBox_brush)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox paintBrushBox_brush;
        private System.Windows.Forms.Panel paintPanel;
        private System.Windows.Forms.TrackBar thicknessSlider;
        private System.Windows.Forms.Label thicknessLabel;
        private System.Windows.Forms.TextBox thicknessTextBox;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.PictureBox currentColorBox;
        private System.Windows.Forms.PictureBox undoBox;
        private System.Windows.Forms.PictureBox saveFileBox;
        private System.Windows.Forms.PictureBox penBox_pen;
        private System.Windows.Forms.PictureBox clearCanvasBox;
        private System.Windows.Forms.PictureBox ellipseBox_ellipse;
        private System.Windows.Forms.PictureBox rectangleBox_rectangle;
        private System.Windows.Forms.PictureBox lineBox_line;
        private System.Windows.Forms.PictureBox eraserBox_eraser;
        private System.Windows.Forms.PictureBox loadImageBox;
        private System.Windows.Forms.PictureBox floodBox_flood;
    }
}

