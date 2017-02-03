using System;
using System.Diagnostics;
using System.Windows.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Класс описания одного поля модели. Включает ключ, название, тип и комментарий
    /// </summary>
    [DictionaryKeyProperty("Alias")]
    [RuntimeNameProperty("Alias")]
    [UsableDuringInitialization(true)]
    [DebuggerDisplay("{Alias}-{PropertyName}")]
    [Serializable]
    public class PropertyDefinition : IEquatable<PropertyDefinition>, ICloneable
    {
        public PropertyDefinition() : this(null, null, null) { }

        public PropertyDefinition(string alias, string propertyName, Type propertyType) 
        {
            Alias = alias;
            PropertyName = propertyName;
            PropertyType = propertyType;
        }

        /// <summary>
        /// Тип свойства
        /// </summary>
        public Type PropertyType
        {
            get;
            set;
        }

        /// <summary>
        /// Название свойства.
        /// По названию осуществляется поиск значения при  валидации
        /// </summary>
        public string PropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// Уникальное дружественное имя данного поля.
        /// Используется в качестве ключа в Xaml при применении DefinitionMarkupExtension
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        /// <summary>
        /// Текстовый комментарий.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        #region Equality members
        public bool Equals(PropertyDefinition other)
        {
            if (object.ReferenceEquals(this, other))
                return true;

            if (other != null)
            {
                return (this.PropertyName == other.PropertyName &&
                    this.Alias == other.Alias &&
                    this.PropertyType == other.PropertyType);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (obj is PropertyDefinition)
            {
                return Equals((PropertyDefinition)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (PropertyName.GetHashCode()) ^ (Alias.GetHashCode() * 3) ^ (PropertyType.GetHashCode() * 7);
        }
        #endregion

        #region ICloneable Members

        public object Clone()
        {
            var obj = (PropertyDefinition)MemberwiseClone();
            obj.PropertyType = this.PropertyType;
            return obj;
        }

        #endregion

        /// <summary>
        /// Копирует экземпляр данного типа
        /// </summary>
        public PropertyDefinition Copy()
        {
            return (PropertyDefinition)this.Clone();
        }
    }
}
