using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Validation.Xaml.Tests.Model;
using QA.Validation.Xaml.Tests.Util;

namespace QA.Validation.Xaml.Tests
{
    /// <summary>
    /// Проверки различных сценариев валидации нетипизированной модели
    /// </summary>
    [TestClass()]
    
    public class XamlObjectValidatorTest
    {
        #region Additional test attributes

        /// <summary>
        /// Код этого метода нужен только для теста. В реальной жизни он не понадобится
        /// </summary>
        [TestInitialize()]
        public void MyTestInitialize()
        {
            lock (typeof(DynamicResourceDictionaryContainer))
            {
                var dict = ValidationHelper.GetXaml<DynamicResourceDictionaryContainer>(DynamicResourceConstants.Container);
                DynamicResourceDictionaryContainer.SetResourceProvider(() => dict); 
            }
        }
       
        #endregion

        #region Проверки различных сценариев нетипизированной модели
        [TestMethod]
        [TestCategory ("Object validator samples")]
        public void Test_Checks_That_One_Property_Is_Not_Null()
        {
            var model = new Person { };
            var validator = ValidationHelper.GetXaml<XamlObjectValidator>(ValidatorConstants.Basic_Object);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.IsNotNull(context.Messages);
            Assert.AreEqual(context.Messages.Count, 0, "Validation summary must be empty.");
            Assert.IsNotNull(context.Result);
            Assert.AreEqual(context.Result.Errors.Count, 1);
            Assert.AreEqual(context.Result.Errors[0].Message, "Не может быть null");
            Assert.AreEqual(context.Result.Errors[0].Definition.PropertyName, "Name");
        }

        [TestMethod]
        [TestCategory("Object validator samples")]
        public void Test_Checks_That_Another_Property_Is_Required_If_The_First_One_Is_Provided()
        {
            var model = new Person { Name = "123" };
            var validator = ValidationHelper.GetXaml<XamlObjectValidator>(ValidatorConstants.Basic_Object);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.IsNotNull(context.Messages);
            Assert.IsNotNull(context.Result);
            Assert.AreEqual(context.Result.Errors[0].Message, "Поле DuplicateName обязательно для заполнения, если поле Name не пустое.");
            Assert.AreEqual(context.Result.Errors[0].Definition.PropertyName, "DuplicateName");
        }

        [TestMethod]
        [TestCategory("Object validator samples")]
        public void Test_Checks_Validation_Of_Equality_Of_Two_Members_With_Localized_Messages()
        {
            var model = new Person { Name = "test1", DuplicateName = "test2" };
            var validator = ValidationHelper.GetXaml<XamlObjectValidator>(ValidatorConstants.Basic_Object);
            var context = new ValidationContext();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-us");

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.IsNotNull(context.Messages);
            Assert.IsNotNull(context.Result);
            Assert.AreEqual(context.Messages[0], "Must be equal.");

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru-ru");
            context = new ValidationContext();
            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.IsNotNull(context.Messages);
            Assert.IsNotNull(context.Result);
            Assert.AreEqual(context.Messages[0], "Имя и повтор имени должны совпадать");

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr-fr");
            context = new ValidationContext();
            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.IsNotNull(context.Messages);
            Assert.IsNotNull(context.Result);
            Assert.AreEqual(context.Messages[0], "Имя и повтор имени должны совпадать (default culture)");
        } 
        #endregion
    }
}
