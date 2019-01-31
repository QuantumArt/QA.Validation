using System;
using System.Text.RegularExpressions;
#if NET_STANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif

namespace QA.Validation.Xaml.Extensions.Conditions
{
    using QA.Validation.Xaml;

    /// <summary>
    /// Проверка корректности адреса Url
    /// </summary>
    [ContentProperty("Value")]
    public class IsAbsoluteUrl : PropertyValidationCondition
    {
        static Regex regex = new Regex(@"^htt(p|ps)://(([a-z|A-Z|0-9|-|_]+).+)\.[a-z]{1,5}(|\?.+)$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public override bool Execute(ValidationConditionContext context)
        {
            var value1 = context.ValueProvider.GetValue(Source ?? context.Definition);

            if (value1 is string)
            {
                return regex.IsMatch((string)value1);
            }

            throw GetNotSupportedException(value1);
        }
    }

    /// <summary>
    /// Параметры проверки
    /// </summary>
    [Flags]
    public enum UrlOptions
    {
        Absolute = 0x0001,
        AllowQuery = 0x0002,
        Http = 0x0004 | Absolute,
        Https = 0x0008 | Absolute,
        Full = Http | Https | AllowQuery | Absolute
    }
}
