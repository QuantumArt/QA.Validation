using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using QA.Validation.Xaml.ListTypes;

namespace QA.Validation.Xaml.TypeConverters
{
    public class ListOfIntTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType.Equals(typeof(string)))
                return true;

            if ((typeof(IEnumerable)).IsAssignableFrom(sourceType))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }


        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                return new ListOfInt(((string)value).Split(' ', ',')
                   .Select(x => x.Trim())
                   .Where(x => !string.IsNullOrEmpty(x))
                   .Select(x => int.Parse(x)));
            }

            if (value is ListOfInt)
            {
                return value;
            }

            if (value is IEnumerable)
            {
                return new ListOfInt(((IEnumerable)value).Cast<int>());
            }

            throw new NotSupportedException("Conversion is not supported.");
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType.Equals(typeof(string)))
                return true;

            if ((typeof(IEnumerable)).IsAssignableFrom(destinationType))
                return true;

            return base.CanConvertFrom(context, destinationType);
        }

        /// <summary>
        /// Конвертировать в string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String))
            {
                if (value == null)
                {
                    return string.Empty;
                }
                if (value is ListOfInt)
                {
                    return string.Join(",", (ListOfInt)value);
                }
            }

            throw new NotSupportedException("Conversion is not supported.");
        }
    }
}
