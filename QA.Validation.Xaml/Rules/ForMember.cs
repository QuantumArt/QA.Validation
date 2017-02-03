
namespace QA.Validation.Xaml
{
    /// <summary>
    /// Валидационное правило. Применяется к конкретному полу.
    /// Ошибки валидации выводятся в списке ValidationContext.Result.Errors
    /// </summary>
    public class ForMember : MemberValidationRule
    {
        protected override bool OnValidate(ValidationConditionContext ctx)
        {
            return Condition.Execute(ctx);
        }
    }
}
