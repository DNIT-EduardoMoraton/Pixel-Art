using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Pixel_Art
{
    class Conversor
    {
        public static void CreateBipMapFromCellList((int x, int y) size, string path, List<Cell> cellList)
        {
            Bitmap bmp = new Bitmap(size.x, size.y);
            foreach (Cell c in cellList)
            {
                System.Windows.Media.Color mediacolor = c.getColor(); // your color

                var drawingcolor = System.Drawing.Color.FromArgb(
                    mediacolor.A, mediacolor.R, mediacolor.G, mediacolor.B);

                bmp.SetPixel(c.getPos().Item1, c.getPos().Item2, drawingcolor);
            }


            bmp.Save(path);
        }



    }
}
