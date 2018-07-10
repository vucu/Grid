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
            model = new Model(3,3);
            DataTable = new DataTable();
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
            UpdateColumns(grid.Columns);

            
        }

        public void UpdateColumns(ObservableCollection<DataGridColumn> columns)
        {
            // First remove all columns
            while (columns.Count>0)
            {
                columns.RemoveAt(0);
            }

            // Then add the number of columns based on the model
            for (int i=0;i<model.ColumnCount;i++)
            {
                columns.Add(new DataGridTextColumn
                {
                    Binding = new Binding("Custom[" + i + "]"),
                    Header = ""+i
                });
            }

            model.resize(model.RowCount, model.ColumnCount+1);
        }


    }
}
