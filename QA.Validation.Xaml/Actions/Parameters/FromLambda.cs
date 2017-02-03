using System;
using System.Collections.Generic;
using System.Linq.Dynamic2;
using System.Windows.Markup;
using QA.Validation.Xaml.Dynamic;

namespace QA.Validation.Xaml
{
    [ContentProperty("Expression")]
    public class FromLambda : IValueArgument
    {
        private Func<DynamicContext, object> _func;
        private string _expression;
        private readonly object _sync = new object();

        /// <summary>
        /// Выражение, которые будет выполняться. Выражение должно возвращать bool.
        /// Примеры:
        /// Model["Name"] == "123"
        /// Int32(Value) mod 2 == 1  and Int32(Model["Age"]) > 10
        /// it.IsValid("Name") and Convert.ToDateTime(Value).Year > 2012
        /// </summary>
        public string Expression
        {
            get { return _expression; }
            set
            {
                if (_func == null)
                {
                    lock (_sync)
                    {
                        if (_func == null)
                        {
                            _expression = value;

                            _func = DynamicExpression
                                .ParseLambda<DynamicContext, object>(_expression)
                                .Compile();
                        }
                    }
                }
            }
        }
        #region IValueArgument Members

        public object Process(PropertyDefinition definition, IValueProvider provider, ValidationConditionContext context, object currentValue)
        {
            if (_expression == null)
                throw new XamlValidatorException();

            var dict = new Dictionary<string, object>();

            foreach (var def in context.All)
            {
                dict.Add(def.Alias, context.ValueProvider.GetValue(def));
            }

            var ctx = new DynamicContext
            {
                Model = dict,
                Source = definition,
                Context = context,
                Value = currentValue
            };

            return _func(ctx);
        }

        #endregion
    }
}
