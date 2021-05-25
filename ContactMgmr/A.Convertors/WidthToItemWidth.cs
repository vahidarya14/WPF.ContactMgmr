using System;
using System.Globalization;
using A.WPF;

namespace A.Convertors
{
    public class WidthToItemWidth : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value) - 22;
        }
    }
}