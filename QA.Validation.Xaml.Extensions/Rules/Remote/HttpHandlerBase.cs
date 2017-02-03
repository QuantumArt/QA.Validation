using System;
using System.Web;

namespace QA.Validation.Xaml.Extensions.Rules.Remote
{
    /// <summary>
    /// Базовый класс для http-хендлера, поддерживающего Unit-тестирование
    /// </summary>
    public abstract class HttpHandlerBase : IHttpHandler
    {
        #region IHttpHandler Members

        public abstract bool IsReusable
        {
            get;
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            try
            {
                OnProcessRequest(new HttpContextWrapper(context));
            }
            catch (Exception ex) 
            {
                context.Response.Clear();
                context.Response.Write(ex.ToString() + " " + ex.StackTrace);
                context.Response.End();
               // context.Response.StatusCode = 500;
            }
        }

        public  abstract void OnProcessRequest(HttpContextBase context);
        #endregion
    }
}
