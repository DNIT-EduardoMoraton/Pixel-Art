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
            canvasSize = (16, 16);
            cellList = new List<Cell>();
            currentColor = Brushes.Black;


            AddOptionsForNewCanvas();
            CreateGrid(canvasSize);
            AddCells(canvasSize);
            FillColorPanel();
            currentColorIndicator.Background = currentColor;
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
            } else if (e.RightButton == MouseButtonState.Pressed)
            {
                cell.setColor(Brushes.Transparent);
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

            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                cell.setColor(currentColor);
            } else
            {
                cell.setColor(Brushes.Transparent);
            }

            changed = true;
        }


        public void RadioButtonColorPick_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            currentColor = (SolidColorBrush)r.Tag;
            currentColorIndicator.Background = currentColor;
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            cellList.ForEach(cell => cell.setColor(Brushes.Transparent));
            changed = false;
        }

        private void RellenarButton_Click(object sender, RoutedEventArgs e)
        {
            cellList.ForEach(cell => cell.setColor(currentColor));
            changed = true;
        }


        private void NewCanvasButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            (int x, int y) newCanvasSize = ((int, int))b.Tag;

            if (changed)
            {
                MessageBoxResult msbRes = MessageBox.Show($"Se han realizado cambios en el dibujo, ¿Desea crear uno nuevo de {newCanvasSize} pixeles? Si lo hace perdera los Cambios", "Aaa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
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
            Regex rgx = new Regex("^#(?:[0-9a-fA-F]{3}){2}$");
            Match matcher = rgx.Match(text);


            if (matcher.Success)
            {
                b.BorderBrush = Brushes.Green;

                SolidColorBrush brushColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(text));
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
            canvasSize = grid;
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
            List<DockPanel> colorSelector_DockPanel = new List<DockPanel>
            {
                CreateColorSelector_DockPanel(Brushes.Black, "Negro"),
                CreateColorSelector_DockPanel(Brushes.White, "Blanco"),
                CreateColorSelector_DockPanel(Brushes.Red, "Rojo"),
                CreateColorSelector_DockPanel(Brushes.Green, "Verde"),
                CreateColorSelector_DockPanel(Brushes.Blue, "Azul"),
                CreateColorSelector_DockPanel(Brushes.Yellow, "Amarillo"),
                CreateColorSelector_DockPanel(Brushes.Orange, "Naranja"),
                CreateColorSelector_DockPanel(Brushes.Pink, "Rosa")
            };


            Border borCustomColor = new Border();
            
            TextBox customColor = new TextBox();
            customColor.TextChanged += CustomColor_TextChanged;
         

            borCustomColor.Child = customColor;
            borCustomColor.BorderThickness = new Thickness(3);

            RadioButton firstRadio = (RadioButton)colorSelector_DockPanel[0].Children[0];
            firstRadio.IsChecked = true;
            foreach (DockPanel sp in colorSelector_DockPanel)
            {
                colorsStackPanel.Children.Add(sp);
            }

            colorsStackPanel.Children.Add(borCustomColor);
            
        }


        public DockPanel CreateColorSelector_DockPanel(SolidColorBrush brush, String name)
        {
            DockPanel sp = new DockPanel();

            String GROUP_NAME = "colors";
            RadioButton r = new RadioButton
            {
                Content = name,
                Tag = brush,
                GroupName = GROUP_NAME
            };
            DockPanel.SetDock(r, Dock.Left);

            Border b = new Border
            {
                Width = 10,
                Height = 10,
                Background = brush,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            DockPanel.SetDock(b, Dock.Right);

            sp.Children.Add(r);
            sp.Children.Add(b);
            
            
            return sp;
        }

        public void AddOptionsForNewCanvas()
        {
            newCanvasStackPanel.Children.Add(CreateOptionNewCanvasButton((8, 8), "Pequeño"));
            newCanvasStackPanel.Children.Add(CreateOptionNewCanvasButton((16, 16), "Mediano"));
            newCanvasStackPanel.Children.Add(CreateOptionNewCanvasButton((32, 32), "Grande"));
            newCanvasStackPanel.Children.Add(CreateOptionNewCanvasButton((64, 64), "Grande +"));
            newCanvasStackPanel.Children.Add(CreateOptionNewCanvasButton((128, 128), "Super Grande"));
        }

        public Button CreateOptionNewCanvasButton((int x, int y) newCanvasSize, string name)
        {
            Button b = new Button
            {
                Tag = newCanvasSize
            };
            b.Click += NewCanvasButton_Click;
            b.Content = newCanvasSize.ToString() + " " + name;
            b.Style = (Style)this.Resources["newCanvasButton"];

            return b;
        }

        private void SaveToPng_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "png file (*.png)|*.png|ico file (*.ico)|*.ico"
            };

            if (saveFileDialog.ShowDialog() == true)
                Conversor.CreateFile(saveFileDialog.FilterIndex, canvasSize, saveFileDialog.FileName, cellList);
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog saveFileDialog = new OpenFileDialog
            {
                Filter = "png file (*.png)|*.png"
            };

            if (saveFileDialog.ShowDialog() == true)
            {

                List<Cell> cellList2 = Conversor.OpenPngToCellList(saveFileDialog.FileName);
                int side = (int)Math.Sqrt(d: cellList2.Count);
                canvasSize = (side, side);
                CreateGrid(canvasSize);
                AddCells(canvasSize);
                for (int i = 0; i
                    < cellList2.Count; i++)
                {
                    cellList[i].setColor(new SolidColorBrush(cellList2[i].getColor()));
                }
            }
                
        }
    }
}
