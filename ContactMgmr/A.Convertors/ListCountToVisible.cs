//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Windows;
//using System.Windows.Data;
//using System.Windows.Markup;
//using DAL.DataModel;

//namespace A.Convertors
//{
//    public class ListCountToVisible :  IValueConverter
//    {
//        private ListCountToVisible _converter;

//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            var ss = ((List<ExtraField>)value).Count > 0 ? Visibility.Visible : Visibility.Hidden;
//            return ss;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }

  
//    }
//}