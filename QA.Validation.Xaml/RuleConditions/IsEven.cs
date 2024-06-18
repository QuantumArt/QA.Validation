using System.Collections;
using System.Linq;

namespace QA.Validation.Xaml
{
    public class IsEven : PropertyValidationCondition
    {
        public override bool Execute(ValidationConditionContext context)
        {
            var source = Source ?? context.Definition;
            var value = context.ValueProvider.GetValue(source);

            return value switch
            {
                int intValue => IsEvenInt(intValue),
                string stringValue => IsEvenInt(stringValue.Length),
                IEnumerable enumerbleValue => IsEvenInt(enumerbleValue.Cast<object>().Count()),
                null => typeof(IEnumerable).IsAssignableFrom(source.PropertyType) || typeof(int).IsAssignableFrom(source.PropertyType)
                            ? false
                            : throw GetNullNotSupportedException(),
                _ => throw GetNotSupportedException(value),
            };
        }

        private bool IsEvenInt(int value) => value % 2 == 0;
    }
}
