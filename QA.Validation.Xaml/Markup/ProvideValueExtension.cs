// Owners: Karlov Nikolay

using System;
using System.Collections.Generic;
using Portable.Xaml;
using Portable.Xaml.Markup;
using QA.Validation.Xaml.Core;

namespace QA.Validation.Xaml.Markup
{
    /// <summary>
    /// Осуществляет поиск ресурса в ресурсоном словаре текущего или корневого элемента
    /// </summary>
    public class ProvideValueExtension : MarkupExtension
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public PropertyDefinition Definition { get; set; }

        public ProvideValueExtension() { }
        public ProvideValueExtension(PropertyDefinition definition) { Definition = definition; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            object result = null;

            var definition = Definition;

            if (definition == null)
            {
                var rule = FindNode<IMemberValidationRule, XamlValidatorBase>(serviceProvider, "ValidationRules");

                definition = rule.Definition;
            }

            var valueProvider = (IValueProvider)FindNode<XamlValidatorBase, XamlValidatorBase>(serviceProvider, "");

            result =  ValueProvider.Provide(valueProvider, definition);

            return result;
        }
               
        private static T FindNode<T, TParent>(IServiceProvider serviceProvider, string propertyName)
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
    }
}
