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
            DataTable = new DataTable();
            MyList = new ObservableCollection<DataItem>();
        }

        public Model Model
        {
            get
            {
                return model;
            }
        }

        public DataTable DataTable
        {
            get;
            private set;
        }

        public ObservableCollection<DataItem> MyList { get; private set; }

        public void BindColumn(DataGrid grid)
        {
            // ExpandoObject implements IDictionary<string,object> 
            IEnumerable<IDictionary<string, object>> rows = grid.ItemsSource.OfType<IDictionary<string, object>>();
            IEnumerable<string> columns = rows.SelectMany(d => d.Keys).Distinct(StringComparer.OrdinalIgnoreCase);
            foreach (string s in columns)
                grid.Columns.Add(new DataGridTextColumn { Header = s });
        }

        public void UpdateDataGrid(DataGrid grid)
        {
            // First resize the model
            int DEBUG_VALUE = 1;
            model.resize(model.RowCount, model.ColumnCount + DEBUG_VALUE);
            
            // Then write back data from view model to model
            for (int i=0;i<this.MyList.Count;i++)
            {
                DataItem dataItem = this.MyList[i];
                for (int j=0;j<dataItem.DataList.Count;j++)
                {
                    int value = dataItem.DataList[j].MyValue;
                    this.model[i, j] = value;
                }
            }

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

            // Remove all items in the view model
            while (this.MyList.Count>0)
            {
                this.MyList.RemoveAt(0);
            }

            // Then add the items to the view model based on the model
            for (int i=0;i<model.RowCount;i++)
            {
                DataItem dataItem = new DataItem();
                for (int j=0;j<model.ColumnCount;j++)
                {
                    dataItem.DataList.Add(new DataListItem() { MyValue = this.model[i, j] });
                }
                this.MyList.Add(dataItem);
            }
        }

        private void UpdateColumns(ObservableCollection<DataGridColumn> columns)
        {
            // First remove all columns
            while (columns.Count>0)
            {
                columns.RemoveAt(0);
            }

            // Then add the number of columns based on the model
            for (int i=0;i<model.ColumnCount;i++)
            {
                DataGridTextColumn col = new DataGridTextColumn();
                col.Header = ""+i;
                Binding binding = new Binding(string.Format("DataList[{0}].MyValue", i));
                binding.Mode = BindingMode.TwoWay;
                col.Binding = binding;
                columns.Add(col);
            }

            foreach (DataGridColumn column in columns)
            {
                Console.WriteLine((column as DataGridTextColumn).Binding);
            }

            model.resize(model.RowCount, model.ColumnCount+1);
        }


    }
}
