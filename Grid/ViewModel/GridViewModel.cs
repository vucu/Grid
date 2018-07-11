using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Grid
{
    class GridViewModel
    {
        private GridModel gridModel;

        public GridViewModel()
        {
            gridModel = new GridModel(2,2);
            MyList = new ObservableCollection<DataItem>();
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
                // col.Header = "" + i;
                Binding binding = new Binding(string.Format("DataList[{0}].Content", i));
                binding.Mode = BindingMode.TwoWay;
                col.Binding = binding;
                grid.Columns.Add(col);
            }
        }
        
        public void ExportToFile(string filename)
        {
            this.gridModel.ExportToFile(filename);
        }

        public void LoadFromFile(string filename)
        {

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
