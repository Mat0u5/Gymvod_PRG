using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsPaint
{
    internal class CanvasUndo
    {
        private List<Bitmap> bitmapHistory = new List<Bitmap>();
        private Painter painter;

        public CanvasUndo(Painter painter)
        {
            this.painter = painter;
            SaveState();
        }

        public void SaveState()
        {
            bitmapHistory.Add(painter.CreateBitmapCopy());
        }

        public void Undo()
        {
            if (bitmapHistory.Count < 2) return;

            Bitmap lastBitmap = bitmapHistory[bitmapHistory.Count - 2];
            bitmapHistory.RemoveAt(bitmapHistory.Count - 1);
            painter.RedrawBitmap(lastBitmap);
        }
    }
}
