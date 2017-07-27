using System;
using System.Collections.Generic;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Содержит информацию о текущей сессии валидации.
    /// </summary>
    public class ValidationConditionContext
    {
        /// <summary>
        /// Общий флаг успешности валидации
        /// </summary>
        public bool IsValid
        {
            get
            {
                return Messages.Count == 0 && Result.Errors.Count == 0;
            }
        }

        /// <summary>
        /// Valudation summary (нетаргетированные сообщения)
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Описание текущего поля
        /// </summary>
        public PropertyDefinition Definition { get; set; }

        /// <summary>
        /// Описания полей всего валидатора
        /// </summary>
        public IEnumerable<PropertyDefinition> All { get; set; }

        /// <summary>
        /// Результаты валидации (контейнер ошибок)
        /// </summary>
        public ValidationResult Result { get; set; }

        /// <summary>
        /// Провайдер значения поля
        /// </summary>
        public IValueProvider ValueProvider { get; set; }

        /// <summary>
        /// Провайдер доступа к конфигурационным объектам
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ValidationConditionContext()
        {
            Messages = new List<string>();
            Result = new ValidationResult();
        }
    }
}
