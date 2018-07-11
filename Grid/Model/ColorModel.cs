using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Grid.Model
{
    class ColorModel
    {
        private readonly string filename = "color.txt";

        public ColorModel()
        {
            Colors = new Dictionary<int, Color>();
        }

        public Dictionary<int, Color> Colors { get; private set; }

        private void LoadFromFile()
        {
            
        }
        public void SaveToFile()
        {
            var numberList = Colors.Keys.ToList();
            numberList.Sort();
            StringBuilder sb = new StringBuilder();

            // Add the header
            sb.Append("Number,R,G,B,A\r\n");

            foreach (int number in numberList)
            {
                sb.Append(number+",");
                Color color = new Color();
                this.Colors.TryGetValue(number, out color);
                sb.Append(color.R + "," + color.G + "," + color.B + "," + color.A +"\r\n");
            }

            System.IO.File.WriteAllText(this.filename, sb.ToString());
        }
    }
}
