using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GoogleSchoolProjectManager.UI.Converter
{
    public class StringFormatConcatenator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string format = values[0].ToString();
            for (int i = 0; i < (values.Length - 1) / 2; i++)
                format = format.Replace("{" + i.ToString() + "}", "{" + i.ToString() + ":" + values[(i * 2) + 1].ToString() + "}");

            return string.Format(format, values.Skip(1).Select((s, i) => new { j = i + 1, s }).Where(t => t.j % 2 == 0).Select(t => t.s).ToArray());
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new string[] { };
        }
    }
}
