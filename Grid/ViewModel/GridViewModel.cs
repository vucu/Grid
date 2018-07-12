using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Grid
{
    class GridViewModel : NotifyPropertyChangedImpl
    {
        private readonly MainWindow view;

        private readonly GridModel gridModel;
        private readonly ColorModel colorModel;

        public GridViewModel(MainWindow view)
        {
            gridModel = GridModel.Instance;
            colorModel = ColorModel.Instance;
            MyList = new ObservableCollection<DataItem>();
            this.view = view;
        }

        public string DefaultCellValue {
            get {
                return gridModel.DefaultValue.Content;
            }
            set
            {
                gridModel.DefaultValue.Content = value;
            }
        }

        public int RowCount
        {
            get
            {
                return gridModel.RowCount;
            }
        }

        public int ColumnCount
        {
            get
            {
                return gridModel.ColumnCount;
            }
        }

        
        private string selectedValueString = null;
        private Color currentColor;
        public void SetSelectedValueString(string value)
        {
            Console.WriteLine("selected value: " + value);
            selectedValueString = value;
            Color c;
            Color white = Color.FromRgb(255, 255, 255);

            if (!colorModel.Colors.TryGetValue(selectedValueString, out c)) currentColor = white;
            else currentColor = c;

            // Notify change for CurrentColor
            OnPropertyChanged("CurrentColor");
        }

        public Color CurrentColor
        {
            get
            {
                return currentColor;
            }
            set
            {
                currentColor = value;
                if (selectedValueString != null)
                {
                    colorModel.Colors[selectedValueString] = value;
                }

                // Notify change
                OnPropertyChanged("CurrentColor");

                // Update the view color
                view.UpdateColor();

                // Save to file
                colorModel.SaveToFile();
            }
        }
        


        public ObservableCollection<DataItem> MyList { get; private set; }
        

        public void ResizeDataGrid(DataGrid grid, int rowsCount, int columnsCount)
        {
            // First resize the model
            gridModel.resize(rowsCount, columnsCount);

            // Update the view model to match model size
            this.WriteViewModelToModel();
            this.LoadModelToViewModel();

            // Update the columns to match model size
            // First remove all columns
            while (grid.Columns.Count > 0)
            {
                grid.Columns.RemoveAt(0);
            }

            // Then add the number of columns based on the model
            for (int i = 0; i < gridModel.ColumnCount; i++)
            {
                DataGridTextColumn col = new DataGridTextColumn();
                Binding binding = new Binding(string.Format("DataList[{0}].Content", i));
                binding.Mode = BindingMode.TwoWay;
                col.Binding = binding;
                grid.Columns.Add(col);
            }
        }
        
        public void DeleteAllColor()
        {
            this.colorModel.DeleteAll();
            this.colorModel.SaveToFile();

            view.UpdateColor();
        }

        public void ExportToFile(string filename)
        {
            // Commit all changes
            this.WriteViewModelToModel();

            // Then export to files
            this.gridModel.ExportToFile(filename);
        }

        public void LoadFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                // Import from file
                gridModel.ImportFromFile(filename);

                // Then load the changes
                this.LoadModelToViewModel();
            }
            else
            {
                MessageBox.Show("File " + filename + " does not exists","Error");
            }
        }

        public void Put(int row, int column, string newValue)
        {
            for (int i = 0; i < this.MyList.Count; i++)
            {
                if (i==row)
                {
                    DataItem dataItem = this.MyList[i];
                    for (int j = 0; j < dataItem.DataList.Count; j++)
                    {
                        if (j==column)
                        {
                            dataItem.DataList[j].Content = newValue;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public string Get(int row, int column)
        {
            for (int i = 0; i < this.MyList.Count; i++)
            {
                if (i == row)
                {
                    DataItem dataItem = this.MyList[i];
                    for (int j = 0; j < dataItem.DataList.Count; j++)
                    {
                        if (j == column)
                        {
                            return dataItem.DataList[j].Content;
                        }
                    }
                }
            }

            return null;
        }
        
        private void WriteViewModelToModel()
        {
            for (int i = 0; i < this.MyList.Count && i < gridModel.RowCount; i++)
            {
                DataItem dataItem = this.MyList[i];
                for (int j = 0; j < dataItem.DataList.Count && j < gridModel.ColumnCount; j++)
                {
                    var value = dataItem.DataList[j];
                    this.gridModel[i, j] = value;
                }
            }
        }

        private void LoadModelToViewModel()
        {
            // Remove all items in the current view model
            while (this.MyList.Count > 0)
            {
                this.MyList.RemoveAt(0);
            }

            // Then add the items to the view model based on the model
            for (int i = 0; i < gridModel.RowCount; i++)
            {
                DataItem dataItem = new DataItem();
                for (int j = 0; j < gridModel.ColumnCount; j++)
                {
                    dataItem.DataList.Add(this.gridModel[i, j]);
                }
                this.MyList.Add(dataItem);
            }
        }
    }
}
