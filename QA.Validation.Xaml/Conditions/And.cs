
namespace QA.Validation.Xaml
{
    /// <summary>
    /// Операнд И. Выполняются условия из коллекции MultiCondition.Items
    /// Выполнение будет остановлено на первом неуспешном условии.
    public class And : MultiCondition
    {
        public override bool Execute(ValidationConditionContext context)
        {
            foreach(var item in Items)
            {
                if (!item.Execute(context))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
