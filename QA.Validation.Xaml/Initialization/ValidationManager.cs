using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Portable.Xaml;

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

        public ValidationContext ValidateModel(ValidationParamObject paramObject, IServiceProvider serviceProvider)
        {
            return ValidateInternal(paramObject, true, serviceProvider);
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
        /// <param name="baseValidatorText"></param>
        public void TestValidator(string validatorText, string dynamicResourceText, string baseValidatorText)
        {
            List<IDictionaryValidator> validators = new List<IDictionaryValidator>();

            lock (_cache)
            {
                var extValidatorTexts = (baseValidatorText != null) ? new string[] { validatorText } : new string[] {};
                baseValidatorText = baseValidatorText ?? validatorText;
                CreateValidators(baseValidatorText, extValidatorTexts, dynamicResourceText, false, validators);
            }
        }

        /// <summary>
        /// Проверяет корректность валидаторов.
        /// Необходимо передавать ресурсный словарь.
        /// </summary>
        /// <param name="model">Словарь с полями формы</param>
        /// <param name="validatorText">Текст валидатора</param>
        /// <param name="dynamicResourceText">Текст ресурсного словаря</param>
        /// <param name="baseValidatorText"></param>
        /// <param name="serviceProvider"></param>
        public void TestValidator(Dictionary<string, string> model,
            string validatorText, string dynamicResourceText, string baseValidatorText,
            IServiceProvider serviceProvider)
        {
            List<IDictionaryValidator> validators = new List<IDictionaryValidator>();

            lock (_cache)
            {
                var extValidatorTexts = (baseValidatorText != null) ? new string[] { validatorText } : new string[] {};
                baseValidatorText = baseValidatorText ?? validatorText;
                CreateValidators(baseValidatorText, extValidatorTexts, dynamicResourceText, false, validators);
            }

            var fields = validators.Cast<XamlValidator>()
                .SelectMany(x => x.Definitions)
                .Select(x => x.Value)
                .ToArray();

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
                        $"Validator is not valid. Property  with name {field} is not presented in model.");
                }
                var _ = model[field];
            }

            // Производим валидацию. Результат не интересует, важно, чтобы ничего не упало.

            var result = new ValidationContext
            {
                ServiceProvider = serviceProvider
            };

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
        protected ValidationContext ValidateInternal(ValidationParamObject paramObject, bool useCache, IServiceProvider serviceProvider)
        {
            if (paramObject.Model == null)
            {
                throw new ArgumentNullException(nameof(paramObject.Model));
            }

            if (string.IsNullOrEmpty(paramObject.Validator))
            {
                throw new ArgumentNullException(nameof(paramObject.Validator));
            }

            List<IDictionaryValidator> validators = new List<IDictionaryValidator>();

            lock (_cache)
            {
                CreateValidators(paramObject.Validator, paramObject.AggregatedValidatorList, paramObject.DynamicResource, useCache, validators);
            }

            var result = new ValidationContext
            {
                ServiceProvider = serviceProvider,
                CustomerCode = paramObject.CustomerCode,
                SiteId = paramObject.SiteId,
                ContentId = paramObject.ContentId
            };

            foreach (var validator in validators)
            {
                validator.Validate(paramObject.Model, result);
            }

            return result;
        }

        private void CreateValidators(string validatorText, string[] aggregatedValidatorList, string dynamicResourceText, bool useCache, List<IDictionaryValidator> validators)
        {
            if (!string.IsNullOrEmpty(dynamicResourceText))
            {
                _container = GetResourceDictionary(dynamicResourceText, useCache);
            }

            foreach (var aggregatedText in aggregatedValidatorList)
            {
                validatorText = MergeValidators(validatorText, aggregatedText);
            }

            validators.Add(GetValidator(validatorText, dynamicResourceText, useCache));
        }

        private string MergeValidators(string validatorText, string aggregatedText)
        {

            var result = validatorText;
            if (!String.IsNullOrEmpty(aggregatedText) && !string.IsNullOrEmpty(validatorText))
            {
                var x1 = XElement.Parse(validatorText);
                var x2 = XElement.Parse(aggregatedText);

                XNamespace ns = "http://artq.com/validation";
                var def1 = x1.Element(ns + "XamlValidator.Definitions");
                var def2 = x2.Element(ns + "XamlValidator.Definitions");

                if (def1 != null && def2 != null)
                {
                    var defToCopy = def2
                            .Elements(ns + "PropertyDefinition")
                            .Where(n => !new[] { "Id", "StatusTypeId" }.Contains(n.Attribute("Alias")?.Value))
                            .ToArray();

                    foreach (var d in defToCopy)
                    {
                        def1.Add(d);
                    }

                    var rest = x2.Elements().Where(n => n.Name != ns + "XamlValidator.Definitions").ToArray();

                    foreach (var r in rest)
                    {
                        x1.Add(r);
                    }

                    result = x1.ToString();
                }
            }

            return result;
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
            foreach (byte t in hash)
            {
                sb.Append(t.ToString("X2"));
            }
            return $"{prefix}_{sb}";
        }
        #endregion
    }
}
