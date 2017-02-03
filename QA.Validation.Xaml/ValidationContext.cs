using System;
using System.Collections.Generic;
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
        public bool IsValid
        {
            get
            {
                return Messages.Count == 0 && Result.Errors.Count == 0;
            }
        }

        /// <summary>
        /// Объект, предоставляющий доступ правилам валидации к дополнительному функционалу.
        /// Является необязательным
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        public ValidationContext()
            : base()
        {
            ServiceProvider = new ConfigurationProvider();
        }
    }
}
