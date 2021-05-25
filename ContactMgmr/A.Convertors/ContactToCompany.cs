using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using A.Extenssions;
using A.WPF;
using DAL.DataModel;

namespace A.Convertors
{
    public class ContactToCompany : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dd = new List<string> { ConvertorsStaticPublic.All };

            dd.AddRange(((ObservableCollection<Contact>)value).Select(a => a.Company.Trim()).OrderBy(a => a).Distinct().ToList());
            return dd.ToList();
        }

    }
}