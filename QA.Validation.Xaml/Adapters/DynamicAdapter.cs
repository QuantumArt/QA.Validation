using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

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
