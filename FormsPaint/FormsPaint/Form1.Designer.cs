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
            this.paintBrushBox = new System.Windows.Forms.PictureBox();
            this.paintPanel = new System.Windows.Forms.Panel();
            this.testLabel = new System.Windows.Forms.Label();
            this.thicknessSlider = new System.Windows.Forms.TrackBar();
            this.thicknessLabel = new System.Windows.Forms.Label();
            this.thicknessTextBox = new System.Windows.Forms.TextBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.currentColorBox = new System.Windows.Forms.PictureBox();
            this.undoBox = new System.Windows.Forms.PictureBox();
            this.clearCanvasBox = new System.Windows.Forms.PictureBox();
            this.saveFileBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.paintBrushBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thicknessSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.undoBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clearCanvasBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveFileBox)).BeginInit();
            this.SuspendLayout();
            // 
            // paintBrushBox
            // 
            this.paintBrushBox.Image = global::FormsPaint.Properties.Resources.icons8_paint_100;
            this.paintBrushBox.Location = new System.Drawing.Point(921, 101);
            this.paintBrushBox.Name = "paintBrushBox";
            this.paintBrushBox.Size = new System.Drawing.Size(45, 44);
            this.paintBrushBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.paintBrushBox.TabIndex = 0;
            this.paintBrushBox.TabStop = false;
            this.paintBrushBox.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // paintPanel
            // 
            this.paintPanel.BackColor = System.Drawing.Color.White;
            this.paintPanel.Location = new System.Drawing.Point(69, 87);
            this.paintPanel.Name = "paintPanel";
            this.paintPanel.Size = new System.Drawing.Size(633, 432);
            this.paintPanel.TabIndex = 1;
            this.paintPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelMouseDrawEvent);
            this.paintPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelMouseDrawEvent);
            this.paintPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelMouseDrawEvent);
            // 
            // testLabel
            // 
            this.testLabel.AutoSize = true;
            this.testLabel.Location = new System.Drawing.Point(144, 48);
            this.testLabel.Name = "testLabel";
            this.testLabel.Size = new System.Drawing.Size(35, 13);
            this.testLabel.TabIndex = 2;
            this.testLabel.Text = "label1";
            // 
            // thicknessSlider
            // 
            this.thicknessSlider.Cursor = System.Windows.Forms.Cursors.No;
            this.thicknessSlider.Location = new System.Drawing.Point(718, 326);
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
            this.thicknessLabel.Location = new System.Drawing.Point(715, 302);
            this.thicknessLabel.Name = "thicknessLabel";
            this.thicknessLabel.Size = new System.Drawing.Size(56, 13);
            this.thicknessLabel.TabIndex = 4;
            this.thicknessLabel.Text = "Thickness";
            // 
            // thicknessTextBox
            // 
            this.thicknessTextBox.Location = new System.Drawing.Point(810, 300);
            this.thicknessTextBox.Name = "thicknessTextBox";
            this.thicknessTextBox.Size = new System.Drawing.Size(68, 20);
            this.thicknessTextBox.TabIndex = 5;
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
            // undoBox
            // 
            this.undoBox.Image = global::FormsPaint.Properties.Resources.icons8_paint_100;
            this.undoBox.Location = new System.Drawing.Point(784, 114);
            this.undoBox.Name = "undoBox";
            this.undoBox.Size = new System.Drawing.Size(45, 44);
            this.undoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.undoBox.TabIndex = 7;
            this.undoBox.TabStop = false;
            this.undoBox.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // clearCanvasBox
            // 
            this.clearCanvasBox.Image = global::FormsPaint.Properties.Resources.icons8_paint_100;
            this.clearCanvasBox.Location = new System.Drawing.Point(844, 192);
            this.clearCanvasBox.Name = "clearCanvasBox";
            this.clearCanvasBox.Size = new System.Drawing.Size(45, 44);
            this.clearCanvasBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.clearCanvasBox.TabIndex = 8;
            this.clearCanvasBox.TabStop = false;
            this.clearCanvasBox.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // saveFileBox
            // 
            this.saveFileBox.Image = global::FormsPaint.Properties.Resources.icons8_paint_100;
            this.saveFileBox.Location = new System.Drawing.Point(921, 231);
            this.saveFileBox.Name = "saveFileBox";
            this.saveFileBox.Size = new System.Drawing.Size(45, 44);
            this.saveFileBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.saveFileBox.TabIndex = 9;
            this.saveFileBox.TabStop = false;
            this.saveFileBox.Click += new System.EventHandler(this.pictureBoxClickEvent);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1000, 581);
            this.Controls.Add(this.saveFileBox);
            this.Controls.Add(this.clearCanvasBox);
            this.Controls.Add(this.undoBox);
            this.Controls.Add(this.currentColorBox);
            this.Controls.Add(this.thicknessTextBox);
            this.Controls.Add(this.thicknessLabel);
            this.Controls.Add(this.thicknessSlider);
            this.Controls.Add(this.testLabel);
            this.Controls.Add(this.paintPanel);
            this.Controls.Add(this.paintBrushBox);
            this.Name = "Form1";
            this.Text = " ";
            this.Load += new System.EventHandler(this.onLoad);
            ((System.ComponentModel.ISupportInitialize)(this.paintBrushBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thicknessSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currentColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.undoBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clearCanvasBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveFileBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox paintBrushBox;
        private System.Windows.Forms.Panel paintPanel;
        private System.Windows.Forms.Label testLabel;
        private System.Windows.Forms.TrackBar thicknessSlider;
        private System.Windows.Forms.Label thicknessLabel;
        private System.Windows.Forms.TextBox thicknessTextBox;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.PictureBox currentColorBox;
        private System.Windows.Forms.PictureBox undoBox;
        private System.Windows.Forms.PictureBox clearCanvasBox;
        private System.Windows.Forms.PictureBox saveFileBox;
    }
}

