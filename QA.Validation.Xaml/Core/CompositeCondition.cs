using System.Linq;


namespace QA.Validation.Xaml
{
    /// <summary>
    /// Базовый класс для составных условий валидации
    /// </summary>
    public abstract class CompositeCondition : MultiCondition
    {
        /// <summary>
        /// Дочернее условие
        /// </summary>
        public ValidationCondition Condition
        {
            get { return Items.Any() ? Items.First() : null; }
            set
            {
                Items.Clear();
                Items.Add(value);
            }
        }
    }
}
