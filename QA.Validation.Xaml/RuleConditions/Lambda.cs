//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Windows.Markup;
//using QA.Validation.Xaml.Dynamic;
//using System.Collections.Concurrent;


//namespace QA.Validation.Xaml
//{
//    using System.Linq.Dynamic2;
//    using Expr = System.Linq.Expressions.Expression;

//    /// <summary>
//    /// Проверяет 
//    /// </summary>
//    [ContentProperty("Expression")]
//    public class Lambda : PropertyValidationCondition
//    {
//        private readonly object _sync = new object();
//        private string _expression;

//        /// <summary>
//        /// Кеш делегатов
//        /// </summary>
//        ConcurrentDictionary<string, Delegate> _delegates = new ConcurrentDictionary<string, Delegate>();

//        /// <summary>
//        /// Выражение, которые будет выполняться. Выражение должно возвращать bool.
//        /// Примеры:
//        /// </summary>
//        public string Expression
//        {
//            get { return _expression; }
//            set { _expression = value; }
//        }

//        /// <summary>
//        /// Выполнение действия
//        /// </summary>
//        /// <param name="context">контекст</param>
//        /// <returns></returns>
//        public override bool Execute(ValidationConditionContext context)
//        {
//            var source = Source ?? context.Definition;

//            var dict = new Dictionary<string, object>();

//            var hash = source.GetHashCode() + string.Join("->", context.All.Select(x => x.GetHashCode()));

//            var lambda = _delegates.GetOrAdd(hash, key => CompileExpression(context));

//            foreach (var definition in context.All)
//            {
//                dict.Add(definition.Alias, context.ValueProvider.GetValue(definition));
//            }

//            var ctx = new DynamicContext
//            {
//                Model = dict,
//                Source = source,
//                Context = context,
//                Value = context.ValueProvider.GetValue(source)
//            };

//            return (bool)lambda.DynamicInvoke(ctx, dict.Values.ToArray());
//        }

//        private Delegate CompileExpression(ValidationConditionContext context)
//        {
//            var parameters = new List<ParameterExpression>();
//            var objParameters = new List<ParameterExpression>();
//            var converters = new List<Expression>();
            
//            var ctx = Expr.Parameter(typeof(DynamicContext), "ctx");
//            converters.Add(Expr.Convert(ctx, typeof(DynamicContext)));
//            parameters.Add(ctx);

//            foreach (var definition in context.All)
//            {
//                var param = Expr.Parameter(definition.PropertyType, definition.Alias);
//                parameters.Add(param);
//                converters.Add(Expr.Convert(param, definition.PropertyType));
//            }

//            LambdaExpression e = DynamicExpression.ParseLambda(parameters.ToArray(), typeof(bool), Expression);

//            var wrap = Expr.Invoke(e, converters.ToArray());

//            var caller = Expr.Lambda(wrap, null,  parameters.Select(x => Expr.Parameter(typeof(object), x.Name)));

//            return caller.Compile();
//        }
//    }
//}
