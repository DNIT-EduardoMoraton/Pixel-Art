using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pixel_Art
{
    class Cell
    {
        private Border insideBorder;
        private (int x, int y) pos;


        private String customColor;

        public Cell((int x, int y) pos)
        {
            this.pos = pos;
            insideBorder = new Border();
            Grid.SetColumn(this.insideBorder, this.pos.y);
            Grid.SetRow(this.insideBorder, this.pos.x);
        }

        public Border GetInsideBorder() => this.insideBorder;
        public (int, int) getPos() => this.pos;


        public void AddToGrid(Grid g)
        {
            g.Children.Add(this.insideBorder);
        }

        public void ApplyStyle(Style s)
        {
            this.insideBorder.Style = s;
        }

        public static bool operator ==(Border b, Cell c)
        {
            return c.GetInsideBorder() == b;
        }

        public static bool operator !=(Border b, Cell c)
        {
            return c.GetInsideBorder() != b;
        }

        public void setColor()
        {
            this.insideBorder.Background = Brushes.Red;
        }

    }
}
