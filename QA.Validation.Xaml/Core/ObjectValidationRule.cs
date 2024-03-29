﻿using System.Collections.Generic;
using System.Linq;
using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    [ContentProperty("Items")]
    public abstract class ObjectValidationRule : IValidationRule
    {
        /// <summary>
        /// Условие правила
        /// </summary>
        ///
        public IList<ValidationCondition> Items { get; private set; }

        public ObjectValidationRule()
        {
            Items = new List<ValidationCondition>();
        }

        public ValidationCondition Condition
        {
            get { return Items.Any() ? Items.First() : null; }
            set
            {
                Items.Clear();
                Items.Add(value);
            }
        }

        public bool Validate(IValueProvider provider, IDefinitionStorage storage, ValidationContext result)
        {
            var ctx = new ValidationConditionContext()
            {
                Definition = new PropertyDefinition { },
                All = storage.GetAll(),
                ValueProvider = provider,
                ServiceProvider = result.ServiceProvider,
                CustomerCode = result.CustomerCode,
                LocalizeMessages = result.LocalizeMessages,
                SiteId = result.SiteId,
                ContentId = result.ContentId

            };

            var isValid = OnValidate(ctx);

            result.Messages.AddRange(ctx.Messages);
            result.Result.Errors.AddRange(ctx.Result.Errors);

            return isValid;
        }

        protected abstract bool OnValidate(ValidationConditionContext context);
    }
}
