using System;
using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Оператор сравнения.
    /// Введенное значение приводится к типу поля.
    /// </summary>
    [ContentProperty("Value")]
    public class GreaterThan : PropertyValidationCondition
    {
        public IComparable Value { get; set; }

        public PropertyDefinition Target { get; set; }

        public override bool Execute(ValidationConditionContext context)
        {
            var source = Source ?? context.Definition;
            var value1 = context.ValueProvider.GetValue(source);

            if (value1 is IComparable)
            {
                if (Target == null)
                {
                    var val = (IComparable)Convert.ChangeType(Value, source.PropertyType);
                    return ((IComparable)value1).CompareTo(val) > 0;
                }
                else
                {
                    var value2 = context.ValueProvider.GetValue(Target);

                    return ((IComparable)value1).CompareTo(value2) > 0;
                }
            }
            else if (value1 is null && typeof(int).IsAssignableFrom(source.PropertyType))
            {
                return false;
            }

            throw GetNotSupportedException(value1);
        }
    }
}
