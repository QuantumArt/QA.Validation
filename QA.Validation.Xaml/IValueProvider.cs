
namespace QA.Validation.Xaml
{
    public interface IValueProvider
    {
        /// <summary>
        /// Получить значение
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        object GetValue(PropertyDefinition source);

        /// <summary>
        /// Установить значение
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        void SetValue(PropertyDefinition source, object value);
    }
}
