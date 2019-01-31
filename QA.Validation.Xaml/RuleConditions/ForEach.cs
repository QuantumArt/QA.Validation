using System;
#if NETSTANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Операнд ЦИКЛ
    /// </summary>
    [ContentProperty("Condition")]
    public class ForEach : PropertyValidationCondition
    {
        public override bool Execute(ValidationConditionContext context)
        {
            var value1 = context.ValueProvider.GetValue(Source ?? context.Definition);

            throw new NotImplementedException("ForEach condition is not implemented yet.");
        }
    }
}
