#if NET_STANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif

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
