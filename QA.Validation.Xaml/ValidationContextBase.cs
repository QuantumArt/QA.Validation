using System.Collections.Generic;

namespace QA.Validation.Xaml
{
    public abstract class ValidationContextBase
    {
        /// <summary>
        /// Нетаргетированные сообщения об ошибках (validation summary)
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Объект с ошибками валидации конкретных полей
        /// </summary>
        public ValidationResult Result { get; set; }

        public ValidationContextBase()
        {
            Result = new ValidationResult();
            Messages = new List<string>();
        }
    }
}
