using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
#if NET_STANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Валидатор для нетипизированных моделей
    /// </summary>
    [ContentProperty("ValidationRules")]
    [DictionaryKeyProperty("Type")]
    public class XamlValidator : XamlValidatorBase, IDefinitionStorage, IDictionaryValidator
    {
        public string Discriminator { get; set; }

        public void Validate(Dictionary<string, string> values, ValidationContext result)
        {
            base.Validate(new LookupValueProvider(values), result);
        }

        protected override void OnDefinitionAdded(PropertyDefinition definition)
        {
            if (definition.PropertyType == null)
            {
                throw new ArgumentNullException("definition.PropertyType");
            }

            if (definition.PropertyName == null)
            {
                throw new ArgumentNullException("definition.PropertyName");
            }
        }

        class LookupValueProvider : IValueProvider
        {
            private Dictionary<string, string> _members;

            internal LookupValueProvider(Dictionary<string, string> members)
            {
                _members = members;
            }

            public object GetValue(PropertyDefinition source)
            {
                if (source == null)
                    throw new ArgumentNullException("source", string.Format("Property 'source' is null. It seems that the key provided is incorrect."));

                var culture = CultureInfo.CurrentCulture;

                string value = null;
                try
                {
                    value = _members[source.PropertyName];
                }
                catch (KeyNotFoundException ex)
                {
                    throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ProvidedDataError,
                        string.Format("The validator definition is not correct. Field {0} is not found.", source.PropertyName), ex);
                }

                if (value == null || (value == string.Empty && source.PropertyType != typeof(string)))
                {
                    return null;
                }

                var converter = TypeDescriptor.GetConverter(source.PropertyType);

                try
                {
                    // добавим кастомную конвертацию 0 -> false, 1 -> true
                    if (source.PropertyType == typeof(Boolean) && (value == "0" || value == "1"))
                    {
                        return Convert.ToBoolean(Convert.ToInt16(value));
                    }

                    if (source.PropertyType == typeof(Double))
                    {
                        double res;
                        if (!Double.TryParse(value, NumberStyles.Float, culture, out res))
                        {
                            if (!Double.TryParse(value.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, out res))
                            {
                                return Convert.ToDouble(value, culture);
                            }
                            else
                            {
                                return res;
                            }
                        }
                        else
                        {
                            return res;
                        }
                    }

                    return converter.ConvertFromString(value);
                }
                catch (FormatException ex)
                {
                    throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ProvidedDataError,
                        string.Format("It's impossible to convert '{1}' to type {0}.", source.PropertyType, value), ex);
                }
                catch (NotSupportedException ex)
                {
                    throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ValidatorError,
                        string.Format("Converter for type {0} is missing.", source.PropertyType), ex);
                }
                catch (Exception ex)
                {
                    throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ProvidedDataError,
                        string.Format("Convertation failed. Check inner exception for details. Type: '{0}' target type: {1}, value: {2}.",
                            value == null ? "n/a" : value.GetType().ToString(),
                            source.PropertyType,
                            value),
                        ex);
                }
            }

            public void SetValue(PropertyDefinition source, object value)
            {

                if (!_members.ContainsKey(source.PropertyName))
                {
                    throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ProvidedDataError,
                        string.Format("It's impossible to set unknown property. The validator definition is not correct. Field {0} is not found.", source.PropertyName));
                }

                try
                {
                    var convertedValue = ConvertValueToString(source, value);
                    _members[source.PropertyName] = convertedValue;
                }
                catch (FormatException ex)
                {
                    throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ProvidedDataError,
                        string.Format("It's impossible to convert '{1}' to type {0}.", source.PropertyType, value), ex);
                }
                catch (NotSupportedException ex)
                {
                    throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ValidatorError,
                        string.Format("Converter for type {0} is missing.", source.PropertyType), ex);
                }
                catch (Exception ex)
                {
                    throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ProvidedDataError,
                        string.Format("Convertation failed. Check inner exception for details. Type: '{0}' target type: {1}, value: {2}.",
                            value == null ? "n/a" : value.GetType().ToString(),
                            source.PropertyType,
                            value),
                        ex);
                }
            }

            private static string ConvertValueToString(PropertyDefinition source, object value)
            {
                string convertedValue = null;

                if (value == null)
                {
                    convertedValue = string.Empty;
                }

                if (source.PropertyType == typeof(Boolean))
                {
                    bool v = Convert.ToBoolean(value);
                    convertedValue = v ? "1" : "0";

                    return convertedValue;
                }


                var converter = TypeDescriptor.GetConverter(source.PropertyType);

                convertedValue = converter.ConvertToInvariantString(value);

                return convertedValue;
            }
        }
    }
}
