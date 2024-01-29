﻿using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Проверяет длину строки, массива или любого типа, реализующего IEnumerable
    /// </summary>
    [ContentProperty("Value")]
    public class LengthInRange : PropertyValidationCondition
    {
        private LengthInternal _inner;

        /// <summary>
        /// Максимальная длина
        /// </summary>
        public int? To
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

        /// <summary>
        /// Минимальная длина
        /// </summary>
        public int? From
        {
            get
            {
                return _inner.MinLength;
            }
            set
            {
                _inner.MinLength = value;
            }
        }

        public override bool Execute(ValidationConditionContext context)
        {
            return _inner.Execute(context);
        }

        public LengthInRange()
        {
            _inner = new LengthInternal();
        }
    }
}
