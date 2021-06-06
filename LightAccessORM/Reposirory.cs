using LightAccessORM.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LightAccessORM
{
    public abstract class Reposirory<T> where T : class, new()
    {
        protected Access _cmd;

        public Reposirory(string accessFilePath)
        {
            _cmd = new Access(accessFilePath, AccessVersion.Auto);
        }


        public virtual int Add(T c)
        {
            var queries = new Reflection().AddQuery(c);

            var newObjIds = new List<(long id, string tbl)>();
            var ss = 0;
            for (var i = queries.Count; i > 0; i--)
            {
                var query =
                    queries[i - 1].masterTbl == null ? queries[i - 1].query :
                    queries[i - 1].query.Replace($"@{queries[i - 1].masterTbl}Id", $"{newObjIds.First(x => x.tbl == queries[i - 1].masterTbl).id}");
                ss = CommandText(query);
                var lastId = Max($"select max(id) from {queries[i - 1].tbl}");
                newObjIds.Add((lastId, queries[i - 1].tbl));
            }

            return ss;
        }

        public virtual int Update(T c)
        {
            var queries = new Reflection().UpdateQuery(c);

            var newObjIds = new List<(long id, string tbl)>();
            var ss = 0;
            for (var i = queries.Count; i > 0; i--)
            {
                var query =
                    queries[i - 1].masterTbl == null ? queries[i - 1].query :
                    queries[i - 1].query.Replace($"@{queries[i - 1].masterTbl}Id", $"{newObjIds.First(x => x.tbl == queries[i - 1].masterTbl).id}");
                ss = CommandText(query);
                var lastId =long.Parse( c.GetType().GetProperty("Id").GetValue(c).ToString());
                newObjIds.Add((lastId, queries[i - 1].tbl));
            }

            return ss;
        }



        public int Delete(long id, bool receursive)
        {
            var ss = CommandText($"DELETE FROM {typeof(T).Name} WHERE [Id]=" + id);

            if (receursive)
            {
                var subClassess = new Reflection().SubClasses(typeof(T));
                try
                {
                    foreach (var item in subClassess)
                    {
                        ss = CommandText($"DELETE FROM {item} WHERE [{typeof(T).Name}Id]=" + id);
                    }
                }
                catch (Exception)
                {
                }

            }

            return ss;
        }

        public int Delete(string where)
        {
            return CommandText($"DELETE FROM {typeof(T).Name} WHERE {where}");
        }


        public long Max(string query)
        {
            var reader = _cmd.Max(query);

            return long.Parse(reader.ToString());
        }

        public string Max(string query, int f = 0)
        {
            var reader = _cmd.Max(query);

            return reader.ToString();
        }

        public List<T> Select(string query,Func<object[],T> makeTFromDataRow)
        {
            var reader = _cmd.ExecuteReader(query);

            var rowList = new List<T>();
            while (reader != null && reader.Read())
            {
                var values = new object[reader.FieldCount];
                reader.GetValues(values);
                var t=makeTFromDataRow(values);
                rowList.Add(t);
            }
            reader.Close();
            return rowList;
        }
        public List<T> Select<T>(string query, Func<object[], T> makeTFromDataRow)
        {
            var reader = _cmd.ExecuteReader(query);

            var rowList = new List<T>();
            while (reader != null && reader.Read())
            {
                var values = new object[reader.FieldCount];
                reader.GetValues(values);
                var t = makeTFromDataRow(values);
                rowList.Add(t);
            }
            reader.Close();
            return rowList;
        }


        //------------------------
        protected int CommandText(string query) => _cmd.CommandText(query);

        //------------------------

        class Reflection
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            public List<(string query, string tbl, string masterTbl)> AddQuery(object c)
            {
                _addQueries = new List<(string query, string tbl, string masterTbl)>();
                MakeAddQuery(c);
                return _addQueries;
            }
            List<(string query, string tbl, string masterTbl)> _addQueries = new List<(string query, string tbl, string masterTbl)>();
            void MakeAddQuery(object c, string masterTbl = null)
            {
                var objType = c.GetType();

                var tblAttr = objType.GetCustomAttributes(true).FirstOrDefault(x => x is TableName);
                var tblName = tblAttr != null ? ((TableName)tblAttr).Name : objType.Name;
                var q = $"INSERT INTO  [{tblName}] (";
                var props = objType.GetProperties();
                for (int i = 0; i < props.Length; i++)
                {
                    if (props[i].Name.ToLower() == "id") continue;
                    var attrs = props[i].GetCustomAttributes(true);

                    var isIgnored = attrs.Any(x => x is Ignore);
                    if (isIgnored) continue;

                    var fied = attrs.FirstOrDefault(x => x is FieldName);
                    var fielsName = fied != null ? ((FieldName)fied).Field : props[i].Name;

                    if (props[i].PropertyType.IsClass && props[i].PropertyType != typeof(string))
                        continue;


                    q += $" [{fielsName}],";
                }
                q = $"{q.Substring(0, q.Length - 1)}) VALUES( ";

                for (int i = 0; i < props.Length; i++)
                {
                    if (props[i].Name.ToLower() == "id") continue;
                    var attrs = props[i].GetCustomAttributes(true);

                    var isIgnored = attrs.Any(x => x is Ignore);
                    if (isIgnored) continue;

                    var field = attrs.FirstOrDefault(x => x is FieldName);
                    var fieldName = field != null ? ((FieldName)field).Field : props[i].Name;

                    if (!string.IsNullOrWhiteSpace(masterTbl) && fieldName.ToLower() == $"{masterTbl.ToLower()}id")
                    {
                        q += $" @{masterTbl}Id,";
                        continue;
                    }

                    if (props[i].PropertyType.IsClass && props[i].PropertyType != typeof(string))
                    {
                        if (typeof(IList).IsAssignableFrom(props[i].PropertyType) && props[i].PropertyType.IsGenericType)
                        {
                            IList item = (IList)props[i].GetValue(c, null);
                            if (item != null)
                                foreach (object o in item)
                                {
                                    MakeAddQuery(o, tblName);
                                }
                        }
                        else
                        {
                            var o = props[i].GetValue(c, null);
                            if (o != null)
                                MakeAddQuery(o, tblName);
                        }
                    }
                    else
                    {
                        if (props[i].PropertyType == typeof(string))
                            q += $"'{props[i].GetValue(c)}',";
                        else if (props[i].PropertyType.IsEnum)
                        {
                            object underlyingValue = Convert.ChangeType(props[i].GetValue(c), typeof(int));
                            q += $"{underlyingValue},";
                        }
                        else
                            q += $"{props[i].GetValue(c)},";
                    }
                }
                q = $"{q.Substring(0, q.Length - 1)})";
                _addQueries.Add((q, tblName, masterTbl));

            }


            /// <summary>
            /// 
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
              public List<(string query, string tbl, string masterTbl)> UpdateQuery(object c)
            {
                _addQueries = new List<(string query, string tbl, string masterTbl)>();
                MakeUpdateQuery(c);
                return _addQueries;
            }
        public void MakeUpdateQuery(object c, string masterTbl = null)
            {
                var objType = c.GetType();
                var tblAttr = objType.GetCustomAttributes(true).FirstOrDefault(x => x is TableName);
                var tblName = tblAttr != null ? ((TableName)tblAttr).Name : objType.Name;

                var id = objType.GetProperty("Id").GetValue(c);
                if (id.ToString() == "0")
                {
                    MakeAddQuery(c, masterTbl);
                    return;
                }

                var q = $"UPDATE  [{tblName}] SET ";
                var props = objType.GetProperties();
                for (int i = 0; i < props.Length; i++)
                {
                    if (props[i].Name.ToLower() == "id") continue;

                    var attrs = props[i].GetCustomAttributes(true);

                    var isIgnored = attrs.Any(x => x is Ignore);
                    if (isIgnored) continue;

                    var field = attrs.FirstOrDefault(x => x is FieldName);
                    var fieldName = field != null ? ((FieldName)field).Field : props[i].Name;

                    if (!string.IsNullOrWhiteSpace(masterTbl) && props[i].Name.ToLower() == $"{masterTbl.ToLower()}id")
                    {
                        q += $"{fieldName}=@{masterTbl}Id,";
                        continue;
                    }

                    if (props[i].PropertyType.IsClass && props[i].PropertyType != typeof(string))
                    {
                        if (typeof(IList).IsAssignableFrom(props[i].PropertyType) && props[i].PropertyType.IsGenericType)
                        {
                            IList item = (IList)props[i].GetValue(c, null);
                            if (item != null)
                            {
                                foreach (object o in item)
                                {
                                    MakeUpdateQuery(o, tblName);
                                }
                            }
                        }
                        else
                        {
                            var o = props[i].GetValue(c, null);
                            if (o != null)
                                MakeUpdateQuery(o, tblName);
                        }
                    }
                    else
                    {
                        q += $" [{fieldName}]=";
                        if (props[i].PropertyType == typeof(string))
                            q += $"'{props[i].GetValue(c)}',";
                        else if (props[i].PropertyType.IsEnum)
                        {
                            object underlyingValue = Convert.ChangeType(props[i].GetValue(c), typeof(int));
                            q += $"{underlyingValue},";
                        }
                        else
                            q += $"{props[i].GetValue(c)},";
                    }
                }
                q = $"{q.Substring(0, q.Length - 1)} WHERE Id={objType.GetProperty("Id").GetValue(c)}";
                _addQueries.Add((q, tblName, masterTbl));
            }


            public List<string> SubClasses(Type objType)
            {
                var subclassess = new List<string>();

                var props = objType.GetProperties();
                for (int i = 0; i < props.Length; i++)
                {
                    var attrs = props[i].GetCustomAttributes(true);

                    var isIgnored = attrs.Any(x => x is Ignore);
                    if (isIgnored) continue;


                    if (props[i].PropertyType.IsClass && props[i].PropertyType != typeof(string))
                    {
                        var fied = attrs.FirstOrDefault(x => x is FieldName);
                        var fielsName = fied != null ? ((FieldName)fied).Field : props[i].Name;

                        if (typeof(IList).IsAssignableFrom(props[i].PropertyType) && props[i].PropertyType.IsGenericType)
                        {
                            var className = props[i].PropertyType.FullName.Replace("System.Collections.Generic.List`1[[", "").Split(',')[0];

                            var t = Type.GetType(className.Substring(className.LastIndexOf('.')+1));
                            var tblName = props[i].PropertyType.GetCustomAttributes(true).FirstOrDefault(x => x is TableName);

                            subclassess.Add(tblName != null ? ((TableName)tblName).Name : fielsName);

                        }
                        else
                        {
                            subclassess.Add(fielsName);
                        }

                    }
                }
                return subclassess;
            }


        }
    }



}
