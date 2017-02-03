
namespace QA.Validation.Xaml
{   
    /// <summary>
    /// Операнд ИЛИ. Выполняются условия из коллекции MultiCondition.Items
    /// Выполнение будет остановлено на первом успешном условии.
    /// </summary>
    public class Or : MultiCondition
    {
        public override bool Execute(ValidationConditionContext context)
        {
            foreach(var item in Items)
            {
                if (item.Execute(context))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
