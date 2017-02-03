
namespace QA.Validation.Xaml
{
    /// <summary>
    /// Аргумент условия валидации
    /// </summary>
    public interface IValueArgument
    {
        /// <summary>
        /// Получене значения
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="provider"></param>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        object Process(PropertyDefinition definition, IValueProvider provider, ValidationConditionContext context, object currentValue);
    }
}
