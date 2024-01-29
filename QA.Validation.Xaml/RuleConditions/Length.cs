using System.Linq;
using System.Collections;
using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Проверяет длину строки, массива или любого типа, реализующего IEnumerable
    /// </summary>
    [ContentProperty("Value")]
    public class Length : PropertyValidationCondition
    {

        private LengthInternal _inner;

        /// <summary>
        /// Максимальная длина
        /// </summary>
        public int? MaxLength
        {
            get
            {
                return _inner.MaxLength;
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

        public Length()
        {
            _inner = new LengthInternal { MinLength = 0 };
        }
    }
}
