using System;
using System.ComponentModel;
using System.Globalization;

namespace QA.Validation.Xaml.TypeConverters
{
    public class WithMessageTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType.Equals(typeof(string)))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, 
            CultureInfo culture, 
            object value)
        {
            if (value is string)
            {
                return new WithMessage { Text = (string)value };
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
