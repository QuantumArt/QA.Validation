using System;
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Условие соответствия регилярному выражению.
    /// </summary>
    [ContentProperty("Expression")]
    public class Matches : PropertyValidationCondition
    {
        /// <summary>
        /// Текст регулярного выражения
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Опции регулярного выражения
        /// </summary>
        public RegexOptions RegexOptions { get; set; }

        public override bool Execute(ValidationConditionContext context)
        {
            var value1 = context.ValueProvider.GetValue(Source ?? context.Definition);

            if (value1 is string)
            {
                Regex regex = new Regex(Expression, RegexOptions);
                return regex.IsMatch((string)value1);
            }

            throw GetNotSupportedException(value1);
        }
    }
}
