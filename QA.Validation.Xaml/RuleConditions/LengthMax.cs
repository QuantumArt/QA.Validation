using System.Linq;
using System.Collections;
using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Проверяет длину строки, массива или любого типа, реализующего IEnumerable
    /// </summary>
    [ContentProperty("Value")]
    public class LengthMax : PropertyValidationCondition
    {
        private Length _inner;

        /// <summary>
        /// Максимальная длина
        /// </summary>
        public int Value
        {
            get
            {
                return _inner.MaxLength ?? 0;
            }
            set
            {
                _inner.MaxLength = value;
            }
        }


        public override bool Execute(ValidationConditionContext context)
        {
            return _inner.Execute(context);
        }

        public LengthMax()
        {
            _inner = new Length { MinLength = 0 };
        }
    }
}
