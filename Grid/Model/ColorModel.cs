using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Grid
{
    class ColorModel
    {
        // *** Singleton *** 
        private static readonly Lazy<ColorModel> lazy =
        new Lazy<ColorModel>(() => new ColorModel());

        public static ColorModel Instance { get { return lazy.Value; } }


        private readonly string filename = "color.txt";

        private ColorModel()
        {
            Colors = new Dictionary<string, Color>();

            // Load the color information from file
            if (File.Exists(filename))
            {
                try
                {
                    this.LoadFromFile();
                }
                catch (IOException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public Dictionary<string, Color> Colors { get; private set; }

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
                    var key = values[0];
                    Color c = new Color();
                    c.R = byte.Parse(values[1]);
                    c.G = byte.Parse(values[2]);
                    c.B = byte.Parse(values[3]);
                    c.A = byte.Parse(values[4]);

                    Colors.Add(key, c);
                }
            }
        }
    }
}
