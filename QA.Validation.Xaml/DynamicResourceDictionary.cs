#if NETSTANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif

using QA.Configuration;

namespace QA.Validation.Xaml
{
    [DictionaryKeyProperty("Name")]
    [ContentProperty("Resources")]
    public class DynamicResourceDictionary : ConfigurableItem
    {
        public DynamicResourceDictionary()
            : base()
        {
        }

        public string Name { get; set; }
    }
}
