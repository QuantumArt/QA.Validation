using System.Linq;
using System.Text.RegularExpressions;
using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Транслитирирование текстового значения
    /// </summary>
    public class ReplaceText : TextActionParameter
    {
        public Replacement Replacement { get; set; }

        public ReplaceText()
        {
        }

        protected override object OnProcess(PropertyDefinition definition, IValueProvider provider, ValidationConditionContext context, string receivedValue)
        {
            if (Replacement == null)
                throw new XamlValidatorException("Replacement is not specified.");

            if (string.IsNullOrWhiteSpace(Replacement.Expression))
                throw new XamlValidatorException("Pattern is not specified.");

            if (receivedValue == null)
                return null;

            return Replace(definition, provider, context, receivedValue);
        }

        private string Replace(PropertyDefinition definition, IValueProvider provider, ValidationConditionContext context, string text)
        {
            string valueToUse = null;
            string result = text;

            var replacement = Replacement;
            if (replacement.Value is IValueArgument)
            {
                var value = ((IValueArgument)replacement.Value).Process(definition, provider, null, text);
                valueToUse = value == null ? (string.Empty) : value.ToString();
            }
            else
            {
                valueToUse = replacement.Value == null ? string.Empty : replacement.Value.ToString();
            }

            var regex = new Regex(replacement.Expression, replacement.RegexOptions);

            if (!replacement.IsNegative)
            {
                result = regex.Replace(text, valueToUse);
            }
            else
            {
                result = string.Join("",
                    regex.Matches(text)
                        .Cast<Match>()
                        .Where(x => x.Success)
                        .OrderBy(x => x.Index)
                        .Select(x => x.Value));
            }

            return result;
        }
    }

    [ContentProperty("Expression")]
    public class Replacement
    {
        internal bool IsValueSet = false;
        private object _value;

        public string Expression { get; set; }
        public RegexOptions RegexOptions { get; set; }
        public bool IsNegative { get; set; }

        public object Value
        {

            get { return _value; }
            set
            {
                IsValueSet = true;
                _value = value;
            }
        }
    }
}
