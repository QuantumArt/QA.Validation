using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Базовый класс для составных условий валидации
    /// </summary>
    [ContentProperty("Condition")]
    public abstract class CompositeCondition : ValidationCondition
    {
        /// <summary>
        /// Дочернее условие
        /// </summary>
        public ValidationCondition Condition { get; set; }
    }
}
