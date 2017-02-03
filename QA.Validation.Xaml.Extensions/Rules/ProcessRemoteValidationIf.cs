﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using QA.Validation.Xaml.Extensions.Rules.Remote;

namespace QA.Validation.Xaml.Extensions.Rules
{
    /// <summary>
    /// Валидационное правило. Применяется ко всей модели.
    /// Ошибки валидации выводятся в списке общих сообщений
    /// </summary>
    public class ProcessRemoteValidationIf : ObjectValidationRule
    {
        private IWebInteractionManager _manager;

        #region Properties
        /// <summary>
        /// Адрес обработчика удаленной валидации
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// QP customer code. Необходимо указывать.
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// QP SiteId. Необходимо указывать.
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// По умолчанию 'POST'
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// По умолчанию 'application/json; charset=UTF-8'
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Таймаут на обращение к сервису валидации (в мс). 
        /// По умолчанию 5000 (5 секунд)
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Список полей, которые подлежат отправке на удаленною валидацию
        /// </summary>
        public List<PropertyDefinition> DefinitionsToSend { get; private set; }

        /// <summary>
        /// Флаг устанавливки заголовка X-Requested-With
        /// </summary>
        public bool AddAjaxHeader { get; set; } 
        #endregion

        /// <summary>
        /// Конструктор
        /// HttpMethod = "POST";
        /// ContentType = "application/json; charset=UTF-8";
        /// Timeout = 5000;
        /// </summary>
        public ProcessRemoteValidationIf()
        {
            DefinitionsToSend = new List<PropertyDefinition>();
            HttpMethod = "POST";
            ContentType = "application/json; charset=UTF-8";
            Timeout = 5000;
            _manager = new WebInteractionManager();
        }

        /// <summary>
        /// Предоставляет функционал по подмене менеджера обращения к хендлеру
        /// </summary>
        /// <param name="manager"></param>
        public ProcessRemoteValidationIf(IWebInteractionManager manager)
            : this()
        {
            _manager = manager;
        }

        protected override bool OnValidate(ValidationConditionContext ctx)
        {
            // пропускаем валидацию, если необходимо
            if (Condition != null && !Condition.Execute(ctx))
            {
                return false;
            }

            var values = new Dictionary<string, object>();

            foreach (var definition in DefinitionsToSend)
            {
                values[definition.PropertyName] = ctx.ValueProvider.GetValue(definition);
            }

            var context = new RemoteValidationContext
            {
                CustomerCode = CustomerCode,
                CurrentUICulture = CultureInfo.CurrentUICulture.Name,
                CurrentCulture = CultureInfo.CurrentCulture.Name,
                SiteId = SiteId,
                Values = values,
                Definitions = DefinitionsToSend.Select(x => new RemotePropertyDefinition
                {
                    PropertyName = x.PropertyName,
                    Alias = x.Alias,
                    PropertyType = x.PropertyType.AssemblyQualifiedName
                }).ToList()
            };

            var headers = new NameValueCollection();

            if (AddAjaxHeader)
            {
                headers.Add("X-Requested-With", "XMLHttpRequest");
            }
            
            var responseBody = _manager
                .GetResponseBody(Url,
                    RemoteValidationContext.GetJson(context),
                    HttpMethod,
                    ContentType,
                    headers,
                    Timeout);

            RemoteValidationResult result = (new JavaScriptSerializer())
                .Deserialize<RemoteValidationResult>(responseBody);

            ctx.Messages.AddRange(result.Messages);
            ctx.Result.Errors.AddRange(result.Result.Errors);

            return true;
        }
    }
}
