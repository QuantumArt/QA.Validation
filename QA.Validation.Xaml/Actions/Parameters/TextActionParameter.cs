
namespace QA.Validation.Xaml
{
    public abstract class TextActionParameter : ActionParameterWrapper
    {
        protected override object OnProcess(PropertyDefinition definition, IValueProvider provider, ValidationConditionContext context, object currentValue)
        {
            if (currentValue == null)
                return null;

            return OnProcess(definition, provider, context, currentValue.ToString());
        }

        protected abstract object OnProcess(PropertyDefinition definition, IValueProvider provider, ValidationConditionContext context, string receivedValue);

    }
}
