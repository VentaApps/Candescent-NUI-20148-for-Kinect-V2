using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CCT.NUI.TestDataCollector
{
    public class MultiValueConverter : IMultiValueConverter
    {
        public MultiValueConverter()
        { }

        public int ValueCount { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values[0];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = new List<object>();
            for (int index = 0; index < this.ValueCount; index++)
            {
                result.Add(value);
            }
            return result.ToArray();
        }
    }
}
