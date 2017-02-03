using System.Collections.Generic;
using System.Linq;

namespace QA.Validation.Xaml.Dynamic
{    
    /// <summary>
    /// Контекст для валидации Dynamic Expressions.
    /// Все методы и свойства доступны в текстовых выражениях
    /// </summary>
    public class DynamicContext
    {
        /// <summary>
        /// Текущие значения полей
        /// </summary>
        public IDictionary<string, object> Model { get; set; }

        /// <summary>
        /// Описание текущего поля
        /// </summary>
        public PropertyDefinition Source { get; set; }

        /// <summary>
        /// Валидационный контекст
        /// </summary>
        public ValidationConditionContext Context { get; set; }

        /// <summary>
        /// Текущее значение
        /// </summary>
        public object Value { get; set; }
                
        /// <summary>
        /// Проверяет, что указанное поле корректно
        /// </summary>
        /// <param name="propertyName">Alias поля</param>
        /// <returns></returns>
        public bool IsValid(string propertyName)
        {
            return Context.Result != null
                && !Context.Result
                .Errors
                .Any(x => x.Definition.Alias == propertyName);
        }

        /// <summary>
        /// Проверяет, есть ли ошибки валидации
        /// </summary>
        /// <returns></returns>
        public bool HasErrors() { return !Context.IsValid; }
    }

}
