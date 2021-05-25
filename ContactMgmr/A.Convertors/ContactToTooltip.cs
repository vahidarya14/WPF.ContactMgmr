using System;
using System.Globalization;
using A.Extenssions;
using A.WPF;
using DAL.DataModel;

namespace A.Convertors
{
    public class ContactToTooltip : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ss = ((Contact)value);
            var tooltip = "";
            tooltip += ss.Gender.ToFarsi() + " " + ss.Name + "\r\n";
            tooltip += "Title :" + ss.Title + "\r\n";
            tooltip += "Mobile :" + ss.Mobile + "\r\n";
            tooltip += "Company :" + ss.Company + "\r\n";
            tooltip += " WebSite :" + ss.WebSite + "\r\n";
            tooltip += "EMail :" + ss.EMail + "\r\n";
            tooltip += " Address :" + ss.Address + "\r\n";
            tooltip += "GroupName :" + ss.GroupName + "\r\n";
            tooltip += "Fax :" + ss.Fax + "\r\n";
            tooltip += "Extenssion :" + ss.Extenssion + "\r\n";
            tooltip += " Tel :" + ss.Tel + "\r\n";
            tooltip += " Remark :" + ss.Remark + "\r\n";
            return tooltip;
        }
    }
}