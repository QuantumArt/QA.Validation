using System.Collections.Generic;
using System.Reflection;
using QA.Configuration;
using System.IO;

namespace QA.Validation.Xaml.Tests.Util
{
    public static class ValidationHelper
    {
        public static TValidator GetXaml<TValidator>(string path)
        {
            using (var stream = Assembly.GetExecutingAssembly()
               .GetManifestResourceStream(path))
            {
                // создаем экземпляр валидатора
                return (TValidator)XamlConfigurationParser.CreateFrom(stream);
            }
        }

        public static Dictionary<string, object> ConvertToDictionary<T>(T item)
        {
            var result = new Dictionary<string, object>();
            var type = typeof(T);

            foreach (var propertyInfo in type.GetProperties(BindingFlags.Public|BindingFlags.Instance))
            {
                result.Add(propertyInfo.Name, propertyInfo.GetValue(item, new object[] { }));
            }

            return result;
        }

        public static string GetEmbeddedResourceText(string path)
        {            
            using (var stream = Assembly.GetExecutingAssembly()
               .GetManifestResourceStream(path))
            {
                using (var textReader = new StreamReader(stream))
                {
                    return textReader.ReadToEnd();
                }
            }
        }
    }
}
