using System;
using System.Globalization;
using System.Windows;
using A.WPF;

namespace A.Convertors
{
    public class StringToVisible : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => string.IsNullOrEmpty(value.ToString()) ? Visibility.Collapsed : Visibility.Visible;
    }

}