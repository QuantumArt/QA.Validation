using System.Collections.Generic;

namespace QA.Validation.Xaml.Adapters
{
    public class DynamicAdapter
    {
        public dynamic AdaptDictionary(Dictionary<string, object> dict)
        {
            dynamic obj = dict.ToExpando();
            return obj;
        }       
    }
}
