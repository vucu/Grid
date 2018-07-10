using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid
{
    class DataListItem : NotifyPropertyChangedImpl
    {
        int myValue;
        public int MyValue {
            get
            {
                return myValue;
            }
            set
            {
                myValue = value;
                OnPropertyChanged("MyValue");
            }
        }
    }
}
