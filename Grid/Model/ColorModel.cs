using System;
using System.Collections.Generic;
using System.IO;
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
            Colors = new Dictionary<AlphaNumeric, Color>();
        }

        public Dictionary<AlphaNumeric, Color> Colors { get; private set; }

        public void DeleteAll()
        {
            var keys = Colors.Keys.ToList();
            foreach (var key in keys)
            {
                Colors.Remove(key);
            }
        }


        public void SaveToFile()
        {
            var keys = Colors.Keys.ToList();
            keys.Sort();
            StringBuilder sb = new StringBuilder();

            // Add the header
            sb.Append("Number,R,G,B,A\r\n");

            foreach (var key in keys)
            {
                sb.Append(key+",");
                Color color = new Color();
                this.Colors.TryGetValue(key, out color);
                sb.Append(color.R + "," + color.G + "," + color.B + "," + color.A +"\r\n");
            }

            System.IO.File.WriteAllText(this.filename, sb.ToString());
        }

        private void LoadFromFile()
        {
            using (var reader = new StreamReader(filename))
            {
                this.DeleteAll();

                // Ignore the header line
                if (!reader.EndOfStream)
                {
                    reader.ReadLine();
                }

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    AlphaNumeric a = new AlphaNumeric(values[0]);
                    Color c = new Color();
                    c.R = byte.Parse(values[1]);
                    c.G = byte.Parse(values[2]);
                    c.B = byte.Parse(values[3]);
                    c.A = byte.Parse(values[4]);
                }
            }
        }
    }
}
