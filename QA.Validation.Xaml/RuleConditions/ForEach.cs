using System;
using System.Collections;
using System.Windows.Markup;

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
