using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grid
{
    class GridData
    {
        private int rowCount;
        private int columnCount;

        public GridData()
        {
            rowCount = 1;
            columnCount = 1;
            Data = new int[rowCount, columnCount];
        }

        #region Properties
        public int[,] Data { get; private set; }
        
        public int this[int i, int j]
        {
            get { return this.Data[i, j]; }
            set { this.Data[i, j] = value; }
        }

        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }
        public int DefaultValue { get; set; }
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

            RowCount = nRows;
            ColumnCount = nCols;
        }

        #endregion
    }
}
