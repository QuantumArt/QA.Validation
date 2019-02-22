using System;
using System.Collections.Generic;
using System.Linq.Dynamic2;
#if NET_STANDARD
using Portable.Xaml.Markup;
#else
using System.Windows.Markup;
#endif
using QA.Validation.Xaml.Dynamic;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Проверяет
    /// </summary>
    [ContentProperty("Expression")]
    public class SatisfyExpression : PropertyValidationCondition
    {
        private readonly object _sync = new object();
        private string _expression;
        private Func<DynamicContext, bool> _func;

        /// <summary>
        /// Выражение, которые будет выполняться. Выражение должно возвращать bool.
        /// Примеры:
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
                                .ParseLambda<DynamicContext, bool>(_expression)
                                .Compile();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Выполнение действия
        /// </summary>
        /// <param name="context">контекст</param>
        /// <returns></returns>
        public override bool Execute(ValidationConditionContext context)
        {
            var source = Source ?? context.Definition;

            var dict = new Dictionary<string, object>();

            foreach (var definition in context.All)
            {
                dict.Add(definition.Alias, context.ValueProvider.GetValue(definition));
            }

            var ctx = new DynamicContext
            {
                Model = dict,
                Source = source,
                Context = context,
                Value = context.ValueProvider.GetValue(source)
            };

            return _func(ctx);
        }
    }
}
