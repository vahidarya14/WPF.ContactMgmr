using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using DAL.DataModel;

namespace A.Extenssions
{
    public static partial class Extenssions
    {
        public static object DeepCloneNoneSerialisableObject(this object obj)
        {
            if (obj == null)
                return null;
            Type type = obj.GetType();

            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }
            else if (type.IsArray)
            {
                Type elementType = Type.GetType(
                     type.FullName.Replace("[]", string.Empty));
                var array = obj as Array;
                Array copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(DeepCloneNoneSerialisableObject(array.GetValue(i)), i);
                }
                return Convert.ChangeType(copied, obj.GetType());
            }
            else if (type.IsClass)
            {

                object toret = Activator.CreateInstance(obj.GetType());
                FieldInfo[] fields = type.GetFields(BindingFlags.Public |
                            BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    object fieldValue = field.GetValue(obj);
                    if (fieldValue == null)
                        continue;
                    field.SetValue(toret, DeepCloneNoneSerialisableObject(fieldValue));
                }
                return toret;
            }
            else
                throw new ArgumentException("Unknown type");
        }
        public static T Clone<T>(this T source) where T : class
        {
            if (source == null) return default(T);

            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        static public List<string> GetSubdirectoriesContainingOnlyFolderes(this string path)
        {
            return (from subdirectory in Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
                        //where Directory.GetDirectories(subdirectory).Length == 0
                    select subdirectory).ToList();
        }

        static public List<string> GetSubdirectoriesContainingOnlyFiles(this string path)
        {
            var all = new List<string>();
            (from subdirectory in Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
                 //where Directory.GetDirectories(subdirectory).Length == 0
             select subdirectory).ToList().ForEach(a => all.AddRange(Directory.GetFiles(a)));

            all.AddRange(Directory.GetFiles(path));
            return all;
        }

        static public string LastPartOfDate(this string d)
        {
            var sd = d.ToStandardDate().Split('/')[2];

            return sd;
        }

        public static Gender ToGender(this string str) => str == "0" ? Gender._ : str == "1" ? Gender.Mr: Gender.Mis ;

        public static string ToFarsi(this Gender value) => value == Gender._ ? "" : value == Gender.Mr ? "آقای" : "خانم";



        public static string CalculateMd5Hash(this string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }





        public static string ToStandardDate(this string str)
        {
            var funits = str.Split(new[] { '/' });
            funits[0] = funits[0].PadLeft(3, '3');
            funits[0] = funits[0].PadLeft(4, '1');
            var ss = "";
            foreach (var a in funits)
                ss += a.PadLeft(2, '0') + "/";
            return ss.Substring(0, ss.Length - 1);
        }

        public static string ToStandardTime(this string str)
        {
            var funits = str.Split(new[] { ':' });
            var ss = "";
            foreach (var a in funits)
                ss += a.PadLeft(2, '0') + ":";
            return ss.Substring(0, ss.Length - 1);
        }

        public static T ToEnum<T>(this string str)
        {
            var funits = Enum.GetValues(typeof(T));
            foreach (var a in funits)
                if (a.ToString() == str) return (T)a;
            return (T)funits.GetValue(0);
        }

        public static double ToDouble(this string str)
        {
            double d;
            double.TryParse(str, out d);
            return d;
        }
        public static int ToInt(this string str)
        {
            int d;
            int.TryParse(str, out d);
            return d;
        }
        public static long ToLong(this string str)
        {
            long d;
            long.TryParse(str, out d);
            return d;
        }

        public static decimal ToDecimal(this string str)
        {
            decimal d;
            decimal.TryParse(str, out d);
            return d;
        }

        public static string TellShow(this string text)
        {
            if (text.Length == 7)
                return text.Substring(0, 3) + "" + text.Substring(3, 2) + "" + text.Substring(5, 2);

            if (text.Length == 8)
                return text.Substring(0, 2) + "" + text.Substring(2, 2) + "" + text.Substring(4, 2) + "" + text.Substring(6, 2);

            return text;
        }

        public static string MobileShow(this string text)
        {
            if (text.Length < 11) return text;

            return text.Substring(0, 4) + "-" + text.Substring(4, 3) + "" + text.Substring(7, 2) + "" + text.Substring(9, 2);
        }

        public static string MonyShow(this string text)
        {
            double d;
            if (!double.TryParse(text, out d)) return text;

            if (d == 0.0) return "0";

            var negetive = text.StartsWith("-");
            text = text.Replace("-", "");

            text = Math.Round(double.Parse(text)).ToString(CultureInfo.InvariantCulture);
            var parts = text.Split('.');
            parts[0] = parts[0].Replace(",", "");
            var str = "";
            var j = 1;
            for (int i = parts[0].Length - 1; i >= 0; i--)
            {
                str = parts[0][i] + str;
                if (j % 3 == 0 && i != 0) str = "," + str;
                j++;
            }
            if (parts.Length > 1)
                str += '.' + parts[1];
            if (negetive) str = "-" + str;
            return str;
        }



        public static string Reversion(this string fileName)
        {

            if (string.IsNullOrWhiteSpace(fileName)) return "";

            //جدا کردن شماره اخر نام فایل از بقیه قسمتها
            var integer = "";
            for (var i = fileName.Length - 1; i > 0; i--)
            {
                if (char.IsDigit(fileName[i]))
                    integer = fileName[i].ToString(CultureInfo.InvariantCulture) + integer;
                else
                {
                    fileName = fileName.Substring(0, i + 1);
                    break;
                }
            }
            if (integer == "") integer = "0";

            var retDouble = (double.Parse(integer) + 1).ToString(CultureInfo.InvariantCulture).PadLeft(4, '0');
            //if (retDouble.ToString(CultureInfo.InvariantCulture).Length < 4) return fileName + "0" + retDouble;  //var result = input.ToString().PadLeft(length, '0');
            return fileName + retDouble;
        }

        public static string ToFileName(this string fileName)
        {
            var parts = fileName.Split('\\');
            return parts[parts.Length - 1];
        }

        public static string TodayPersianDate(this PersianCalendar persianCalendar)
        {
            return persianCalendar.GetYear(DateTime.Now).ToString(CultureInfo.InvariantCulture).PadLeft(4, '0') + "/" +
                   persianCalendar.GetMonth(DateTime.Now).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + "/" +
                   persianCalendar.GetDayOfMonth(DateTime.Now).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
        }

        public static string ToPersianDate(this DateTime date)
        {
            var persianCalendar = new PersianCalendar();
            return persianCalendar.GetYear(date).ToString(CultureInfo.InvariantCulture).PadLeft(4, '0') + "/" +
                   persianCalendar.GetMonth(date).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + "/" +
                   persianCalendar.GetDayOfMonth(date).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
        }

        public static Tuple<int,int,int> ToPersianDate(this DateTime date,int alaki=0)
        {
            var persianCalendar = new PersianCalendar();
            return 
                new Tuple<int, int, int>(persianCalendar.GetYear(date), persianCalendar.GetMonth(date), persianCalendar.GetDayOfMonth(date));
        }


        public static int DateTimeCompare(this string date1, string date2)
        {
            if (string.IsNullOrWhiteSpace(date1)) date1 = "00/00";
            date1 = date1.Trim().Replace("00", "00/00");
            var parts1 = date1.Split('/').ToList().ConvertAll(a => int.Parse(a));
            var parts2 = date2.Trim().Split('/').ToList().ConvertAll(a => int.Parse(a));

            if (parts2.Count != parts1.Count) return -1;

            if (parts2[0] == parts1[0] && parts2[1] == parts1[1] && parts2[2] == parts1[2]) return 0;


            if (parts2[0] < parts1[0]) return 1;
            if (parts2[1] < parts1[1]) return 1;
            if (parts2[2] < parts1[2]) return 1;


            return -1;
        }

        public static string ToSnoozeText(this int i)
        {
            switch (i)
            {
                case 0: return "None";
                case 1: return "1 minute";
                default: return i + " minutes";
            }
        }

        public static double ToValidDouble(this string str)
        {
            double b;
            var s = double.TryParse(str, out b);
            return s ? b : 0;
        }




    }

}
