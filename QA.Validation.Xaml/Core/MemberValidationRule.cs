using Portable.Xaml.Markup;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Правило валидации
    /// </summary>
    [ContentProperty("Condition")]
    public abstract class MemberValidationRule : IValidationRule, QA.Validation.Xaml.Core.IMemberValidationRule
    {
        /// <summary>
        /// Условие валидации
        /// </summary>
        public ValidationCondition Condition { get; set; }

        /// <summary>
        /// Поле, которое бкдет валидироваться
        /// </summary>
        public PropertyDefinition Definition { get; set; }

        public bool Validate(IValueProvider provider, IDefinitionStorage storage, ValidationContext result)
        {
            var ctx = new ValidationConditionContext()
            {
                Definition = Definition,
                All = storage.GetAll(),
                ValueProvider = provider
            };

            var isValid = OnValidate(ctx);

            result.Messages.AddRange(ctx.Messages);
            result.Result.Errors.AddRange(ctx.Result.Errors);

            return isValid;
        }

        protected abstract bool OnValidate(ValidationConditionContext context);
    }
}
