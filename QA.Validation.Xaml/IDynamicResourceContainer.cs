// Owners: Karlov Nikolay

using System.Collections.Generic;
#if NET_STANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Интерфейс класса с ресурсным словарем, конфигурируемого с помощью Xaml
    /// </summary>
    public interface IDynamicResourceContainer
    {
        [Ambient]
        Dictionary<string, DynamicResourceDictionary> ResourceDictionaries { get; }
        bool TryGetResourceDictionary(string name, out DynamicResourceDictionary value);
    }
}
