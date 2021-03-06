﻿using Microsoft.Win32;
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
            viewModel = new GridViewModel(this);
            this.DataContext = viewModel;

            this.OnResizeButtonClick(null, null);
        }

        private void UpdateColor(object sender, RoutedEventArgs e)
        {
            this.UpdateColor();
        }

        private void OnResizeButtonClick(object sender, RoutedEventArgs e)
        {
            int oldRows = viewModel.RowCount;
            int oldColumns = viewModel.ColumnCount;
            int r = int.TryParse(rowCountTextBox.Text, out r) ? r : oldRows;
            int c = int.TryParse(columnCountTextBox.Text, out c) ? c : oldColumns;
            viewModel.ResizeDataGrid(numberGrid,r,c);

            UpdateColor();
        }
     
        private void OnSaveAsButtonClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            // Reset the default directory
            if (!string.IsNullOrEmpty(viewModel.FileName))
            {
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(viewModel.FileName);
            }

            dlg.FileName = "output"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt|All files|*.*"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                viewModel.FileName = dlg.FileName;
                viewModel.ExportToFile(dlg.FileName);
                viewModel.FileAvailable = true;
            }
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.ExportToFile(viewModel.FileName);
        }

        private void OnImportButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            // Reset the default directory
            if (!string.IsNullOrEmpty(viewModel.FileName))
            {
                dlg.InitialDirectory = System.IO.Path.GetDirectoryName(viewModel.FileName);
            }

            dlg.FileName = "output"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt|All files|*.*"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Load document
                viewModel.FileName = dlg.FileName;
                viewModel.LoadFromFile(dlg.FileName);
                viewModel.FileAvailable = true;
 
                UpdateColor();
            }
        }
        
        // Change all selected cells
        public void CellEditingEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                // Modify all selected values
                var tb = e.EditingElement as TextBox;
                string value = tb.Text;

                IList<DataGridCellInfo> selectedCells = numberGrid.SelectedCells;

                foreach (DataGridCellInfo cellInfo in selectedCells)
                {
                    int row = numberGrid.Items.IndexOf(cellInfo.Item);
                    int column = cellInfo.Column.DisplayIndex;
                    viewModel.Put(row, column, value);
                }

                // Then update the color
                UpdateColor();
            }
        }

        private void CellColorPickerChanged(object sender, RoutedEventArgs e)
        {
            viewModel.CurrentColor = cellColorPicker.SelectedColor ?? new Color();
            UpdateColor();
        }

        public void UpdateColor()
        {
            // Iterate through all cells
            for (int i = 0; i < numberGrid.Items.Count; i++)
            {
                for (int j = 0; j < numberGrid.Columns.Count; j++)
                {
                    // Get the visual cell
                    DataGridCell cell = ViewLibrary.GetCell(numberGrid, i, j);
                    if (cell==null)
                    {
                        Console.WriteLine("Beware! Cell is null");
                        continue;
                    }

                    // Get the cell content
                    string value = viewModel.Get(i, j);

                    // Update the color
                    ColorModelAccessor accessor = new ColorModelAccessor();
                    Color c;
                    bool success = accessor.ColorModel.Colors.TryGetValue(value, out c);
                    if (success)
                    {
                        cell.Background = new SolidColorBrush(c);
                    }
                    else
                    {
                        cell.Background = new SolidColorBrush(ColorModel.MakeStandardColor());
                    }
                }
            }
        }

        private void OnSelectionChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            IList<DataGridCellInfo> selectedCells = numberGrid.SelectedCells;

            
            // The selected value will be the most common value among selected cells
            if (selectedCells.Count>0)
            {
                // Count each type of value
                Dictionary<string, int> counts = new Dictionary<string, int>();
                foreach (DataGridCellInfo cellInfo in selectedCells)
                {
                    int row = numberGrid.Items.IndexOf(cellInfo.Item);
                    int column = cellInfo.Column.DisplayIndex;

                    string s = viewModel.Get(row, column);
                    if (counts.ContainsKey(s))
                    {
                        counts[s] += 1;
                    }
                    else
                    {
                        counts[s] = 1;
                    }
                }

                // Find the most common value
                int highestCount = 0;
                string mostCommonValue = null;
                foreach (DataGridCellInfo cellInfo in selectedCells)
                {
                    int row = numberGrid.Items.IndexOf(cellInfo.Item);
                    int column = cellInfo.Column.DisplayIndex;

                    string s = viewModel.Get(row, column);
                    int count = counts[s];
                    if (count>highestCount)
                    {
                        highestCount = count;
                        mostCommonValue = s;
                    }
                }

                // Update the most common value in the view model
                viewModel.SetSelectedValueString(mostCommonValue);
            }
            
        }

        private void OnDeleteAllColorsButtonClick(object sender, RoutedEventArgs e)
        {
            viewModel.DeleteAllColor();
        }
    }
}
