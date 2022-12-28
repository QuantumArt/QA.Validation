using System;

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
