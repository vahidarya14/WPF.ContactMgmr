using System;
using System.Globalization;
using System.Windows;
using A.WPF;

namespace A.Convertors
{
    public class BoolToVisible : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ss = (bool)value ? Visibility.Visible : Visibility.Hidden;
            return ss;
        }
    }
}