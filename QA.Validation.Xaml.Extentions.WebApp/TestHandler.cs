using System;
using System.Web;
using QA.Validation.Xaml.Extensions.Rules;
using QA.Validation.Xaml.Extensions.Rules.Remote;

namespace QA.Validation.Xaml.Extentions.WebApp
{
    public class TestHandler : ValidationHandlerBase
    {
        public TestHandler()
        {

        }
        public override bool IsReusable
        {
            get { return true; }
        }

        protected override void OnValidation(RemoteValidationContext model, RemoteValidationResult context)
        {
            context.Messages.Add("Есть ошибки валидации");

            var name = model.ProvideValueExact<string>(model.Definitions[0]);
            var age = model.ProvideValueExact<int>(model.Definitions[1]);
            var date = model.ProvideValueExact<DateTime?>(model.Definitions[2]);
            var length = model.ProvideValueExact<double>(model.Definitions[3]);
            var checkBox = model.ProvideValueExact<bool?>(model.Definitions[4]);

            // проверки:

            if (string.IsNullOrEmpty(name))
            {
                context.Result.AddError(model.Definitions[0].PropertyName, "Поле должно быть заполнено");
            }

            if (age < 18 || age > 100)
            {
                context.Result.AddError(model.Definitions[1].PropertyName, "Возраст должен быть не менее 18, но не более 100");
            }

            if (date == null)
            {
                context.Result.AddError(model.Definitions[2].PropertyName, "Укажите дату");
            }

            if (length <= 100)
            {
                context.Result.AddError(model.Definitions[3].PropertyName, "Должно быть более 100");
            }

            if (checkBox == false)
            {
                context.Result.AddError(model.Definitions[4].PropertyName, "Должно быть выбрано");
            }

            if (checkBox == null)
            {
                context.Result.AddError(model.Definitions[4].PropertyName, "Не может быть null");
            }
        }

    }
}
