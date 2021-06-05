using System.Collections.Generic;
using System.Linq;
using A.Extenssions;
using DAL.DataModel;
using LightAccessORM;
using Serviecs;

namespace DAL.Access
{
    public class ContactRepository : Reposirory<Contact>
    {

        public ContactRepository():base(StaticPublic.MdbPath){}

        public List<Contact> Select()
        {
            var reader = _cmd.ExecuteReader("Select * from Contact order by [Company]");

            var rowList = new List<Contact>();
            while (reader != null && reader.Read())
            {
                var values = new object[reader.FieldCount];
                var i = reader.GetValues(values);
                rowList.Add(new Contact
                {
                    Id = long.Parse(values[0].ToString()),
                    Name = values[1].ToString(),
                    Title = values[2].ToString(),
                    Mobile = values[3].ToString(),
                    Company = values[4].ToString(),
                    WebSite = values[5].ToString(),
                    EMail = values[6].ToString(),
                    Address = values[7].ToString(),
                    GroupName = values[8].ToString(),
                    Fax = values[9].ToString(),
                    Extenssion = values[10].ToString(),
                    Tel = values[11].ToString(),
                    Gender = values[12].ToString().ToGender(),
                    Remark = values[13].ToString(),
                });
            }
            reader.Close();

            var extfid = SelectContactExtraFields("Select * from ContactExtraFiled");
            foreach (var contact in rowList)
            {
                contact.ExtraFields = extfid.Where(a => a.ContactId == contact.Id).ToList();
            }

            return rowList;
        }

        public List<ExtraField> SelectContactExtraFields(string query)
        {
            var reader = _cmd.ExecuteReader(query);

            var rowList = new List<ExtraField>();
            while (reader != null && reader.Read())
            {
                var values = new object[reader.FieldCount];
                var i = reader.GetValues(values);
                rowList.Add(new ExtraField
                {
                    Id = long.Parse(values[0].ToString()),
                    Key = values[1].ToString(),
                    Value = values[2].ToString(),
                    ContactId = long.Parse(values[3].ToString())
                });
            }
            reader.Close();
            return rowList;
        }


        //public override int Add(Contact c)
        //{

        //    var ss = CommandText("INSERT INTO Contact ([CName],[Title],[Mobile], [Company],[WebSite],[EMail], [Address],[GroupName],[Fax],[Extenssion],[Tel],[Gender],[Remark])" +
        //                         " VALUES('" + c.CName + "','" + c.Title + "','" + c.Mobile + "','" + c.Company + "','" + c.WebSite + "','" + c.EMail + "','" + c.Address + "','" + c.GroupName + "','" + c.Fax + "','" + c.Extenssion + "','" + c.Tel + "','" + (int)c.Gender + "','" + c.Remark + "')");


        //    foreach (var extraField in c.ExtraFields)
        //        CommandText($"INSERT INTO ContactExtraFiled ([Key],[Value],[Contactid]) VALUES('{extraField.Key}','{extraField.Value}','{c.Id}'");


        //    return ss;
        //}

        //public int AddExtraFiled(ExtraField extraField, Contact c)
        //{
        //   return  CommandText("INSERT INTO ContactExtraFiled ([Key],[Value],[Contactid]) VALUES('" + extraField.Key + "','" + extraField.Value + "','" + c.Id + "')");

        //}


        //public override int Update(Contact c)
        //{

        //    CommandText("UPDATE Contact SET " +
        //            "[CName]='" + c.Name + "',[Title]='" + c.Title + "',[Mobile]='" + c.Mobile + "',[Company]='" + c.Company + "',[WebSite]='" + c.WebSite + "',[EMail]='" + c.EMail +
        //            "',[Address]='" + c.Address + "',[GroupName]='" + c.GroupName + "',[Fax]='" + c.Fax + "',[Extenssion]='" + c.Domestic + "',[Tel]='" + c.Tel + "',[Gender]='" + (int)c.Gender +
        //            "',[Remark]='" + c.Remark +
        //            "' WHERE [Id]=" + c.Id);

        //    var ss = Delete("ContactExtraFiled", $"[Contactid]={c.Id}");
        //    foreach (var extraField in c.ExtraFields)
        //    {
        //        ss = AddExtraFiled(extraField, c);
        //    }

        //    //FilterdContacts = Contacts;

        //    return ss;

        //}

        public  int Delete(long id)
        {
            var ss = CommandText("DELETE FROM ContactExtraFiled WHERE [Contactid]=" + id);
            ss = Delete( id,true);

            return ss;
        }


    }
}