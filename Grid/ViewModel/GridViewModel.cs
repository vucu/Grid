using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Grid
{
    class GridViewModel
    {
        private Model model;

        public GridViewModel()
        {
            model = new Model(10,10);
        }

        public ObservableCollection<DataGridColumn> ColumnCollection
        {
            get;
            private set;
        }
    }
}
