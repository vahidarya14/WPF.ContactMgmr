using Microsoft.Office.Interop.Access;
using System;
using System.Data;
using System.Data.OleDb;

namespace LightAccessORM
{
    public enum AccessVersion { Auto, Ver2003, Ver2007, Ver2010, Ver2013, Ver2016, Ver2019 }
    public class Access
    {
        private readonly string _connectionstring;
        private OleDbConnection _myconn;

        //public ConnectionState ConnectionState=> _myconn.State; 

        protected OleDbCommand Cmd;


        /// <summary> ساخت و اتصال به فایل با توجه با کانکشن استرینگ ورودی </summary>
        /// <param name="connectionString">کانکشن استرین برای اتصال به اکسس</param>
        public Access(string connectionString)
        {
            _connectionstring = connectionString;
            OpenConnection();
        }


        /// <summary> ساخت و اتصال به فایل با توجه به ادرس و ورژن اکسز  </summary>
        /// <param name="accessFileName">ادرس فایل اکسز</param>
        /// <param name="accessVersion">ورژن اکسز سیستم</param>
        public Access(string accessFileName, AccessVersion accessVersion, string passWord = "majidsabzalian")
        {
            var connection20102007 = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={accessFileName};Jet OLEDB:Database Password={passWord};";
            var connection20132003 = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={accessFileName};Jet OLEDB:Database Password={passWord};";

            if (accessVersion == AccessVersion.Auto)
                accessVersion = GetOfficeVersion();

            _connectionstring = accessVersion == AccessVersion.Ver2007 || accessVersion == AccessVersion.Ver2010 ? connection20102007 : connection20132003;

            OpenConnection();
        }




        /// <summary> فقط زمانی نیاز هست که, کانکشنی را کلوز کرده باشید. در زمان ساخت شئی از این کلاس عمل کانکت هم بصورت اتومات اجرا میشود </summary>
        public void OpenConnection()
        {
            if (_myconn != null && _myconn.State == ConnectionState.Open) _myconn.Close();// return;

            _myconn = new OleDbConnection(_connectionstring);
            try
            {
                _myconn.Open();
                Cmd = new OleDbCommand { Connection = _myconn };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        /// <summary> اجرای دستورات بر روی فایل اکسز </summary>
        /// <param name="query">دستور قابل اجرا توسط اکسز</param>
        /// <returns></returns>
        public int CommandText(string query)
        {
            Cmd.CommandText = query;
            return Cmd.ExecuteNonQuery();
        }

        public long Max(string query)
        {
            Cmd.CommandText = query;
            var reader = Cmd.ExecuteScalar();

            return long.Parse(reader.ToString());
        }


        public OleDbDataReader ExecuteReader(string query)
        {
            Cmd.CommandText = query;
            var reader = Cmd.ExecuteReader();

            return reader;
        }

        public void CloseConnection()
        {
            if (_myconn == null || _myconn.State == ConnectionState.Closed) return;

            try
            {
                _myconn.Close();
                Cmd = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        static AccessVersion GetOfficeVersion()
        {
            try
            {
                var appVersion = new Application { Visible = false };

                switch (appVersion.Version)
                {
                    case "11.0":
                        return AccessVersion.Ver2003;
                    case "12.0":
                        return AccessVersion.Ver2007;
                    case "14.0":
                        return AccessVersion.Ver2010;
                    case "15.0":
                        return AccessVersion.Ver2013;
                    case "16.0":
                        return AccessVersion.Ver2016;
                    case "17.0":
                        return AccessVersion.Ver2019;
                }
            }
            catch (Exception _)
            {
            }

            return AccessVersion.Ver2016;
        }




    }
}
