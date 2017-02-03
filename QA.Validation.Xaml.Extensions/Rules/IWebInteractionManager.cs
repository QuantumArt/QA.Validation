using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace QA.Validation.Xaml.Extensions.Rules
{
    public interface IWebInteractionManager
    {
        string GetResponseBody(Uri url, string context, string httpMethod, string contentType, NameValueCollection headers, int timeout);
    }
}
