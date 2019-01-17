using System.Collections.Generic;

namespace QA.Validation.Xaml.Adapters
{
    public interface IModelAdapter
    {
        Dictionary<string, string> AdaptModel<TModel>(TModel item);
    }
}
