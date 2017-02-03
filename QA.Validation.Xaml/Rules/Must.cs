
namespace QA.Validation.Xaml
{
    /// <summary>
    /// Валидационное правило. Применяется ко всей модели.
    /// Ошибки валидации выводятся в списке общих сообщений
    /// </summary>
    public class Must : ObjectValidationRule
    {
        protected override bool OnValidate(ValidationConditionContext ctx)
        {
            return Condition.Execute(ctx);
        }
    }
}
