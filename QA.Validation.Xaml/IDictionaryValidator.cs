using System;
using System.Collections.Generic;
namespace QA.Validation.Xaml
{
    /// <summary>
    /// Валидатор данных
    /// </summary>
    public interface IDictionaryValidator
    {
        /// <summary>
        /// Валидация модели данных
        /// </summary>
        /// <param name="values">Данные</param>
        /// <param name="result">результат валидации</param>
        void Validate(Dictionary<string, string> values, ValidationContext result);
    }
}
