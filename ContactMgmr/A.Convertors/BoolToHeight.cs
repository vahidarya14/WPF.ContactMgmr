using System;
using System.Globalization;
using A.WPF;

namespace A.Convertors
{
    public class BoolToHeight : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 70 : 0;
        }
    }
}