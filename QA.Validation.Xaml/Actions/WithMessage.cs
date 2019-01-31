using System.ComponentModel;
#if NETSTANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif
using QA.Validation.Xaml.TypeConverters;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Добавление сообщения об ошибке.
    /// </summary>
    [ContentProperty("Text")]
    [TypeConverter(typeof(WithMessageTypeConverter))]
    public class WithMessage : ValidationCondition
    {
        /// <summary>
        /// Текст ошибки, который устанавливается, если данное действие вызвано.
        /// </summary>
        public string Text { get; set; }

        public override bool Execute(ValidationConditionContext context)
        {
            if (context.Definition.PropertyType != null)
            {
                context.Result.AddError(context.Definition, GetText());
            }
            else
            {
                context.Messages.Add(GetText());
            }

            return true;
        }

        /// <summary>
        /// Получение сообщения
        /// </summary>
        /// <returns></returns>
        protected virtual string GetText()
        {
            return Text;
        }
    }
}
