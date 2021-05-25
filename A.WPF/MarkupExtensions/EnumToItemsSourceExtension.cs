using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;


namespace A.WPF
{
    //usage : 
    //<ComboBox
    //     ItemsSource = "{wpfCommon:EnumToItemsSource {x:Type enums:Undercable}}"
    //     SelectedValue="{Binding Cable.Undercable}"
    //     SelectedValuePath="Value" 
    //     DisplayMemberPath="DisplayName"/>
    public class EnumToItemsSourceExtension : MarkupExtension
    {
        private readonly Type _type;

        public EnumToItemsSourceExtension(Type type)
        {
            _type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_type)
                .Cast<object>()
                .Select(e => new { Value = e, DisplayName = DescriptionAttr(e) });
        }

        string DescriptionAttr<T>( T source)
        {
            if (source == null) return "";
            if (source is Type)
            {
                var attributes1 = (DescriptionAttribute[])(source as Type).GetCustomAttributes(typeof(DescriptionAttribute), false);
                return attributes1.Length > 0 ? attributes1[0].Description : source.ToString();
            }

            var fi = source.GetType().GetField(source.ToString());
            if (fi == null)
            {
                var attributes1 = source.GetType().GetCustomAttribute(typeof(DescriptionAttribute), false);
                return attributes1 != null ? ((DescriptionAttribute)attributes1).Description : source.GetType().Name;
            }

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : source.ToString();
        }

    }
 
}
