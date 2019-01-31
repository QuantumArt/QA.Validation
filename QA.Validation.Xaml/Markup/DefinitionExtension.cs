// Owners: Karlov Nikolay

using System;
using System.Diagnostics;
#if NETSTANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif

namespace QA.Validation.Xaml.Markup
{
    /// <summary>
    /// Осуществляет поиск ресурса в ресурсоном словаре текущего или корневого элемента
    /// </summary>
    [ContentProperty("Key")]
    public class DefinitionExtension : MarkupExtension
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public string Key { get; set; }

        public DefinitionExtension() { }
        public DefinitionExtension(string key) { Key = key; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            PropertyDefinition result = null;
            try
            {
                if (result == null)
                {
                    IDefinitionStorage root = XamlMembersHelper.FindRoot<IDefinitionStorage>(serviceProvider);
                    root.TryGetDefinition(Key, out result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }
    }
}
