using System;
using System.Collections.Generic;

namespace QA.Validation.Xaml.Core
{
    public class ConfigurationProvider : IServiceProvider
    {
        IDictionary<Type, object> _services;
        public ConfigurationProvider(IDictionary<Type, object> services)
        {
            _services = services;
        }

        public ConfigurationProvider()
            : this(new Dictionary<Type, object>()) { }

        public object this[Type key]
        {
            get { return _services[key]; }
            set { _services[key] = value; }
        }

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            object result;
            if (!_services.TryGetValue(serviceType, out result))
                return null;

            return result;
        }

        #endregion
    }
}
