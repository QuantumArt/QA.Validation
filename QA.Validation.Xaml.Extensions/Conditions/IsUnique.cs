using System;
using System.Linq;
using System.Windows.Markup;

namespace QA.Validation.Xaml.Extensions.Conditions
{
    using System.Collections;
    using System.Collections.Generic;
    using QA.Validation.Xaml;

    /// <summary>
    /// Условие валидации, проверяющее, еслить ли повторы в коллекции
    /// На вход принимает любые типы, реализующие IEnumerable
    /// </summary>
    [ContentProperty("Value")]
    public class IsUnique : PropertyValidationCondition
    {
        public override bool Execute(ValidationConditionContext context)
        {
            var value1 = context.ValueProvider.GetValue(Source ?? context.Definition);

            if (value1 is IEnumerable)
            {
                return Array.TrueForAll(
                    ((IEnumerable)value1)
                        .Cast<object>()
                        .GroupBy(x => x).ToArray(), 
                    gr => gr.Count() == 1);
            }

            throw GetNotSupportedException(value1);
        }
    }
}
