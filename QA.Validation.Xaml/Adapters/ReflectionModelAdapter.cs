using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace QA.Validation.Xaml.Adapters
{
    /// <summary>
    /// Преобразование модели, основанное на reflection (c кешированием)
    /// </summary>
    public class ReflectionModelAdapter<TModel> : IModelAdapter
    {
        private Dictionary<string, Delegate> _properties;
        delegate dynamic ModelDelegate(TModel model);
        public ReflectionModelAdapter()
        {
            var type = typeof(TModel);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _properties = new Dictionary<string, Delegate>();

            foreach (var propertyInfo in properties)
            {
                var method = propertyInfo.GetGetMethod(false);

                ParameterExpression obj = Expression.Parameter(typeof(object), "obj");

                Expression<Func<TModel, object>> expr =
                    Expression.Lambda<Func<TModel, object>>(
                        Expression.Convert(
                            Expression.Call(
                                Expression.Convert(obj, method.DeclaringType),
                                method),
                            typeof(object)),
                        obj);

                _properties.Add(propertyInfo.Name, expr.Compile());
            }

        }

        #region IModelAdapter Members

        public Dictionary<string, string> AdaptModel<TModel>(TModel item)
        {
            var result = new Dictionary<string, string>();
            var type = typeof(TModel);

            foreach (var method in _properties)
            {
                var value = ((Func<TModel, object>)method.Value).Invoke(item);//(propertyInfo.Value).Method.Invoke(item, new object[]{});

                string converted = (value == null) ? null : value.ToString();

                result.Add(method.Key, converted);
            }

            return result;
        }

        #endregion
    }

    /// <summary>
    /// Преобразование модели, основанное на reflection  (без кеширования)
    /// </summary>
    public class ReflectionModelAdapter : IModelAdapter
    {
        #region IModelAdapter Members

        public Dictionary<string, string> AdaptModel<TModel>(TModel item)
        {
            var result = new Dictionary<string, string>();
            var type = typeof(TModel);

            foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = propertyInfo.GetValue(item, new object[] { });

                string converted = (value == null) ? null : value.ToString();

                TypeConverter converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);

                if (converter.CanConvertTo(typeof(string)))
                {
                    converted = converter.ConvertToInvariantString(value);
                }

                result.Add(propertyInfo.Name, converted);
            }

            return result;
        }

        #endregion
    }
}
