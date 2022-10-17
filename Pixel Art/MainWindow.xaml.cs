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
        public MainWindow()
        {
            InitializeComponent();
            canvasSize = (20, 20);
            cellList = new List<Cell>();


            CreateGrid(canvasSize);
            AddCells(canvasSize);
        }

        public void CellMouseEnter(object sender, MouseEventArgs e)
        {
            Border b = (Border) sender;
            Cell cell = cellList.Find(c => b == c);
            if (e.LeftButton == MouseButtonState.Pressed)
                cell.setColor();
        }

        public void CellLeftMouseButtonDown(object sender, MouseEventArgs e)
        {
            Border b = (Border)sender;
            Cell cell = cellList.Find(c => b == c);
            cell.setColor();
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

    }
}
