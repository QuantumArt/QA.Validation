using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Операнд ОТРИЦАНИЕ
    /// </summary>
    public class Not : And
    {
        public override bool Execute(ValidationConditionContext context)
        {
            return !base.Execute(context);
        }
    }
}
