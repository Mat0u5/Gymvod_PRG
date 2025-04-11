using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace FormsPaint
{
    internal class FileManager
    {
        private Painter painter;

        public FileManager(Painter painter)
        {
            this.painter = painter;
        }

        public void LoadFile()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Load Image",
                Filter = "Png Image (.png)|*.png|Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|" +
                         "Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //Loads the selected image into the canvas, making sure it fully fits (resising if necessary)
                Bitmap image = new Bitmap(dialog.FileName);
                Panel panel = painter.paintPanel;

                float scaleX = (float)image.Width / panel.Width;
                float scaleY = (float)image.Height / panel.Height;
                float scale = Math.Max(scaleX, scaleY);

                Graphics panelGraphics = panel.CreateGraphics();
                Graphics bitmapGraphics = Graphics.FromImage(painter.GetCurrentBitmap());

                panelGraphics.DrawImage(image, new Rectangle(0, 0, Convert.ToInt32(image.Width / scale),
                                                         Convert.ToInt32(image.Height / scale)));
                bitmapGraphics.DrawImage(image, new Rectangle(0, 0, Convert.ToInt32(image.Width / scale),
                                                           Convert.ToInt32(image.Height / scale)));
            }
        }

        public void SaveFile()
        {
            //Saves the current crawing as a file5
            SaveFileDialog dialog = new SaveFileDialog
            {
                FileName = "formsPaintSave",
                DefaultExt = "png",
                ValidateNames = true,
                Filter = "Png Image (.png)|*.png|Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|" +
                         "Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bitmap = painter.GetCurrentBitmap();

                if (dialog.FileName.EndsWith(".bmp")) bitmap.Save(dialog.FileName, ImageFormat.Bmp);
                if (dialog.FileName.EndsWith(".jpeg")) bitmap.Save(dialog.FileName, ImageFormat.Jpeg);
                if (dialog.FileName.EndsWith(".png")) bitmap.Save(dialog.FileName, ImageFormat.Png);
                if (dialog.FileName.EndsWith(".tiff")) bitmap.Save(dialog.FileName, ImageFormat.Tiff);
                if (dialog.FileName.EndsWith(".wmf")) bitmap.Save(dialog.FileName, ImageFormat.Wmf);
            }
        }
    }
}
