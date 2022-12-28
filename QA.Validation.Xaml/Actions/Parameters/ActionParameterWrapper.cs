using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    [ContentProperty("Child")]
    public abstract class ActionParameterWrapper : IValueArgument
    {
        public IValueArgument Child { get; set; }
        #region IValueArgument Members

        public object Process(PropertyDefinition definition, IValueProvider provider, ValidationConditionContext context, object currentValue)
        {
            var childValue = Child.Process(definition, provider, context, currentValue);
            return OnProcess(definition, provider, context, childValue);
        }

        protected abstract object OnProcess(PropertyDefinition definition, IValueProvider provider, ValidationConditionContext context, object receivedValue);

        #endregion
    }
}
