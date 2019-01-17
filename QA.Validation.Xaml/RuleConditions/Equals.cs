using System.ComponentModel;
using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    [ContentProperty("Value")]
    public class Equals : PropertyValidationCondition
    {
        public object Value { get; set; }

        public override bool Execute(ValidationConditionContext context)
        {
            var definition = Source ?? context.Definition;
            var value1 = context.ValueProvider.GetValue(definition);
            object value2 = Value;

            if (value2 != null)
            {
                var converter = TypeDescriptor.GetConverter(definition.PropertyType);

                if (converter.CanConvertFrom(Value.GetType()))
                {
                    value2 = converter.ConvertFrom(value2);
                }
                
            }

            return object.Equals(value1, value2);
        }
    }
}
