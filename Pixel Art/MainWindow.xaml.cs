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
using System.Text.RegularExpressions;
using Microsoft.Win32;

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
        bool changed = false;
        public MainWindow()
        {
            InitializeComponent();
            canvasSize = (20, 20);
            cellList = new List<Cell>();
            currentColor = Brushes.Black;


            AddOptionsForNewCanvas();
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
            {
                cell.setColor(currentColor);
                changed = true;
            }
               

           
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

            cell.setColor(currentColor);

            changed = true;
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


        private void newCanvasButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            (int x, int y) newCanvasSize = ((int, int))b.Tag;

            if (changed)
            {
                MessageBoxResult msbRes =  MessageBox.Show($"Se han realizado cambios en el dibujo, ¿Desea crear uno nuevo de {newCanvasSize.ToString()} pixeles? Si lo hace perdera los Cambios", "Aaa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (msbRes == MessageBoxResult.Yes)
                {
                    CreateGrid(newCanvasSize);
                    AddCells(newCanvasSize);
                    changed = false;
                }
            } else
            {
                CreateGrid(newCanvasSize);
                AddCells(newCanvasSize);
                changed = false;
            }


        }

        private void CustomColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            customColorRadioButton.IsChecked = true;
            TextBox tb = (TextBox)sender;
            Border b = (Border)tb.Parent;
            String text = tb.Text;
            Regex rgx = new Regex("^(?:[0-9a-fA-F]{3}){2}$");
            Match matcher = rgx.Match(text);


            if (matcher.Success)
            {
                b.BorderBrush = Brushes.Green;

                SolidColorBrush brushColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + text));
                currentColorIndicator.Background = brushColor;
                currentColor = brushColor;
            }
            else
            {
                b.BorderBrush = Brushes.Red;
            }
        }

        public void CreateGrid((int x, int y) grid)
        {
            canvasGrid.RowDefinitions.Clear();
            canvasGrid.ColumnDefinitions.Clear();
            canvasGrid.Children.Clear();

 

            for (int i = 0; i < grid.x; i++)
            {
                canvasGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < grid.y; i++)
            {
                canvasGrid.RowDefinitions.Add(new RowDefinition());
            }


        }

        public void AddCells((int x, int y) canvas)
        {
            cellList = new List<Cell>();
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
            List<StackPanel> colorSelector_StackPanel = new List<StackPanel>();

            colorSelector_StackPanel.Add(createColorSelector_StackPanel(Brushes.Black, "Negro"));
            colorSelector_StackPanel.Add(createColorSelector_StackPanel(Brushes.Red, "Rojo"));
            colorSelector_StackPanel.Add(createColorSelector_StackPanel(Brushes.Yellow, "Amarillo"));
            colorSelector_StackPanel.Add(createColorSelector_StackPanel(Brushes.Aqua, "Aqua"));
            colorSelector_StackPanel.Add(createColorSelector_StackPanel(Brushes.Brown, "Marron"));
            colorSelector_StackPanel.Add(createColorSelector_StackPanel(Brushes.Pink, "Rosa"));
            colorSelector_StackPanel.Add(createColorSelector_StackPanel(Brushes.Salmon, "Salmon"));



            Border borCustomColor = new Border();
            
            TextBox customColor = new TextBox();
            customColor.TextChanged += CustomColor_TextChanged;


            borCustomColor.Child = customColor;
            borCustomColor.BorderThickness = new Thickness(3);

            RadioButton firstRadio = (RadioButton)colorSelector_StackPanel[0].Children[0];
            firstRadio.IsChecked = true;
            foreach (StackPanel sp in colorSelector_StackPanel)
            {
                colorsStackPanel.Children.Add(sp);
            }

            colorsStackPanel.Children.Add(borCustomColor);
            
        }


        public StackPanel createColorSelector_StackPanel(SolidColorBrush brush, String name)
        {
            StackPanel sp = new StackPanel();

            String GROUP_NAME = "colors";
            RadioButton r = new RadioButton();
            r.Content = name;
            r.Tag = brush;
            r.GroupName = GROUP_NAME;

            Border b = new Border();
            b.Width = 10;
            b.Height = 10;
            b.Background = brush;

            sp.Children.Add(r);
            sp.Children.Add(b);
            
            return sp;
        }

        public void AddOptionsForNewCanvas()
        {




            newCanvasStackPanel.Children.Add(CreateOptionNewCanvasButton((10, 10)));
            newCanvasStackPanel.Children.Add(CreateOptionNewCanvasButton((20, 20)));
            newCanvasStackPanel.Children.Add(CreateOptionNewCanvasButton((40, 40)));
            newCanvasStackPanel.Children.Add(CreateOptionNewCanvasButton((100, 100)));
        }

        public Button CreateOptionNewCanvasButton((int x, int y) newCanvasSize)
        {
            Button b = new Button();
            b.Tag = newCanvasSize;
            // Añadir estilo
            b.Click += newCanvasButton_Click;
            b.Content = newCanvasSize.ToString();
            b.Style = (Style)this.Resources["newCanvasButton"];

            return b;
        }

        private void saveToPng_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "png file (*.png)|*.png|";
            if (saveFileDialog.ShowDialog() == true)
                Conversor.CreateBipMapFromCellList(canvasSize, saveFileDialog.FileName, cellList);
        }
    }
}
