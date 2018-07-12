using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Grid
{
    class ValueToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string input;
            try
            {
                DataGridCell dgc = (DataGridCell) value;
                
                int column = dgc.Column.DisplayIndex;
                DataItem rowView = (DataItem)dgc.DataContext;
                var a = rowView.DataList[column];
                input = a.Content;
            }
            catch (InvalidCastException e)
            {
                return Brushes.Orange;
            }
            ColorModelAccessor accessor = new ColorModelAccessor();
            Color c;
            if (accessor.ColorModel.Colors.TryGetValue(input, out c))
            {
                Brush brush = new SolidColorBrush(c);
                return brush;
            }
            else
            {
                return Brushes.Orange;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
