// Owners: Karlov Nikolay

using System;
using System.Reflection;
using QA.Configuration;

namespace QA.Validation.Xaml.Fluent
{
    /// <summary>
    /// Базовый класс для типизированных валидаторов
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class AbstractValidator<TModel> : IObjectValidator
    {
        protected XamlObjectValidator Validator { get; private set; }

        public AbstractValidator()
        {
            Validator = CreateInstance(OnResolvePath());
            if (Validator.Type == null)
            {
                Validator.Type = typeof(TModel);
            }
            else if (Validator.Type != typeof(TModel))
            {
                throw new InvalidOperationException("This validator is not appliable to the type " + typeof(TModel));
            }
        }

        public void Validate(TModel obj, ValidationContext result)
        {
            OnValidate(obj, result);
        }

        protected virtual void OnValidate(TModel obj, ValidationContext result)
        {
            Validator.Validate(obj, result);
        }

        protected virtual XamlObjectValidator CreateInstance(string path)
        {
            using (var stream = Assembly.GetEntryAssembly()
                .GetManifestResourceStream(path))
            {
                // создаем экземпляр валидатора
                return (XamlObjectValidator)XamlConfigurationParser.CreateFrom(stream);
            }
        }

        protected abstract string OnResolvePath();

        void IObjectValidator.Validate(object obj, ValidationContext result)
        {
            Validate((TModel)obj, result);
        }
    }
}
