using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

namespace Grid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GridViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new GridViewModel();
            this.DataContext = viewModel;

            this.OnResizeButtonClick(null, null);
        }

        private void OnResizeButtonClick(object sender, RoutedEventArgs e)
        {
            int oldRows = viewModel.RowCount;
            int oldColumns = viewModel.ColumnCount;
            int r = int.TryParse(rowCountTextBox.Text, out r) ? r : oldRows;
            int c = int.TryParse(columnCountTextBox.Text, out c) ? c : oldColumns;
            viewModel.ResizeDataGrid(numberGrid,r,c);    
        }

        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.Clear();
        }
             
        private void OnExportButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.ExportToFile(fileNameTextBox.Text);
        }
        
        // Change all selected cells
        public void CellEditingEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var tb = e.EditingElement as TextBox;
                int value;
                if (!int.TryParse(tb.Text, out value)) return;

                IList<DataGridCellInfo> selectedCells = numberGrid.SelectedCells;

                foreach (DataGridCellInfo cellInfo in selectedCells)
                {
                    int row = numberGrid.Items.IndexOf(cellInfo.Item);
                    int column = cellInfo.Column.DisplayIndex;
                    viewModel.Put(row, column, value);
;                }
            }
        }
    }
}
