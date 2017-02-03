using System;
using System.Collections.Generic;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Контейнер с ошибками валидации полей
    /// </summary>
    [Serializable]
    public class ValidationResult
    {
        /// <summary>
        /// Список ошибок
        /// </summary>
        public List<ValidationError> Errors { get; set; }

        public ValidationResult()
        {
            Errors = new List<ValidationError>();
        }

        /// <summary>
        /// Добавить ошибку для поля
        /// </summary>
        public void AddError(PropertyDefinition definition, string message)
        {
            Errors.Add(new ValidationError
            {
                Definition = definition.Copy(),
                Message = message
            });
        }

        /// <summary>
        /// Добавить ошибку для поля
        /// </summary>
        public void AddError(string propertyName, string message)
        {
            AddError(new PropertyDefinition(null, propertyName, null), message);
        }
    }
}
