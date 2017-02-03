using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace QA.Validation.Xaml
{    
    /// <summary>
    /// Контейнер для типизированных валидаторов, который может быть секцией конфигурационного файла приложения.
    /// </summary>
    [ContentProperty("Validators")]
    public class ValidationConfiguration
    {
        public Dictionary<Type, XamlObjectValidator> Validators { get; set; }
        public ValidationConfiguration()
        {
            Validators = new Dictionary<Type, XamlObjectValidator>();
        }

        public XamlObjectValidator GetValidatorFor<T>()
        {
            return Validators[typeof(T)];
        }
    }
}
