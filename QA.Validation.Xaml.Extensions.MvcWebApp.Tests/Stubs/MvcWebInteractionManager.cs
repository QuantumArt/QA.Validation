using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using QA.Validation.Extensions.Tests;
using QA.Validation.Xaml.Extensions.Rules;

namespace QA.Validation.Xaml.Extensions.MvcWebApp.Tests.Stubs
{
    public class MvcWebInteractionManager<TController> : IWebInteractionManager
        where TController : Controller
    {
        private Expression<Func<TController, Func<RemoteValidationContext, ActionResult>>> _expr;

        #region IWebInteractionManager Members

        public MvcWebInteractionManager(Expression<Func<TController, Func<RemoteValidationContext, ActionResult>>> expr)
        {
            _expr = expr;
        }

        public string GetResponseBody(Uri url, string modelToSent, string httpMethod, string contentType, NameValueCollection headers, int timeout)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                using (var fixture = HandlerExecutionFixture.Create(true)
                      .WithHttpMethod(httpMethod)
                      .WithRequestHeaders(headers)
                      .WithContentType(contentType)
                      .WithInputStream(modelToSent)
                      .WithUrl(url)
                      .WithAppRelativeCurrentExecutionFilePath("~/remotevalidation")
                      .WithPathInfo("")
                      .WithForm(new NameValueCollection())
                      .AllowSetResponseContentType()
                      .WithQueryString(new NameValueCollection())
                      .WithWriteMethod(writer))
                {

                    var ctx = fixture.Use();
                    var routes = new RouteCollection();
                    RouteConfig.RegisterRoutes(routes);
                    var routeData = routes.GetRouteData(ctx);
                    var rc = new RequestContext(ctx, routeData);

                    var controller = Activator.CreateInstance<TController>();

                    controller.ControllerContext = new ControllerContext(rc, controller);
                    controller.Url = new UrlHelper(rc);

                    var selector = _expr.Compile();

                    var result = selector(controller)
                        (JsonConvert.DeserializeObject<RemoteValidationContext>(modelToSent));

                    result.ExecuteResult(controller.ControllerContext);
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
