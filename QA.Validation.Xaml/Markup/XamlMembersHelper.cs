using System;
using System.Collections.Generic;
using Portable.Xaml;

namespace QA.Validation.Xaml.Markup
{
    /// <summary>
    /// Поиск элементов графа объектов Xaml (используется в расширениях разметки)
    /// </summary>
    public static class XamlMembersHelper
    {
        /// <summary>
        /// Осуществляет поиск элемента графа объектов, помеченного атрибутом AmbientAttribute, по заданным критериям
        /// </summary>
        /// <typeparam name="T">Тип свойства</typeparam>
        /// <typeparam name="TParent">Тип объекта, содержащего свойство</typeparam>
        /// <param name="serviceProvider">Xaml ServiceProvider</param>
        /// <param name="propertyName">Имя свойства (null в случае поиска по )</param>
        /// <returns></returns>
        public static T FindNode<T, TParent>(IServiceProvider serviceProvider, string propertyName)
        {
            var xamlSchemaContextProvider = (IXamlSchemaContextProvider)serviceProvider.GetService(typeof(IXamlSchemaContextProvider));

            if (xamlSchemaContextProvider == null)
            {
                throw new InvalidOperationException(string.Format("The service {0} cannot be resolved.", typeof(IXamlSchemaContextProvider)));
            }

            var ambientProvider = (IAmbientProvider)serviceProvider.GetService(typeof(IAmbientProvider));

            if (ambientProvider == null)
            {
                throw new InvalidOperationException(string.Format("The service {0} cannot be resolved.", typeof(IAmbientProvider)));
            }

            XamlSchemaContext schemaContext = xamlSchemaContextProvider.SchemaContext;
            XamlType xamlType = schemaContext.GetXamlType(typeof(TParent));
            XamlMember[] members = new XamlMember[] { };

            if (!string.IsNullOrEmpty(propertyName))
            {
                XamlMember ambientMember = xamlType.GetMember(propertyName);
                members = new XamlMember[]
			    {
                    ambientMember,
			    };

            }
            XamlType[] types = new XamlType[]
			{
				schemaContext.GetXamlType(typeof(T))
            };

            IEnumerable<AmbientPropertyValue> allAmbientValues = ambientProvider.GetAllAmbientValues(null, false, types, members);

            List<AmbientPropertyValue> list = allAmbientValues as List<AmbientPropertyValue>;

            for (int i = 0; i < list.Count; i++)
            {
                AmbientPropertyValue ambientPropertyValue = list[i];
                if (ambientPropertyValue.Value is T)
                {
                    T resourceDictionary = (T)ambientPropertyValue.Value;
                    return resourceDictionary;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Поиск корневого элемента графа объектов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static T FindRoot<T>(IServiceProvider serviceProvider)
        {
            var rootProvider = (IRootObjectProvider)serviceProvider.GetService(typeof(IRootObjectProvider));

            if (rootProvider == null)
            {
                throw new ArgumentException("IRootObjectProvider");
            }

            return (T)rootProvider.RootObject;
        }
    }
}
