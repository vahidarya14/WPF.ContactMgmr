using System.Collections;
using Serviecs;

namespace DAL.Access
{
    public abstract class Reposirory<T> where T:class,new()
    {
        protected Access _cmd;

        public Reposirory(string accessFilePath)
        {
           _cmd=new Access(accessFilePath, AccessVersion.Auto);
        }
        public Reposirory():this(StaticPublic.MdbPath) { }

        public abstract int Add(T c);
        public abstract int Delete(long id);


        public int CommandText(string query)
        {
            return _cmd.CommandText(query);
        }

    
        public long Max(string query)
        {        
            var reader = _cmd.Max(query);

            return long.Parse(reader.ToString());
        }

        public string Max(string query,int f=0)
        {
            var reader = _cmd.Max(query);

            return reader.ToString();
        }

        public ArrayList SelectCommand(string query)
        {
         
            var reader = _cmd.ExecuteReader(query);

            var rowList = new ArrayList();
            while (reader != null && reader.Read())
            {
                var values = new object[reader.FieldCount];
                //var i = reader.GetValues(values);
                rowList.Add(values);
            }
            reader.Close();
            return rowList;
        }







        //------------------------


        //------------------------


    }
}
