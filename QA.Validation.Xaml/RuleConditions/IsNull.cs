using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    [ContentProperty("Prop1")]
    public class IsNull : PropertyValidationCondition
    {
        public override bool Execute(ValidationConditionContext context)
        {
            var source = Source ?? context.Definition;
            var value1 = context.ValueProvider.GetValue(source);

            return object.Equals(value1, null);
        }
    }
}
