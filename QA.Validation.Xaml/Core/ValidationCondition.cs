using System;
using System.Collections.Generic;
using System.IO;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Базовый класс для всех условий валидации
    /// </summary>
    public abstract class ValidationCondition
    {
        /// <summary>
        /// Остановить цепочку выполнения на этом условии
        /// </summary>
        public bool StopPropagation { get; set; }

        /// <summary>
        /// Добавлять диагностические сообщения в коллекцию ошибок
        /// </summary>
        
        public bool AddDiagnosticsSummary { get; set; }
        /// <summary>
        /// Выполнение проверок условия
        /// </summary>
        /// <param name="context">контекст</param>
        /// <returns>успешность выполнения (или результат)</returns>
        public abstract bool Execute(ValidationConditionContext context);

        /// <summary>
        /// Получение уникального имени для данного условия
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.GetType().Name + "_" + this.GetHashCode();
        }

        /// <summary>
        /// Генерация исключения (о неприменимости правила к данному значению поля)
        /// </summary>
        /// <param name="value">значение</param>
        /// <returns>Исключение</returns>
        protected Exception GetNotSupportedException(object value)
        {
            var inner = new NotSupportedException(string.Format("{0} validation condition cannot work with '{1}' of type '{2}'.", 
                    this.GetName(), value, value == null ? "x:Null" : value.GetType().ToString()));

            return new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ProvidedDataError, 
                "Provided data is invalid. Check inner exception for details. " + inner.Message,inner);
        }

        protected Exception GetNullNotSupportedException()
        {           
            return new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ProvidedDataError,
                "Provided data is invalid. Null values are not supported. Consider additional rule condition. ");
        }
    }
}
