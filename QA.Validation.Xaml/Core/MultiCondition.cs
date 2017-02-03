using System.Collections.Generic;
using System.Windows.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Базовый класс для условий-контейнеров валидации
    /// </summary>
    [ContentProperty("Items")]
    public abstract class MultiCondition : ValidationCondition
    {
        /// <summary>
        /// Коллекция вложенных условий
        /// </summary>
        public List<ValidationCondition> Items { get; private set; }

        public MultiCondition()
        {
            Items = new List<ValidationCondition>();
        }
    }
}
