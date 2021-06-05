using System;

namespace LightAccessORM.Attributes
{
    public class FieldName : Attribute
    {
        public string Field { get;  }

        public FieldName(string field)
        {
            Field = field;
        }
    }
}
