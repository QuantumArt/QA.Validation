using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Проверяет длину строки, массива или любого типа, реализующего IEnumerable
    /// </summary>
    [ContentProperty("Value")]
    public class LengthExact : PropertyValidationCondition
    {
        private Length _inner;

        /// <summary>
        /// Точная длина
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
                _inner.MinLength = value;
            }
        }

        public override bool Execute(ValidationConditionContext context)
        {
            return _inner.Execute(context);
        }

        public LengthExact()
        {
            _inner = new Length();
        }
    }
}
