using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace QA.Validation.Xaml.Extensions.Rules
{
    public class WebInteractionManager : IWebInteractionManager
    {
        public string GetResponseBody(Uri url, string serializedModel, string httpMethod, string contentType, NameValueCollection headers, int timeout)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Headers.Add(headers);

            request.Method = httpMethod;
            request.ContentType = contentType;
            request.Timeout = timeout;

            var encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(serializedModel);
            request.ContentLength = data.Length;

            if (httpMethod.Equals("post", StringComparison.InvariantCultureIgnoreCase))
            {
                var requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
            }
                        
            using (var response = request.GetResponse())
            {
                var responseStream = response.GetResponseStream();
                using (var reader = new StreamReader(responseStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
