namespace QA.Validation.Xaml
{
    /// <summary>
    /// Расширения
    /// </summary>
    public static class ValidationContextExtensions
    {
        /// <summary>
        /// Добавление ошибки для поля с установкой IsValid = false
        /// </summary>
        /// <param name="context">контекст</param>
        /// <param name="propertyName">имя свойства</param>
        /// <param name="message">сообщение</param>
        /// <returns>context</returns>
        public static T AddModelError<T>(this T context, string propertyName, string message)
            where T: ValidationContextBase
        {
            context.Result.AddError(propertyName, message);
            return context;
        }

        /// <summary>
        /// Добавление текста ошибки с установкой IsValid = false
        /// </summary>
        /// <param name="context">контекст</param>
        /// <param name="message">сообщение</param>
        /// <returns>context</returns>
        public static T AddErrorMessage<T>(this T context, string message)
              where T : ValidationContextBase
        {
            context.Messages.Add(message);
            return context;
        }
    }
}
