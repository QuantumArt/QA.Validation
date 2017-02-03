using System;
using System.Collections.Generic;
using System.ComponentModel;
using QA.Validation.Xaml.TypeConverters;

namespace QA.Validation.Xaml.ListTypes
{
    [TypeConverter(typeof(ListOfStringTypeConverter))]
    [Serializable]
    public class ListOfString : List<string>
    {
        public ListOfString() { }
        public ListOfString(IEnumerable<string> items) : base(items) { }
    }

}
