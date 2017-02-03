using System.Collections.Generic;
using System.Globalization;
using System.Windows.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Добавление локализованного сообщения об ошибке валидации
    /// Текущая культура берется из Thread.CurrentThread.CurrentUICulture
    /// </summary>
    [ContentProperty("Localized")]
    public class WithLocalizedMessage : WithMessage
    {
        public Dictionary<string, string> Localized { get; set; }

        public WithLocalizedMessage()
            : base()
        {
            Localized = new Dictionary<string, string>();
        }

        protected override string GetText()
        {
            var culture = CultureInfo.CurrentUICulture.ToString().ToLower();

            if (Localized.ContainsKey(culture))
            {
                return Localized[culture];
            }

            // Если не найдено локализованное сообщение, берем стандартное (WithMessage.Text)
            return base.GetText();
        }
    }
}
