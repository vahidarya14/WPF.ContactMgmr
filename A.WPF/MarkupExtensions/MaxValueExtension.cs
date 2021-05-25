using System;
using System.Windows.Markup;

namespace A.WPF
{
    //usage :
    //<Grid xmlns:ext="clr-namespace:CustomMarkupExtensions">
    //  <TextBlock Text = "{ext:MaxValue Value1=200,Value2=25}" />
    //</ Grid >
    public class MaxValueExtension : MarkupExtension
    {
        public MaxValueExtension() { }

        public MaxValueExtension(object value1, object value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        [ConstructorArgument("value1")]
        public object Value1 { get; set; }

        [ConstructorArgument("value2")]
        public object Value2 { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Value1 is IComparable && Value2 is IComparable)
            {
                IComparable val1 = Value1 as IComparable;
                IComparable val2 = Value2 as IComparable;

                if (val1.CompareTo(val2) >= 0)
                    return Value1.ToString();
                else
                    return Value2.ToString();
            }
            else
            {
                return string.Empty;
            }
        }


    }
}