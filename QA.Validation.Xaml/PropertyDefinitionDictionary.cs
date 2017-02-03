using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QA.Validation.Xaml
{
    public class PropertyDefinitionDictionary : IDictionary<string, PropertyDefinition>
    {
        IDictionary<string, PropertyDefinition> _innerDictionary = new Dictionary<string, PropertyDefinition>();
        private IDefinitionStorage _validator;
        static Regex AliasPattern = new Regex(@"^[a-z|A-Z|_][0-9|_|a-z|A-Z]+$", RegexOptions.Compiled);

        public PropertyDefinitionDictionary(Object parentNode)
        {
            if (parentNode is IDefinitionStorage)
            {
                _validator = (IDefinitionStorage)parentNode;
            }
        }

        public void Add(string key, PropertyDefinition value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (string.IsNullOrEmpty(value.Alias))
            {
                throw new ArgumentNullException("value.Alias");
            }

            if (string.IsNullOrEmpty(value.PropertyName))
            {
                throw new ArgumentNullException("value.PropertyName");
            }

            if (value.PropertyType == null)
            {
                throw new ArgumentNullException("value.PropertyType");
            }

            if (!AliasPattern.IsMatch(value.Alias))
            {
                throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ValidatorError, 
                    "Alias may contain only characters in set: 'a-z|A-Z|0-9|_'.");
            }

            if (_validator != null)
            {
                _validator.OnDefinitionAdded(value);
            }

            _innerDictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _innerDictionary.Keys; }
        }

        public bool TryGetValue(string key, out PropertyDefinition value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        public ICollection<PropertyDefinition> Values
        {
            get { return _innerDictionary.Values; }
        }

        public PropertyDefinition this[string key]
        {
            get
            {
                return _innerDictionary[key];
            }
            set
            {
                _innerDictionary[key] = value;
            }
        }

        public int Count
        {
            get { return _innerDictionary.Count; }
        }


        public bool Remove(string key)
        {
            throw new NotSupportedException("This feature is not supported.");
        }

        public void Add(KeyValuePair<string, PropertyDefinition> item)
        {
            _innerDictionary.Add(item);
        }

        public void Clear()
        {
            throw new NotSupportedException("This feature is not supported.");
        }

        public bool Contains(KeyValuePair<string, PropertyDefinition> item)
        {
            return _innerDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, PropertyDefinition>[] array, int arrayIndex)
        {
            throw new NotSupportedException("This feature is not supported.");
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, PropertyDefinition> item)
        {
            throw new NotSupportedException("This feature is not supported.");
        }

        public IEnumerator<KeyValuePair<string, PropertyDefinition>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

    }
}
