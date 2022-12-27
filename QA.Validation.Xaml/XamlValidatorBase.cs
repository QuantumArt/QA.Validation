using System.Linq;
using System.Collections.Generic;
using Portable.Xaml.Markup;
using QA.Configuration;

namespace QA.Validation.Xaml
{
    [ContentProperty("ValidationRules")]
    [DictionaryKeyProperty("Type")]
    public abstract class XamlValidatorBase : ConfigurableItem, IDefinitionStorage
    {
        /// <summary>
        /// Правила валидации
        /// </summary>
        [Ambient]
        public IList<IValidationRule> ValidationRules { get; private set; }

        /// <summary>
        /// Описания полейы
        /// </summary>
        [Ambient]
        public PropertyDefinitionDictionary Definitions { get; private set; }

        public XamlValidatorBase()
        {
            ValidationRules = new List<IValidationRule>();
            Definitions = new PropertyDefinitionDictionary(this);
        }

        protected virtual void Validate(IValueProvider provider, ValidationContext result)
        {
            foreach (var rule in ValidationRules)
            {
                rule.Validate(provider, (IDefinitionStorage)this, result);
            }
        }

        protected abstract void OnDefinitionAdded(PropertyDefinition definition);

        #region IDefinitionStorage Members

        bool IDefinitionStorage.TryGetDefinition(string key, out PropertyDefinition definition)
        {
            return Definitions.TryGetValue(key, out definition);
        }

        void IDefinitionStorage.OnDefinitionAdded(PropertyDefinition definition)
        {
            OnDefinitionAdded(definition);
        }

        PropertyDefinition[] IDefinitionStorage.GetAll()
        {
           return this.Definitions.Values
               .Select(x => x.Copy())
               .ToArray();
        }

        #endregion
    }
}
