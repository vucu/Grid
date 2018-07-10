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
            UpdateColumns(grid.Columns);

            var m1 = new DataItem() { Name = "test1" };
            m1.DataList.Add(new DataListItem() { MyValue = 10 });
            m1.DataList.Add(new DataListItem() { MyValue = 20 });

            var m2 = new DataItem() { Name = "test2" };
            m2.DataList.Add(new DataListItem() { MyValue = 100 });
            m2.DataList.Add(new DataListItem() { MyValue = 200 });

            this.MyList.Add(m1);
            this.MyList.Add(m2);
            Console.WriteLine(this.MyList.Count);
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
