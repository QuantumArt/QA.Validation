using System;
using System.Collections.Generic;
using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Внешний подключаемый ресурсный словарь
    /// </summary>
    [ContentProperty("ResourceDictionaries")]
    public class DynamicResourceDictionaryContainer : IDynamicResourceContainer
    {
        private static Func<DynamicResourceDictionaryContainer> _provider;
        public Dictionary<string, DynamicResourceDictionary> ResourceDictionaries { get; private set; }

        public DynamicResourceDictionaryContainer()
        {
            ResourceDictionaries = new Dictionary<string, DynamicResourceDictionary>();
        }

        public bool TryGetResourceDictionary(string name, out DynamicResourceDictionary dictionary)
        {
            return ResourceDictionaries.TryGetValue(name, out dictionary);
        }

        internal static DynamicResourceDictionary GetDictionary(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            DynamicResourceDictionary result = null;

            if (_provider == null)
            {
                throw new System.InvalidOperationException("_provider is null.");
            }

            var instance = _provider();

            if (instance.TryGetResourceDictionary(name, out result))
            {
                return result;
            }

            return null;
        }

        public static void SetResourceProvider(Func<DynamicResourceDictionaryContainer> dynamicResourceProvider)
        {
            _provider = dynamicResourceProvider;
        }
    }
}
