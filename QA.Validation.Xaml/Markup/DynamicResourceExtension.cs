// Owners: Karlov Nikolay

using System;
using System.Collections.Generic;
using System.Diagnostics;
#if NET_STANDARD
using Portable.Xaml.Markup;
using Portable.Xaml;
#else
using System.Windows.Markup;
using System.Xaml;
#endif

namespace QA.Validation.Xaml.Markup
{
    /// <summary>
    /// Осуществляет поиск ресурса в ресурсоном словаре текущего или корневого элемента
    /// </summary>
    public class DynamicResourceExtension : MarkupExtension
    {

        /// <summary>
        /// Имя словаря
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ключ
        /// </summary>
        public string Key { get; set; }

        public DynamicResourceExtension() { }
        public DynamicResourceExtension(string name, string key) { Name = name; Key = key; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            object result = null;

            var rootObjectProvider = (IRootObjectProvider)serviceProvider.GetService(typeof(IRootObjectProvider));

            // если искомое значение находится в текуще графе объектов

            if (FindInCurrent(serviceProvider, Name, Key, out result))
            {
                return result;
            }

            // иначе ищем во внешнем словаре

            var dict = DynamicResourceDictionaryContainer.GetDictionary(Name);

            if (dict != null)
            {
                if (dict.HasResources && dict.TryGetResource(Key, out result))
                {
                    Debug.WriteLine("OK");
                }
                else
                {
                    throw new KeyNotFoundException(string.Format("An item with the given key '{0}' and name '{1}' is not found.", Key, Name));
                }
            }
            else
            {
                throw new KeyNotFoundException(string.Format("A dictionary with the given name '{0}' is not found.", Name));
            }

            return result;
        }

        private static bool FindInCurrent(IServiceProvider serviceProvider, string name, string key, out object value)
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
            XamlType xamlType = schemaContext.GetXamlType(typeof(IDynamicResourceContainer));
            XamlMember ambientMember = xamlType.GetMember("ResourceDictionaries");
            XamlType[] types = new XamlType[]
			{
				schemaContext.GetXamlType(typeof(Dictionary<string, DynamicResourceDictionary>))
			};

            IEnumerable<AmbientPropertyValue> allAmbientValues = ambientProvider.GetAllAmbientValues(null, false, types, new XamlMember[]
			{
                ambientMember,
			});

            foreach (var ambientPropertyValue in allAmbientValues)
            {
                if (ambientPropertyValue.Value is Dictionary<string, DynamicResourceDictionary>)
                {
                    Dictionary<string, DynamicResourceDictionary> resourceDictionary = (Dictionary<string, DynamicResourceDictionary>)ambientPropertyValue.Value;
                    DynamicResourceDictionary dynamicResourceDictionary = null;
                    if (resourceDictionary.TryGetValue(name, out dynamicResourceDictionary))
                    {
                        if (dynamicResourceDictionary.TryGetResource(key, out value))
                        {
                            return true;
                        }
                    }
                }
            }

            value = null;

            return false;
        }


    }
}
