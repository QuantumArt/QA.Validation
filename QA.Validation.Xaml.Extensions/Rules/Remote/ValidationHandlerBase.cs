using System;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

namespace QA.Validation.Xaml.Extensions.Rules.Remote
{
    /// <summary>
    /// Базовый класс для хендлера remote-валидации
    /// </summary>
    public abstract class ValidationHandlerBase : HttpHandlerBase
    {
        public sealed override void OnProcessRequest(HttpContextBase context)
        {
            if (context.Request.HttpMethod.Equals("get", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.Write("Use post to pass the model");
                context.Response.End();
                return;
            }
            // TODO: parse request
            string json = "";

            using (var streamReader = new StreamReader(context.Request.InputStream))
            {
                json = streamReader.ReadToEnd();
            }

            var serializer = new JavaScriptSerializer();

            // deserialize model
            var model = serializer.Deserialize<RemoteValidationContext>(json);

            // convert to ValidationModel

            // call onvalidation
            var result = new RemoteValidationResult();
            OnValidation(model, result);

            // serialize results

            var resultJson = serializer.Serialize(result);

            context.Response.Write(resultJson);
            context.Response.ContentType = "application/json; charset=UTF-8";
        }

        protected abstract void OnValidation(RemoteValidationContext context, RemoteValidationResult result);
    }
}
