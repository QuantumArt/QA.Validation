using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QA.Validation.Xaml.Adapters
{
    public interface IModelAdapter
    {
        Dictionary<string, string> AdaptModel<TModel>(TModel item);
    }
}
