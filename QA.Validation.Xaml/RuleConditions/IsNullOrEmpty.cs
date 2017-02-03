
namespace QA.Validation.Xaml
{
    public class IsNullOrEmpty : PropertyValidationCondition
    {
        public override bool Execute(ValidationConditionContext context)
        {
            var source = Source ?? context.Definition;
            var value1 = context.ValueProvider.GetValue(source);


            return (value1 == null) || object.Equals(value1, string.Empty);
        }
    }
}
