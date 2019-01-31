using System.ComponentModel;
using QA.Validation.Xaml.TypeConverters;
using System;
#if NETSTANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Добавление сообщения об ошибке.
    /// </summary>
    [ContentProperty("Text")]
    [TypeConverter(typeof(WithMessageTypeConverter))]
    public class WithValueFormattedMessage : WithLocalizedMessage
    {
        public override bool Execute(ValidationConditionContext context)
        {
            if (context.Definition.PropertyType != null)
            {
                context.Result.AddError(context.Definition,
                    string.Format(GetText(), context.ValueProvider.GetValue(context.Definition)));
            }
            else
            {
                throw new NotSupportedException("WithValueFormattedMessage can only be used in ForMember clause.");
            }

            return true;
        }
    }
}
