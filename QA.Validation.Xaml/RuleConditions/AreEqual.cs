
namespace QA.Validation.Xaml
{
    /// <summary>
    /// Проверяет два свойства на равенство
    /// </summary>
    public class AreEqual : PropertyValidationCondition
    {      
        public PropertyDefinition Target { get; set; }

        public override bool Execute(ValidationConditionContext context)
        {
            var source = Source ?? context.Definition;
            var value1 = context.ValueProvider.GetValue(source);
            var value2 = context.ValueProvider.GetValue(Target);

            var isValid = object.Equals(value1, value2);

            return isValid;
        }
    }
}
