using LightAccessORM.Attributes;
using System.Collections.Generic;

namespace DAL.DataModel
{
    public class Contact
    {
        public long Id { get; set; }

        [FieldName("CName")]
        public string Name { get; set; }
        public string Title { get; set; }
        public string Mobile { get; set; }
        public string Company { get; set; }
        public string WebSite { get; set; }
        public string EMail { get; set; }
        public string Address { get; set; }
        public string GroupName { get; set; }
        public string Fax { get; set; }
        public string Extenssion { get; set; }//Domestic
        public string Tel { get; set; }
        public Gender Gender { get; set; }
        public string Remark { get; set; }

        [Ignore]
        public bool IsEditable { get;set;}
        public List<ExtraField> ExtraFields { get; set; }

        public Contact()
        {
            IsEditable = false;
            ExtraFields=new List<ExtraField>();
        }
    }

    public enum Gender
    {
        Mr=1,
        Mis=2,
        _= 0
    }

    [TableName("ContactExtraFiled")]
    public class ExtraField
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public long ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
