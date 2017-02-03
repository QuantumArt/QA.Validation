using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Xaml;

namespace QA.Validation.Xaml.Initialization
{
    internal class ValidationManager
    {
        /// <summary>
        /// Кеш
        /// </summary>
        private readonly MemoryCache _cache;
        private int _cacheTime = 1000;
        private DynamicResourceDictionaryContainer _container;

        public ValidationManager()
        {
            _cache = new MemoryCache(Guid.NewGuid().ToString());
            DynamicResourceDictionaryContainer.SetResourceProvider((this).GetDynamicResource);
        }

        public ValidationContext ValidateModel(Dictionary<string, string> model,
            string validatorText,
            string[] aggregatedValidatorList,
            string dynamicResourceText, IServiceProvider serviceProvider)
        {
            return ValidateInternal(model, validatorText, aggregatedValidatorList, dynamicResourceText, true, serviceProvider);
        }


        public void TestResourceDictionary(string text)
        {
            lock (_cache)
            {
                GetResourceDictionary(text, false);
            }
        }

        /// <summary>
        /// Проверяет корректность валидаторов. Проверяется корректность языка Xaml.
        /// Необходимо передавать ресурсный словарь.
        /// </summary>
        /// <param name="validatorText">Текст валидатора</param>
        /// <param name="dynamicResourceText">Текст ресурсного словаря</param>
        public void TestValidator(string validatorText, string dynamicResourceText)
        {
            List<IDictionaryValidator> validators = new List<IDictionaryValidator>();

            lock (_cache)
            {
                CreateValidators(validatorText, new string[] { }, dynamicResourceText, false, validators);
            }
        }

        /// <summary>
        /// Проверяет корректность валидаторов.
        /// Необходимо передавать ресурсный словарь.
        /// </summary>
        /// <param name="model">Словарь с полями формы</param>
        /// <param name="validatorText">Текст валидатора</param>
        /// <param name="dynamicResourceText">Текст ресурсного словаря</param>
        public void TestValidator(Dictionary<string, string> model,
            string validatorText, string dynamicResourceText, 
            IServiceProvider serviceProvider)
        {
            List<IDictionaryValidator> validators = new List<IDictionaryValidator>();

            lock (_cache)
            {
                CreateValidators(validatorText, new string[] { }, dynamicResourceText, false, validators);
            }

            var fields = validators.Cast<XamlValidator>()
                .SelectMany(x => x.Definitions)
                .Select(x => x.Value);

            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field.PropertyName))
                {
                    throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ValidatorError,
                        "Validator is not valid. Some property definitions have not PropertyName specified.");
                }

                if (field.PropertyType == null)
                {
                    throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ValidatorError,
                        "Validator is not valid. Some property definitions have not PropertyType specified.");
                }
            }

            var uniqueFields = fields
                .Select(x => x.PropertyName)
                .Distinct();

            // проверяем, что все указанные в описании поля присутствуют в модели
            foreach (var field in uniqueFields)
            {
                if (!model.ContainsKey(field))
                {
                    throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ValidatorError,
                        string.Format("Validator is not valid. Property  with name {0} is not presented in model.", field));
                }
                var _ = model[field];
            }

            // Производим валидацию. Результат не интересует, важно, чтобы ничего не упало.

            var result = new ValidationContext();
            result.ServiceProvider = serviceProvider;

            Array.ForEach(validators.ToArray(),
                x => x.Validate(model, result));
        }

        public string GenerateXamlValidatorText(IEnumerable<PropertyDefinition> definitions)
        {
            // валидация корректности

            // генерация текста
            var validator = new XamlValidator();
            foreach (var definition in definitions)
            {
                validator.Definitions.Add(definition.Alias, definition);
            }

            return XamlServices.Save(validator);
        }

        public string GenerateDynamicResourceText(DynamicResourceDictionaryContainer container)
        {
            if (container == null)
            {
                container = new DynamicResourceDictionaryContainer();
            }

            return XamlServices.Save(container);
        }
        public PropertyDefinition[] GetPropertyDefinitions(string validatorText, string dynamicResourceText)
        {
            List<IDictionaryValidator> validators = new List<IDictionaryValidator>();

            lock (_cache)
            {
                CreateValidators(validatorText, new string[] { }, dynamicResourceText, false, validators);
            }

            return ((IDefinitionStorage)validators.First()).GetAll();
        }

        DynamicResourceDictionaryContainer GetDynamicResource()
        {
            return _container;
        }


        #region Protected members
        protected ValidationContext ValidateInternal(Dictionary<string, string> model, 
            string validatorText,
            string[] aggregatedValidatorList,
            string dynamicResourceText, 
            bool useCache,
            IServiceProvider serviceProvider)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (string.IsNullOrEmpty(validatorText))
            {
                throw new ArgumentNullException("validatorText");
            }

            List<IDictionaryValidator> validators = new List<IDictionaryValidator>();

            lock (_cache)
            {
                CreateValidators(validatorText, aggregatedValidatorList, dynamicResourceText, useCache, validators);
            }

            var result = new ValidationContext();
            result.ServiceProvider = serviceProvider;

            foreach (var validator in validators)
            {
                validator.Validate(model, result);
            }

            return result;
        }

        private void CreateValidators(string validatorText, string[] aggregatedValidatorList, string dynamicResourceText, bool useCache, List<IDictionaryValidator> validators)
        {
            if (!string.IsNullOrEmpty(dynamicResourceText))
            {
                _container = GetResourceDictionary(dynamicResourceText, useCache);
            }

            validators.Add(GetValidator(validatorText, dynamicResourceText, useCache));

            foreach (var aggregatedText in aggregatedValidatorList)
            {
                validators.Add(GetValidator(aggregatedText, dynamicResourceText, useCache));
            }
        }

        protected IDictionaryValidator GetValidator(string text, string dynamicResourceText, bool useCache = true)
        {
            return GetOrPut(GetHashKey(text + dynamicResourceText, "validator"),
                useCache,
                () => (IDictionaryValidator)XamlServices.Parse(text));
        }

        protected DynamicResourceDictionaryContainer GetResourceDictionary(string text, bool useCache = true)
        {
            return GetOrPut(GetHashKey(text, "resource"),
                useCache,
                () => (DynamicResourceDictionaryContainer)XamlServices.Parse(text));
        }

        protected T GetOrPut<T>(string key, bool useCache, Func<T> valueProvider)
        {
            if (!useCache)
            {
                return valueProvider();
            }

            var result = (T)_cache.Get(key);

            if (result == null)
            {
                lock (_cache)
                {
                    result = (T)_cache.Get(key);

                    if (result == null)
                    {
                        result = valueProvider();
                        _cache.Set(key, result, new CacheItemPolicy
                        {
                            AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(_cacheTime)
                        });

                        return result;
                    }
                }
            }

            return result;
        }

        protected static string GetHashKey(string text, string prefix)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(text);
            byte[] hash = md5.ComputeHash(inputBytes);


            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return string.Format("{0}_{1}", prefix, sb.ToString());
        }
        #endregion
    }
}
