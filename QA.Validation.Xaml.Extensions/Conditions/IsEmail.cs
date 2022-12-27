using System.Text.RegularExpressions;
using Portable.Xaml.Markup;

namespace QA.Validation.Xaml.Extensions.Conditions
{
    using QA.Validation.Xaml;

    [ContentProperty("Value")]
    public class IsEmail : PropertyValidationCondition
    {
        static Regex regex = new Regex(@"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public override bool Execute(ValidationConditionContext context)
        {
            var value1 = context.ValueProvider.GetValue(Source ?? context.Definition);

            if (value1 is string)
            {
                return regex.IsMatch((string)value1);
            }

            throw GetNotSupportedException(value1);
        }
    }
}
