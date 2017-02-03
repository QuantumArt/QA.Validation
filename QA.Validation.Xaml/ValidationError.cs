
using System;
namespace QA.Validation.Xaml
{
    /// <summary>
    /// Объект с информацией об ошибке валидации
    /// </summary>
    [Serializable]
    public class ValidationError
    {
        /// <summary>
        /// Описание поля
        /// </summary>
        public PropertyDefinition Definition { get; set; }

        /// <summary>
        /// Валидационное сообщение
        /// </summary>
        public string Message { get; set; }
    }
}
