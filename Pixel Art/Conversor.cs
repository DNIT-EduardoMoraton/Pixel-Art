using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Media;

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

                bmp.SetPixel(c.getPos().Item2, c.getPos().Item1, drawingcolor);
            }


            bmp.Save(path);
        }
        public static void CreateBipMapFromCellListIco((int x, int y) size, string path, List<Cell> cellList)
        {
            Bitmap bmp = new Bitmap(size.x, size.y);
            foreach (Cell c in cellList)
            {
                System.Windows.Media.Color mediacolor = c.getColor(); // your color

                var drawingcolor = System.Drawing.Color.FromArgb(
                    mediacolor.A, mediacolor.R, mediacolor.G, mediacolor.B);

                bmp.SetPixel(c.getPos().Item1, c.getPos().Item2, drawingcolor);
            }
            using (Stream sw = new FileStream(path,FileMode.OpenOrCreate))
            {
                Icon.FromHandle(bmp.GetHicon()).Save(sw);
            }
            
        }
       

        public static void CreateFile(int filterIndex, (int x, int y) size, string path, List<Cell> cellList)
        {
            Console.WriteLine(filterIndex);

            switch(filterIndex)
            {
                case 1:
                    CreateBipMapFromCellList(size, path, cellList);
                    break;
                case 2:
                    CreateBipMapFromCellListIco(size, path, cellList);
                    break;
                default:
                    break;
            }
        }

        public static List<Cell> OpenPngToCellList(String path)
        {
            List<Cell> list = new List<Cell>();

            Bitmap bmp = new Bitmap(path);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Cell c = new Cell((i, j));

                    
                    
                    c.setColor((SolidColorBrush) new SolidBrush(System.Windows.Media.Color.FromArgb(bmp.GetPixel(i, j).ToArgb())))
                }
            }



            return list;
        }

    }



}
