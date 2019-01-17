using System.Web.Script.Serialization;
using System.Windows.Markup;
using Portable.Xaml.Markup;

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
                    var serializer = new JavaScriptSerializer();
                    var obj = serializer.DeserializeObject((string)value1);

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
