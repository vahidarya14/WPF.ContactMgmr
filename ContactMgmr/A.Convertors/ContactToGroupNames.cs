using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using A.WPF;
using DAL.DataModel;

namespace A.Convertors
{
    public class ContactToGroupNames : ValueConverter
    {
        public ObservableCollection<Contact> Contacts { get; set; }
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var showAllItem = parameter is null || bool.Parse(parameter.ToString());
            var ss = showAllItem?new List<string> { ConvertorsStaticPublic.All }:new List<string>();
            ss.AddRange(((ObservableCollection<Contact>)value).Select(a => a.GroupName.Trim()).OrderBy(a => a).Distinct().ToList());
            return ss;
        }
    }
}