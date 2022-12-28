// Owners: Karlov Nikolay

using System;
using System.Collections.Generic;
using QA.Configuration;
using QA.Validation.Xaml.Adapters;

namespace QA.Validation.Xaml.Fluent
{
    /// <summary>
    /// Базовый класс для типизированных валидаторов
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class AbstractValidator<TModel, TAdapter> : IObjectValidator
        where TAdapter : IModelAdapter
    {
        protected Func<AbstractValidator<TModel, TAdapter>, TAdapter> _adapterProvider{get; private set;}
        protected XamlValidator Validator { get; private set; }

        protected AbstractValidator(Func<AbstractValidator<TModel, TAdapter>, TAdapter> adapterProvider)
        {
            _adapterProvider = adapterProvider;
            Validator = CreateInstance(OnResolvePath());
        }

        public void Validate(TModel obj, ValidationContext result)
        {
            OnValidate(obj, result);
        }

        protected virtual void OnValidate(TModel obj, ValidationContext result)
        {
            if (object.Equals(obj, null))
            {
                throw new ArgumentNullException("obj");
            }

            var adapter = _adapterProvider(this);
            Dictionary<string, string> form = adapter.AdaptModel<TModel>(obj);

            Validator.Validate(form, result);
        }

        protected virtual XamlValidator CreateInstance(string path)
        {
            using (var stream = this.GetType().Assembly.GetManifestResourceStream(path))
            {
                // создаем экземпляр валидатора
                return (XamlValidator)XamlConfigurationParser.LoadFrom(stream);
            }
        }

        protected abstract string OnResolvePath();

        void IObjectValidator.Validate(object obj, ValidationContext result)
        {
            Validate((TModel)obj, result);
        }
    }
}
