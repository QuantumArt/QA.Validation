using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QA.Validation.Xaml.Extensions.Rules.Remote;

namespace QA.Validation.Xaml.Extensions.Rules
{
    /// <summary>
    /// Валидационное правило. Применяется ко всей модели.
    /// Ошибки валидации выводятся в списке общих сообщений
    /// </summary>
    public class ProcessRemoteValidationIf : ObjectValidationRule
    {
        private readonly IWebInteractionManager _manager;

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

        /// <summary>
        /// Применять ли новые значения полей во время валидации
        /// </summary>
        public bool ApplyValues { get; set; }
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

        /// <summary>
        /// Обработчик валидационного события
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
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
                CustomerCode = ctx.CustomerCode ?? CustomerCode,
                CurrentUICulture = CultureInfo.CurrentUICulture.Name,
                CurrentCulture = CultureInfo.CurrentCulture.Name,
                SiteId = ctx.SiteId != 0 ? ctx.SiteId : SiteId,
                ContentId = ctx.ContentId,
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
            string delimiter = !Url.ToString().Contains("?") ? "?" : "&";
            var url = (context.CustomerCode == null) ? Url : new Uri(Url + delimiter + "customerCode=" + context.CustomerCode);

            var responseBody = _manager
                .GetResponseBody(url,
                    RemoteValidationContext.GetJson(context),
                    HttpMethod,
                    ContentType,
                    headers,
                    Timeout);

            RemoteValidationResult result = (JsonConvert.DeserializeObject<RemoteValidationResult>(responseBody));


            if (result.NewValues != null)
            {
                if (ApplyValues)
                {
                    // смотрим, пришли ли значения полей, и если пришли, то применяем

                    foreach (var kvp in result.NewValues)
                    {
                        var definition = DefinitionsToSend.FirstOrDefault(x => x.PropertyName == kvp.Key);
                        if (definition == null)
                        {
                            throw new XamlValidatorException(XamlValidatorException.ValidatorErrorReason.ValidatorError,
                                $"Unable to set value for definition ('{kvp.Key}') which is not in the 'Definitions to send' collection.");
                        }

                        ctx.ValueProvider.SetValue(definition, kvp.Value);
                    }
                }
                else if(result.NewValues.Any())
                {
                    // add error
                    ctx.Messages.Add("Unable to set value from remote validation thus applying of values is disabled. Consider to set ApplyValues to true.");
                }
            }

            ctx.Messages.AddRange(result.Messages);
            ctx.Result.Errors.AddRange(result.Result.Errors);

            return true;
        }
    }
}
