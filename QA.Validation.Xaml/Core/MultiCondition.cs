using System.Collections.Generic;
#if NETSTANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif

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
        public IList<ValidationCondition> Items { get; private set; }

        public MultiCondition()
        {
            Items = new List<ValidationCondition>();
        }
    }
}
