using System;
using System.Collections.Generic;
using System.Linq;
#if NETSTANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif

namespace QA.Validation.Xaml
{
    [ContentProperty("Arguments")]
    public class FromFormatted : IValueArgument
    {
        public string Text { get; set; }
        public List<Object> Arguments { get; private set; }
        public FromFormatted()
        {
            Arguments = new List<Object>();
        }
        #region IValueArgument Members

        public object Process(PropertyDefinition definition, IValueProvider provider, ValidationConditionContext context, object currentValue)
        {
            if (Text == null)
                return null;

            return string.Format(Text, Arguments.Select(x =>
                x is IValueArgument ?
                ((IValueArgument)x).Process(definition, provider, context, currentValue) :
                x)
                .ToArray());
        }

        #endregion
    }
}
