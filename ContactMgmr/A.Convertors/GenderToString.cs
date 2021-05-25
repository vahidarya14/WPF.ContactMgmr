using System;
using System.Globalization;
using A.WPF;
using DAL.DataModel;

namespace A.Convertors
{
    public class GenderToString : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Gender)value == Gender._ ? "" : (Gender)value == Gender.Mr ? "آقای" : "خانم";
        }
    }
}