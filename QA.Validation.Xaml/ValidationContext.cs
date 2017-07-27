using System;
using QA.Validation.Xaml.Core;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Объект с результатами валидации
    /// </summary>
    public class ValidationContext : ValidationContextBase
    {
        /// <summary>
        /// Флаг корректноси модели (результат валидации)
        /// </summary>
        public bool IsValid => Messages.Count == 0 && Result.Errors.Count == 0;

        /// <summary>
        /// Кастомер-код    
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// ID сайта
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Объект, предоставляющий доступ правилам валидации к дополнительному функционалу.
        /// Является необязательным
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ValidationContext()
        {
            ServiceProvider = new ConfigurationProvider();
        }
    }
}
