using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid
{
    class DataItem 
    {
        public string Name { get; set; }
        public ObservableCollection<DataListItem> DataList { get; set; }

        public DataItem()
        {
            this.DataList = new ObservableCollection<DataListItem>();
        }
    }
}
