using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pixel_Art
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private (int x, int y) canvasSize;
        private List<Cell> cellList;
        private SolidColorBrush currentColor;
        public MainWindow()
        {
            InitializeComponent();
            canvasSize = (20, 20);
            cellList = new List<Cell>();
            currentColor = Brushes.Black;

            CreateGrid(canvasSize);
            AddCells(canvasSize);
            FillColorPanel();
        }

        public void CellMouseEnter(object sender, MouseEventArgs e)
        {
            Border b = (Border) sender;
            Cell cell = cellList.Find(c => b == c);
            cell.Hover();
            if (e.LeftButton == MouseButtonState.Pressed)
                cell.setColor(currentColor);
        }

        public void CellMouseLeave(object sender, MouseEventArgs e)
        {
            Border b = (Border)sender;
            Cell cell = cellList.Find(c => b == c);
            cell.Unhover();
            
        }
        public void CellMouseDown(object sender, MouseEventArgs e)
        {
            Border b = (Border)sender;
            Cell cell = cellList.Find(c => b == c);
            if (e.LeftButton == MouseButtonState.Pressed)
                cell.setColor(currentColor);
        }


        public void RadioButtonColorPick_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            currentColor = (SolidColorBrush)r.Tag;
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            cellList.ForEach(cell => cell.setColor(Brushes.Transparent));
        }

        private void RellenarButton_Click(object sender, RoutedEventArgs e)
        {
            cellList.ForEach(cell => cell.setColor(currentColor));
        }

        private void CustomColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            String text = tb.Text;


            if (text.Length == 6)
            {
                String Rhex = text.Substring(0, 2).ToUpper();
                String Ghex = text.Substring(2, 2).ToUpper();
                String Bhex = text.Substring(4, 2).ToUpper();
                rgbtext.Text = Rhex + Ghex + Bhex;
                int R = int.Parse(Rhex, System.Globalization.NumberStyles.HexNumber);
                int G = int.Parse(Rhex, System.Globalization.NumberStyles.HexNumber);
                int B = int.Parse(Rhex, System.Globalization.NumberStyles.HexNumber);
                Color c = new Color();
                c.R = Byte.Parse(Rhex);
                c.G = Byte.Parse(Ghex);
                c.B = Byte.Parse(Bhex);
                SolidColorBrush brushColor = new SolidColorBrush(c);
                rgbtext.Text = brushColor.ToString();
                rgbtext.Background = brushColor;
                currentColorIndicator.Background = brushColor;
                currentColor = brushColor;
            }




        }

        public void CreateGrid((int x, int y) grid)
        {
            List<ColumnDefinition> colsList = new List<ColumnDefinition>();
            List<RowDefinition> rowsList = new List<RowDefinition>();

            for (int i = 0; i < grid.x; i++)
            {
                colsList.Add(new ColumnDefinition());
            }
            for (int i = 0; i < grid.y; i++)
            {
                rowsList.Add(new RowDefinition());
            }

            colsList.ForEach(c => canvasGrid.ColumnDefinitions.Add(c));
            rowsList.ForEach(r => canvasGrid.RowDefinitions.Add(r));
        }

        public void AddCells((int x, int y) canvas)
        {
            for (int i = 0; i < canvas.x; i++)
            {
                for (int j = 0; j < canvas.y; j++)
                {
                    cellList.Add(new Cell((i, j)));
                }
            }


            foreach (Cell cell in cellList)
            {
                cell.AddToGrid(canvasGrid);
                cell.ApplyStyle((Style)this.Resources["cellStyle"]);
            }
            
        }

        public void FillColorPanel()
        {
            List<RadioButton> radioButtonsColorList = new List<RadioButton>();

            radioButtonsColorList.Add(createColorRadioButton(Brushes.Black, "Negro"));
            radioButtonsColorList.Add(createColorRadioButton(Brushes.Red, "Rojo"));
            radioButtonsColorList.Add(createColorRadioButton(Brushes.Yellow, "Amarillo"));
            radioButtonsColorList.Add(createColorRadioButton(Brushes.Aqua, "Aqua"));
            radioButtonsColorList.Add(createColorRadioButton(Brushes.Brown, "Marron"));
            radioButtonsColorList.Add(createColorRadioButton(Brushes.Pink, "Rosa"));
            radioButtonsColorList.Add(createColorRadioButton(Brushes.Salmon, "Salmon"));

            TextBox customColor = new TextBox();
            customColor.TextChanged += CustomColor_TextChanged;

            radioButtonsColorList[0].IsChecked = true;

            foreach (RadioButton radio in radioButtonsColorList)
            {
                colorsStackPanel.Children.Add(radio);
            }

            colorsStackPanel.Children.Add(customColor);
            
        }


        public RadioButton createColorRadioButton(SolidColorBrush b, String name)
        {
            String GROUP_NAME = "colors";
            RadioButton r = new RadioButton();
            r.Content = name;
            r.Tag = b;
            r.GroupName = GROUP_NAME;

            return r;
        }

    }
}
