using System;
using System.Collections.Generic;

namespace Serviecs
{
    public enum WindowMode { View, New, Edit, ASearch, EditGroup }

    public class StaticPublic
    {
        public static string JsonPath => AppDomain.CurrentDomain.BaseDirectory + @"\json.txt";

        public static string ApplicationPathPath => AppDomain.CurrentDomain.BaseDirectory;

        public static string MdbPath => ApplicationPathPath + @"\db.mdb";


        public const string All = "(All)";
        public const string PrintSeprator = "*****";

        public static List<string> Monthes => new List<string> { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };

        public static int Tax = 3;
        public static int AddedValue = 9;

       

    }

}
