using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Предоставляет доступ к значениям полей
    /// </summary>
    public static class ValueProvider
    {
        public static object Provide(IValueProvider provider, PropertyDefinition definition)
        {
            return provider.GetValue(definition);
        }

        public static object ProvideAsString(IValueProvider provider, PropertyDefinition definition)
        {
            var value = Provide(provider, definition);
            return value == null ? null : value.ToString();
        }

        public static object GetDefinitionProperty(PropertyDefinition definition, string propertyName)
        {
            if (definition == null)
                throw new NullReferenceException("definition");

            if (propertyName == null)
                throw new NullReferenceException("propertyName");

            var type = typeof(PropertyDefinition);

            var propInfo = type.GetProperty(propertyName, BindingFlags.Public);

            if (propInfo == null)
            {
                throw new XamlValidatorException(string.Format("Property {0} is not found.", propertyName));
            }

            return propInfo.GetValue(definition, new object[] { });
        }
    }
}
