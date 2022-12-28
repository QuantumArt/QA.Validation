namespace QA.Validation.Xaml
{
    /// <summary>
    /// Сравнивает значения поля с True
    /// </summary>
    public class IsTrue : PropertyValidationCondition
    {
        public override bool Execute(ValidationConditionContext context)
        {
            var definition = Source ?? context.Definition;
            var value1 = context.ValueProvider.GetValue(definition);

            return object.Equals(value1, true);
        }
    }
}
