using System;
using System.Collections.Generic;
using System.ComponentModel;
using QA.Validation.Xaml.TypeConverters;

namespace QA.Validation.Xaml.ListTypes
{
    [TypeConverter(typeof(ListOfIntTypeConverter))]
    [Serializable]
    public class ListOfInt : List<int>
    {
        public ListOfInt() { }
        public ListOfInt(IEnumerable<int> items) : base(items) { }
    }
}
