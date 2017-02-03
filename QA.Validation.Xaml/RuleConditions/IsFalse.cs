
namespace QA.Validation.Xaml
{
    /// <summary>
    /// Сравнивает значения поля с False
    /// </summary>
    public class IsFalse : PropertyValidationCondition
    {
        public override bool Execute(ValidationConditionContext context)
        {
            var definition = Source ?? context.Definition;
            var value1 = context.ValueProvider.GetValue(definition);

            return object.Equals(value1, false);
        }
    }
}
