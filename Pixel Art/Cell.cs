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


        private String customColor { get => this.insideBorder.Background.ToString(); }

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

        public void setColor(SolidColorBrush c)
        {
            this.insideBorder.Background = c;
        }

        public void Hover()
        {
            this.insideBorder.BorderBrush = Brushes.Black;
            this.insideBorder.BorderThickness = new Thickness(1.5);
        }

        public void Unhover()
        {

            this.insideBorder.BorderBrush = Brushes.Gray;
            this.insideBorder.BorderThickness = new Thickness(0.5);
        }


        public static bool operator ==(Border b, Cell c)
        {
            return c.GetInsideBorder() == b;
        }

        public static bool operator !=(Border b, Cell c)
        {
            return c.GetInsideBorder() != b;
        }


        public override bool Equals(object obj)
        {
            return obj is Cell cell &&
                   EqualityComparer<Border>.Default.Equals(insideBorder, cell.insideBorder) &&
                   pos.Equals(cell.pos) &&
                   customColor == cell.customColor;
        }

        public override int GetHashCode()
        {
            int hashCode = -905174207;
            hashCode = hashCode * -1521134295 + EqualityComparer<Border>.Default.GetHashCode(insideBorder);
            hashCode = hashCode * -1521134295 + pos.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(customColor);
            return hashCode;
        }
    }
}
