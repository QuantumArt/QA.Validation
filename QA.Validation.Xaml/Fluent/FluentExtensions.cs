using System;
using System.Linq.Expressions;

namespace QA.Validation.Xaml.Fluent
{
    /// <summary>
    /// Этот функционал не готов
    /// </summary>
    public static class FluentExtensions
    {
        public static IValidationRule ForMember<TModel, TProperty>(this XamlValidatorBase validator, Expression<Func<TModel, TProperty>> prop)
        {
            //var ctx = new ForMember() { Definition = PropertyDefinition.Create(prop) };
            //validator.ValidationRules.Add(ctx);
            //return ctx;
            throw new NotImplementedException();
        }

        public static If If(this IValidationRule rule)
        {
            rule.Condition = new If();
            return (If)rule.Condition;
        }

        public static If If(this ValidationCondition rule)
        {
            return new If();
        }

        public static If True(this If rule, ValidationCondition condition)
        {
            rule.Then = condition;
            return rule;
        }

        public static If False(this If rule, ValidationCondition condition)
        {
            rule.Else = condition;
            return rule;
        }

        public static CompositeCondition WithCondition(this CompositeCondition item, ValidationCondition condition)
        {
            //item.Condition = condition;
            return item;
        }
    }
}
