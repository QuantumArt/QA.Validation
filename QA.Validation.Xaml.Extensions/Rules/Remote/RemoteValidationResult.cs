using System.Collections.Generic;

namespace QA.Validation.Xaml.Extensions.Rules
{
    /// <summary>
    /// Объект с результатами валидации
    /// </summary>
    public class RemoteValidationResult : ValidationContextBase
    {
        public Dictionary<string, object> NewValues { get; set; } = new Dictionary<string, object>();
    }
}
