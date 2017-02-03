using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QA.Validation.Xaml
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class ValidationPluginAttribute : Attribute
    {
        public ValidationPluginAttribute()
        {

        }
    }
}
