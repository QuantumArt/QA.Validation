using System.Collections.Generic;
using System.Linq;
using System.Web;
using QA.Validation.Xaml.Extensions.ValueArguments;

namespace QA.Validation.Xaml.Extensions.ValueArguments
{
    /// <summary>
    /// Экранирование текста
    /// </summary>
    public class Encode : TextActionParameter
    {
        /// <summary>
        /// Тип экранирования строки
        /// </summary>
        public EncodingType EncodingType { get; set; }

        protected override object OnProcess(PropertyDefinition definition, IValueProvider provider, ValidationConditionContext context, string receivedValue)
        {
            switch (EncodingType)
            {
                case EncodingType.None:
                    break;

                case EncodingType.UrlEncode:
                    return HttpUtility.UrlEncode(receivedValue);
                case EncodingType.UrlPathEncode:
                    return HttpUtility.UrlPathEncode(receivedValue);
                case EncodingType.UrlDecode:
                    return HttpUtility.UrlDecode(receivedValue);
                case EncodingType.JsEncode:
                    return HttpUtility.JavaScriptStringEncode(receivedValue);                    
                case EncodingType.HtmlEncode:
                    return HttpUtility.HtmlEncode(receivedValue);
                case EncodingType.HtmlDecode:
                    return HttpUtility.HtmlDecode(receivedValue);
                case EncodingType.HtmlAttributeEncode:
                    return HttpUtility.HtmlAttributeEncode(receivedValue);

                default:
                    break;
            }

            return receivedValue;
        }
    }
}
