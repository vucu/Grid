using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Grid
{
    class GridModel
    {
        AlphaNumeric[,] data;
        private int rowCount;
        private int columnCount;
        private AlphaNumeric defaultValue;

        public GridModel(int rowCount, int columnCount)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.data = new AlphaNumeric[rowCount, columnCount];
            this.defaultValue = new AlphaNumeric();

            this.resize(rowCount, columnCount);
        }

        #region Properties

        public AlphaNumeric[,] Data
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
        
        public AlphaNumeric this[int i, int j]
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

        public AlphaNumeric DefaultValue {
            get
            {
                return this.defaultValue;
            }
            set {
                this.defaultValue = value;
            }
        }
        #endregion

        #region Methods

        public void resize(int nRows, int nCols)
        {
            AlphaNumeric[,] newData = new AlphaNumeric[nRows, nCols];

            for (int i=0;i<nRows;i++)
            {
                for (int j=0;j<nCols;j++)
                {
                    // Set to default value
                    newData[i, j] = new AlphaNumeric(DefaultValue);

                    // Copy old data to new data if possible
                    if (i<RowCount && j<ColumnCount)
                    {
                        newData[i, j] = new AlphaNumeric(Data[i, j]);
                    } 
                }
            }

            Data = newData;
            RowCount = nRows;
            ColumnCount = nCols;
        }

        public void ExportToFile(string filename)
        {
            System.IO.File.WriteAllText(filename, this.ToString());
        }

        public void ImportFromFile(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                List<List<AlphaNumeric>> grid = new List<List<AlphaNumeric>>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    List<AlphaNumeric> row = new List<AlphaNumeric>();
                    var values = line.Split(',');
                    foreach (string value in values)
                    {
                        row.Add(new AlphaNumeric(value));
                    }

                    grid.Add(row);
                }

                // Convert grid to data
                int rowCount = grid.Count;
                int longestRow = grid.Max<List<AlphaNumeric>>(t => t.Count);
                this.resize(rowCount, longestRow);

                for (int i=0;i<this.RowCount;i++)
                {
                    List<AlphaNumeric> row = grid[i];
                    int j;
                    for (j=0;j<row.Count;j++)
                    {
                        data[i, j] = row[j];
                    }
                    int k;
                    for (k=j+1;k<this.ColumnCount;k++)
                    {
                        data[i, k] = new AlphaNumeric();
                    }
                }
            }
        }

        // *** Serialization ***
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.RowCount; i++)
            {
                sb.Append(Data[i, 1]);
                for (int j = 1; j < this.ColumnCount; j++)
                {
                    sb.Append(",");
                    sb.Append(Data[i, j]);
                }
                sb.Append("\r\n");
            }

            return sb.ToString();
        }

        #endregion
    }
}
 