using System.Collections.Generic;
using System.Windows.Markup;

namespace QA.Validation.Xaml
{
    [ContentProperty("Condition")]
    public abstract class ObjectValidationRule : IValidationRule
    {
        /// <summary>
        /// Условие правила
        /// </summary>
        public ValidationCondition Condition { get; set; }

        public bool Validate(IValueProvider provider, IDefinitionStorage storage, ValidationContext result)
        {
            var ctx = new ValidationConditionContext()
            {
                Definition = new PropertyDefinition { },
                All = storage.GetAll(),
                ValueProvider = provider,
                ServiceProvider = result.ServiceProvider,
                CustomerCode = result.CustomerCode,
                SiteId = result.SiteId
            };

            var isValid = OnValidate(ctx);

            result.Messages.AddRange(ctx.Messages);
            result.Result.Errors.AddRange(ctx.Result.Errors);

            return isValid;
        }

        protected abstract bool OnValidate(ValidationConditionContext context);
    }
}
