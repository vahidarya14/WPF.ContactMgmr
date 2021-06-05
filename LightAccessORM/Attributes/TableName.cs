using System;

namespace LightAccessORM.Attributes
{
    public class  TableName : Attribute
    {
        public string Name { get; }

        public TableName(string name)
        {
            Name = name;
        }
    }
}
