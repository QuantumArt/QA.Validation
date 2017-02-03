
namespace QA.Validation.Xaml
{
    /// <summary>
    /// Выполняются по цепочке условия из коллекции MultiCondition.Items
    /// Выполнение будет остановлено, если .
    /// </summary>
    public class Sequence : MultiCondition
    {
        public bool StopOnFirstFailure { get; set; }
        
        public override bool Execute(ValidationConditionContext context)
        {
            bool isValid = true;
            foreach (var item in Items)
            {
                if (((isValid = item.Execute(context)) != true) &&
                    (item.StopPropagation || StopOnFirstFailure))
                {
                    break;
                }
            }

            return isValid;
        }
    }
}
