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
        private Model model;

        public GridViewModel()
        {
            model = new Model(2,2);
            MyList = new ObservableCollection<DataItem>();
        }

        public Model Model
        {
            get
            {
                return model;
            }
        }

        public ObservableCollection<DataItem> MyList { get; private set; }
        

        public void ResizeDataGrid(DataGrid grid, int rowsCount, int columnsCount)
        {
            // First resize the model
            model.resize(rowsCount, columnsCount);

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
            for (int i = 0; i < model.ColumnCount; i++)
            {
                DataGridTextColumn col = new DataGridTextColumn();
                // col.Header = "" + i;
                Binding binding = new Binding(string.Format("DataList[{0}].MyValue", i));
                binding.Mode = BindingMode.TwoWay;
                col.Binding = binding;
                grid.Columns.Add(col);
            }
        }

        public void Clear()
        {
            // Clear the model with default values
            for (int i=0;i<model.RowCount;i++)
            {
                for (int j=0;j<model.ColumnCount;j++)
                {
                    model[i, j] = model.DefaultValue;
                }
            }

            // Update the view model
            LoadModelToViewModel();
        }

        public void ExportToFile(string filename)
        {
            WriteViewModelToModel();

            StringBuilder sb = new StringBuilder();
            for (int i=0;i<this.Model.RowCount;i++)
            {
                sb.Append(model[i, 1]);
                for (int j=1;j<this.Model.ColumnCount;j++)
                {
                    sb.Append(",");
                    sb.Append(model[i, j]);
                }
                sb.Append("\r\n");
            }

            System.IO.File.WriteAllText(filename, sb.ToString());
        }
        
        private void WriteViewModelToModel()
        {
            for (int i = 0; i < this.MyList.Count && i < model.RowCount; i++)
            {
                DataItem dataItem = this.MyList[i];
                for (int j = 0; j < dataItem.DataList.Count && j < model.ColumnCount; j++)
                {
                    int value = dataItem.DataList[j].MyValue;
                    this.model[i, j] = value;
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
            for (int i = 0; i < model.RowCount; i++)
            {
                DataItem dataItem = new DataItem();
                for (int j = 0; j < model.ColumnCount; j++)
                {
                    dataItem.DataList.Add(new DataListItem() { MyValue = this.model[i, j] });
                }
                this.MyList.Add(dataItem);
            }
        }
    }
}
