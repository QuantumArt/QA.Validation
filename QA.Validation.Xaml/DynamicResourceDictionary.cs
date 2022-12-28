using Portable.Xaml.Markup;
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
