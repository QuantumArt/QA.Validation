namespace QA.Validation.Xaml
{
    public class ExtractValue : IValueArgument
    {
        public PropertyDefinition Source { get; set; }
        #region IValueArgument Members

        public object Process(PropertyDefinition definition, IValueProvider provider, ValidationConditionContext context, object currentValue)
        {
            object result = null;

            var source = Source ?? definition;

            if (source == null)
                throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ValidatorError, "The value cannot be extracted. Source is not provided.");

            result = provider.GetValue(source);

            return result;
        }

        #endregion
    }
}
