using System;
using System.Linq;
using System.Web.Mvc;
using QA.Validation.Xaml.Extensions.Rules;
using QA.Validation.Xaml.ListTypes;

namespace QA.Validation.Xaml.Extensions.MvcWebApp.Controllers
{
    /// <summary>
    /// Демонстрация remote-валидации
    /// </summary>
    public class RemoteValidationController : Controller
    {
        //
        // GET: /RemoteValidation/

        /// <summary>
        /// Пример обработки удаленной валидации
        /// </summary>
        /// <param name="model">Тип модели должен быть RemoteValidationContext</param>
        /// <returns>результаты валидации</returns>
        [HttpPost]
        public JsonResult Index(RemoteValidationContext model)
        {
            var result = new RemoteValidationResult();

            #region валидация
            // TODO: Произвести валидацию пришедшей модели 

            // для примера проверим наличие всех обязательных полей:
            if (model.SiteId != 35)
            {
                result.AddErrorMessage("Неверно указан siteid");
            }

            if (model.CustomerCode != "QP123")
            {
                result.AddErrorMessage("Неверно указан customercode");
            }

            result.Messages.Add("Тестовое сообщение");
            // ... и добавим ошибку для одного из полей:

            var valueOfProp3 = model.ProvideValueExact<DateTime?>("prop3");

            if (valueOfProp3 == null)
            {
                result.AddModelError("prop3", "Укажите дату");
            }
            else if (valueOfProp3.Value < DateTime.Parse("2013.01.01 18:00:00"))
            {
                result.AddModelError("prop3", "Введенная дата меньше 2013.01.01 18:00:00");
            }



            #endregion

            return Json(result);
        }


        /// <summary>
        /// Пример обработки удаленной валидации
        /// </summary>
        /// <param name="model">Тип модели должен быть RemoteValidationContext</param>
        /// <returns>результаты валидации</returns>
        [HttpPost]
        public JsonResult ListOfIntTest(RemoteValidationContext model)
        {
            var result = new ValidationContext();

            #region валидация
            try
            {
                var def = model.Definitions.FirstOrDefault(x => x.Alias == "prop6");

                var value = model.ProvideValueExact(def);
                var typedValue = model.ProvideValueExact<ListOfInt>(def.PropertyName);

            }
            catch (Exception ex)
            {
                result.AddErrorMessage(ex.Message);
            }
            #endregion

            return Json(result);
        }
    }
}
