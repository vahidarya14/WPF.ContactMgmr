using System;
using System.Globalization;
using A.Extenssions;
using A.WPF;

namespace A.Convertors
{
    public class StringToTellForm : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().TellShow();
        }
    }
}