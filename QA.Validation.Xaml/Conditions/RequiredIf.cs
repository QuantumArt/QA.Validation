
namespace QA.Validation.Xaml
{
    /// <summary>
    /// Устанавливает ошибку для текущего свойства, если выполнено условие
    /// </summary>
    public class RequiredIf : CompositeCondition
    {
        public WithMessage Message { get; set; }
        public override bool Execute(ValidationConditionContext context)
        {
            if (Condition != null)
            {
                if (Condition.Execute(context))
                {
                    if (Message != null)
                    {
                        object val = context.ValueProvider.GetValue(context.Definition);
                        if (object.Equals(val, null)
                            || (val is string && string.IsNullOrEmpty((string)val)))
                        {
                            Message.Execute(context);
                        }
                    }
                }
            }

            return true;
        }
    }
}
