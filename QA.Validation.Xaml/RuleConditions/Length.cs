using System;
using System.Linq;
using System.Collections;
using System.Windows.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Проверяет длину строки, массива или любого типа, реализующего IEnumerable
    /// </summary>
    [ContentProperty("Value")]
    public class Length : PropertyValidationCondition
    {
        /// <summary>
        /// Минимальная длина
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Максимальная длина
        /// </summary>
        public int? MinLength { get; set; }

        public override bool Execute(ValidationConditionContext context)
        {
            var source = Source ?? context.Definition;
            var value1 = context.ValueProvider.GetValue(source);

            if (value1 is string)
            {
                var val = (string)value1;
                return CheckCount(val.Length);
            }
            else if (value1 is IEnumerable)
            {
                var count = ((IEnumerable)value1)
                    .Cast<object>()
                    .Count();

                return CheckCount(count);
            }

            if (value1 == null)
            {
                if (typeof(IEnumerable).IsAssignableFrom(source.PropertyType))
                {
                    // трактуется как пустая коллекция
                    return CheckCount(0);
                }

                throw GetNullNotSupportedException();
            }

            throw GetNotSupportedException(value1);
        }

        private bool CheckCount(int count)
        {
            return (MaxLength != null ? count <= MaxLength : true) &&
                (MinLength != null ? count >= MinLength : true);
        }
    }
}
