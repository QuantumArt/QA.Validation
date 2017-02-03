using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Валидатор для типизированных моделей
    /// </summary>
    [ContentProperty("ValidationRules")]
    [DictionaryKeyProperty("Type")]
    public class XamlObjectValidator : XamlValidatorBase, IDefinitionStorage, IObjectValidator
    {
        private Dictionary<string, Func<object, object>> _propertiesInfo
            = new Dictionary<string, Func<object, object>>();

        public Type Type { get; set; }

        public void Validate(object obj, ValidationContext result)
        {
            base.Validate(new ReflectedValueProvider(_propertiesInfo, obj), result);
        }

        protected override void OnDefinitionAdded(PropertyDefinition definition)
        {
            var type = this.Type;
            var propertyInfo = type.GetProperty(definition.PropertyName,
                BindingFlags.Public | BindingFlags.Instance);

            var method = propertyInfo.GetGetMethod(false);

            ParameterExpression obj = Expression.Parameter(typeof(object), "obj");

            Expression<Func<object, object>> expr =
                Expression.Lambda<Func<object, object>>(
                    Expression.Convert(
                        Expression.Call(
                            Expression.Convert(obj, method.DeclaringType),
                            method),
                        typeof(object)),
                    obj);

            _propertiesInfo.Add(definition.PropertyName, expr.Compile());
        }

        class ReflectedValueProvider : IValueProvider
        {
            private Dictionary<string, Func<object, object>> _members;
            private object _obj;

            internal ReflectedValueProvider(Dictionary<string, Func<object, object>> members, object obj)
            {
                _members = members;
                _obj = obj;
            }

            public object GetValue(PropertyDefinition source)
            {
                return _members[source.PropertyName].Invoke(_obj);
            }

            #region IValueProvider Members
            public void SetValue(PropertyDefinition Source, object value)
            {
                throw new NotImplementedException();
            }
            #endregion
        }
    }
}