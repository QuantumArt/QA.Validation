using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using QA.Validation.Xaml.Extensions.Rules.Remote;

namespace QA.Validation.Xaml.Extensions.Rules
{
    /// <summary>
    /// Контекст, передаваемый обработчику удаленной валидации
    /// </summary>
    [Serializable]
    public class RemoteValidationContext
    {
        public string CustomerCode { get; set; }

        public int SiteId { get; set; }

        public int ContentId { get; set; }

        public Dictionary<string, object> Values { get; set; }

        public List<RemotePropertyDefinition> Definitions { get; set; }

        public RemoteValidationContext()
        {
            Values = new Dictionary<string, object>();
            Definitions = new List<RemotePropertyDefinition>();
        }

        public static string GetJson(RemoteValidationContext model)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Serialize(model);
        }

        /// <summary>
        /// Получение значения свойства по определению с конвертацией типа
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        public object ProvideValueExact(RemotePropertyDefinition definition)
        {
            var targetType = Type.GetType(definition.PropertyType);
            var converter = TypeDescriptor.GetConverter(targetType);
            var value = Values[definition.PropertyName];

            if (value == null)
            {
                return null;
            }

            var type = value.GetType();

            if (type == targetType)
            {
                if (type == typeof(DateTime))
                {
                    // http://blog.devarchive.net/2008/02/serializing-datetime-values-using.html
                    return ((DateTime)value).ToLocalTime();
                }

                return value;
            }

            if (converter.CanConvertFrom(type))
            {
                return converter.ConvertFrom(value);
            }
            else
            {
                converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertTo(targetType))
                {
                    return converter.ConvertTo(value, targetType);
                }
            }

            throw new NotSupportedException(string.Format("Cannot convert {0} to {1}.", type, targetType));
        }

        /// <summary>
        /// Установка значения свойства
        /// </summary>
        /// <param name="definition">описание</param>
        /// /// <param name="value">значение</param>
        /// <returns></returns>
        public void SetValue(RemoteValidationResult result, RemotePropertyDefinition definition, object value)
        {
            result.NewValues[definition.PropertyName] = value;
        }

        /// <summary>
        /// Установка значения свойства
        /// </summary>
        /// <param name="propertyName">имя свойства</param>
        /// <param name="value">значение</param>
        public void SetValue(RemoteValidationResult result, string propertyName, object value)
        {
            RemotePropertyDefinition definition = FindDefinition(propertyName);

            SetValue(result, FindDefinition(propertyName), value);
        }

        /// <summary>
        /// Получение значения свойства по определению с конвертацией типа приведением
        /// </summary>
        public T ProvideValueExact<T>(RemotePropertyDefinition definition)
        {
            var value = ProvideValueExact(definition);
            return value == null ? default(T) : (T)value;
        }

        /// <summary>
        /// Получение значения свойства по имени с конвертацией типа и приведением
        /// </summary>
        public T ProvideValueExact<T>(string propertyName)
        {
            RemotePropertyDefinition definition = FindDefinition(propertyName);

            return ProvideValueExact<T>(definition);
        }

        private RemotePropertyDefinition FindDefinition(string propertyName)
        {
            var definition = Definitions.FirstOrDefault(x => x.PropertyName == propertyName);
            if (definition == null)
            {
                throw new KeyNotFoundException("Definition with the given key is not found.");
            }

            return definition;
        }

        /// <summary>
        /// Название текущей языковой культуры
        /// </summary>
        public string CurrentUICulture { get; set; }

        /// <summary>
        /// Название текущей языковой культуры
        /// </summary>
        public string CurrentCulture { get; set; }
    }
}
