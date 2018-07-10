using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Grid
{
    class Model
    {
        int[,] data;
        private int rowCount;
        private int columnCount;
        private int defaultValue;

        public Model(int rowCount, int columnCount)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            data = new int[rowCount, columnCount];
        }

        #region Properties

        public int[,] Data
        {
            get
            {
                return data;
            }
            private set
            {
                data = value;
            }
        }
        
        public int this[int i, int j]
        {
            get {
                return this.Data[i, j];
            }
            set {
                this.Data[i, j] = value;
            }
        }

        public int RowCount
        {
            get
            {
                return this.rowCount;
            }
            private set
            {
                this.rowCount = value;
            }
        }

        public int ColumnCount
        {
            get
            {
                return this.columnCount;
            }
            private set
            {
                this.columnCount = value;
            }
        }

        public int DefaultValue {
            get
            {
                return this.defaultValue;
            }
            private set {
                this.defaultValue = value;
            }
        }
        #endregion

        #region Methods

        public void resize(int nRows, int nCols)
        {
            int[,] newData = new int[nRows, nCols];

            for (int i=0;i<nRows;i++)
            {
                for (int j=0;j<nCols;j++)
                {
                    // Set to default value
                    newData[i, j] = DefaultValue;

                    // Copy old data to new data if possible
                    if (i<RowCount && j<ColumnCount)
                    {
                        newData[i, j] = Data[i, j];
                    } 
                }
            }

            Data = newData;
            RowCount = nRows;
            ColumnCount = nCols;
        }

        #endregion
    }
}
