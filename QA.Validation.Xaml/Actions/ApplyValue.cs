using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace QA.Validation.Xaml
{
    [ContentProperty("Value")]
    public class ApplyValue : ValidationCondition
    {
        private bool _isValueSet;
        private object _value;

        public PropertyDefinition Source { get; set; }       
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _isValueSet = true;
                _value = value;
            }
        }

        public ApplyValue()
        {
            // не установлено значение
            _isValueSet = false;
        }

        public override bool Execute(ValidationConditionContext context)
        {
            var source = Source ?? context.Definition;

            if (source == null)
                throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ValidatorError, "Source in not provided.");

            if (_isValueSet)
            {
                object value = null;
                if (Value is IValueArgument)
                {
                    var argument = (IValueArgument)Value;
                    value = argument.Process(source, context.ValueProvider, context, null);
                }
                else
                {
                    value = Value;
                }

                context.ValueProvider.SetValue(source, value);
            }

            return true;
        }
    }
}
