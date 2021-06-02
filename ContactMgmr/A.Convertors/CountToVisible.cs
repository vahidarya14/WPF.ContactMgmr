using System;
using System.Globalization;
using System.Windows;
using A.WPF;

namespace A.Convertors
{
    public class CountToVisible : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (int)value > 0 ? Visibility.Visible : Visibility.Hidden;
    }

}