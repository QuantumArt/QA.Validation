using Newtonsoft.Json.Linq;
#if NET_STANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif

namespace QA.Validation.Xaml.Extensions.Conditions
{


    [ContentProperty("Value")]
    public class IsJson : PropertyValidationCondition
    {
        public override bool Execute(ValidationConditionContext context)
        {
            var value1 = context.ValueProvider.GetValue(Source ?? context.Definition);

            if (value1 is string)
            {
                try
                {
                    var obj = JObject.Parse((string)value1);
                    return obj != null;
                }
                catch
                {
                    return false;
                }
            }

            throw GetNotSupportedException(value1);
        }
    }
}
