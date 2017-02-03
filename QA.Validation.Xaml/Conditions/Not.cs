using System.Windows.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Операнд ОТРИЦАНИЕ
    /// </summary>
    [ContentProperty("Condition")]
    public class Not : CompositeCondition
    {
        public override bool Execute(ValidationConditionContext context)
        {
            return !Condition.Execute(context);
        }
    }
}
