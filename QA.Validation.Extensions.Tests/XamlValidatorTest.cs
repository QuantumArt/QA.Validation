using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Validation.Xaml;
using QA.Validation.Xaml.Extensions.Conditions;
using QA.Validation.Xaml.Extensions.ValueArguments;
using QA.Validation.Xaml.Tests.Util;

namespace QA.Validation.Extensions.Tests
{
    /// <summary>
    /// Валидация модели, описанной словарем
    /// </summary>
    [TestClass()]
    public class XamlValidatorTest
    {
        static XamlValidatorTest()
        {
            var encode = new Encode();
            System.Diagnostics.Trace.Write(encode.ToString());
        }

        #region Additional test attributes

        /// <summary>
        /// Код этого метода нужен только для теста. В реальной жизни он не понадобится
        /// </summary>
        [TestInitialize()]
        public void MyTestInitialize()
        {
            ApplyResourceDictionary();
            // подключение сборки с доп. правилами - иначе не будет работать пространство имен по умолчанию в текстах валидаторов
            var email = new IsEmail();
            Trace.WriteLine(email.ToString());
        }

        private static void ApplyResourceDictionary()
        {
            // Подключение внешнего ресурсного словаря
            lock (typeof(DynamicResourceDictionaryContainer))
            {
                var dict = ValidationHelper.GetXaml<DynamicResourceDictionaryContainer>(DynamicResourceConstants.Container);
                DynamicResourceDictionaryContainer.SetResourceProvider(() => dict);
            }
        }

        #endregion

        #region Проверки различных сценариев валидации модели, описанной словарем
        [TestMethod]
        [TestCategory("XamlValidator: samples")]
        public void Test_Checks_That_One_Property_Is_Not_Null()
        {
            var model = new Dictionary<string, string>()
                {
                    { "Name", null },
                    { "DuplicateName", null },
                    { "Age", "0" },
                    { "Passport", null },
                    { "Email", "mail@mail.ru" },
                    { "Date", null },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Basic_Dictionary);
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
        [TestCategory("XamlValidator: samples")]
        public void Test_Checks_That_Another_Property_Is_Required_If_The_First_One_Is_Provided()
        {
            var model = new Dictionary<string, string>()
                {
                    { "Name", "Иван" },
                    { "DuplicateName", null },
                    { "Age", "0" },
                    { "Passport", null },
                    { "Email", "mail@mail.ru" },
                    { "Date", null },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Basic_Dictionary);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.IsNotNull(context.Messages);
            Assert.IsNotNull(context.Result);
            Assert.AreEqual(context.Result.Errors[0].Message, "Поле DuplicateName обязательно для заполнения, если поле Name не пустое.");
            Assert.AreEqual(context.Result.Errors[0].Definition.PropertyName, "DuplicateName");
        }

        [TestMethod]
        [TestCategory("XamlValidator: samples")]
        public void Test_Checks_Validation_Of_Equality_Of_Two_Members_With_Localized_Messages()
        {
            var model = new Dictionary<string, string>()
                {
                    { "Name", "Ivanov" },
                    { "DuplicateName", "Ivanova" },
                    { "Age", "0" },
                    { "Passport", null },
                    { "Email", "mail@mail.ru" },
                    { "Date", null },
                };
            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Basic_Dictionary);
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


        [TestMethod]
        [TestCategory("XamlValidator: samples")]
        public void Test_Checks_That_Resource_Dictionaries_Work()
        {
            var model = new Dictionary<string, string>()
                {
                    { "Name", "Ivan" },
                    { "DuplicateName", "Ivan" },
                    { "Age", "35" },
                    { "Passport", null },
                    { "Email", "mail@mail.ru" },
                    { "Date", null },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Advanced_Dictionary);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.IsNotNull(context.Messages);
            Assert.IsNotNull(context.Result);

            //<!--к полю Name применяем условия, указанные в ресурсном словаре-->
            //<ForMember Definition="{x:Definition Name}" Condition="{x:Resource expr}" />
            //<!--к полю DuplicateName применяем те же условия, что и к Name-->
            //<ForMember Definition="{x:Definition DuplicateName}" Condition="{x:Resource expr}"/>
            Assert.IsTrue(context.Result.Errors.Count >= 2, "Должно быть не менее 2х ошибок");
            Assert.AreEqual(context.Result.Errors[0].Definition.PropertyName, "Name");
            Assert.AreEqual(context.Result.Errors[0].Message, "Длина поля должна быть не менее 5 символов.");
            Assert.AreEqual(context.Result.Errors[1].Definition.PropertyName, "DuplicateName");
            Assert.AreEqual(context.Result.Errors[1].Message, "Длина поля должна быть не менее 5 символов.");

            // PS есть и другие ошибки валидации, но мы их непроверяем
        }

        [TestMethod]
        [TestCategory("XamlValidator: samples")]
        public void Test_Checks_Complicated_Condition()
        {
            //<!--пример сложного условия: Если поле не пустое, то его значение должно быть валидировано-->
            // указываем непустое значение

            var model = new Dictionary<string, string>()
                {
                    { "Name", "Alexander" },
                    { "DuplicateName", "Alexander" },
                    { "Age", "35" },
                    { "Email", "mail@mail.ru" },
                    { "Passport", "1234 123456" },
                    { "Date", new DateTime(2011, 1, 1).ToString() },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Advanced_Dictionary);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.IsNotNull(context.Messages);
            Assert.IsNotNull(context.Result);

            Assert.AreEqual(context.Result.Errors[0].Definition.PropertyName, "Date");
            Assert.AreEqual(context.Result.Errors[0].Message, "Если дата указана, то она должна быть не ранее 2012.03.03");

            Assert.AreEqual(1, context.Result.Errors.Count);

            // Указываем пустое значение,
            model = new Dictionary<string, string>()
                {
                    { "Name", "Alexander" },
                    { "DuplicateName", "Alexander" },
                    { "Age", "35" },
                    { "Email", "mail@mail.ru" },
                    { "Passport", "1234 123456" },
                    { "Date", null },
                };

            context = new ValidationContext();
            validator.Validate(model, context);
            Assert.IsTrue(context.IsValid);
        }

        [TestMethod]
        [TestCategory("XamlValidator: samples")]
        public void Test_Checks_Must_Condition_With_Message_From_Dynamic_Resource()
        {
            //<!--Приминение подключаемого ресурсного словаря.
            //    В примере используется файл {Project}\DynamicResources/Container.xaml-->

            var model = new Dictionary<string, string>()
                {
                    { "Name", "Vasiliy" },
                    { "DuplicateName", "Vasiliy" },
                    { "Age", "35" },
                    { "Email", "mail@mail.ru" },
                    { "Passport", "1234 123456" },
                    { "Date", new DateTime(2014, 1, 1).ToString() },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Advanced_Dictionary);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.IsNotNull(context.Messages);
            Assert.IsNotNull(context.Result);
            Assert.IsTrue(context.Messages.Count == 1);
            Assert.AreEqual("Неверный формат фамилии или имени (динамические ресурсы).", context.Messages[0]);
        }

        [TestMethod]
        [TestCategory("XamlValidator: collections")]
        public void Test_Checks_That_Validation_Of_Lists_Works_Correctly()
        {
            //<!--Приминение подключаемого ресурсного словаря.
            //    В примере используется файл {Project}\DynamicResources/Container.xaml-->

            var model = new Dictionary<string, string>()
                {
                    { "Names", "1, 2, 3, 4, 5" },
                    { "Ids", null },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.ArrayType_Dictionary);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.AreEqual(context.Result.Errors[0].Definition.PropertyName, "Ids");
            Assert.AreEqual(context.Result.Errors[0].Message, "Не может быть null");

            Assert.AreEqual(1, context.Result.Errors.Count);

        }

        [TestMethod]
        [TestCategory("XamlValidator: collections")]
        public void Test_Checks_That_Validation_Of_Correct_Lists_Works_Passes()
        {
            //<!--Приминение подключаемого ресурсного словаря.
            //    В примере используется файл {Project}\DynamicResources/Container.xaml-->

            var model = new Dictionary<string, string>()
                {
                    { "Names", "1, 2, 3, 4, 5" },
                    { "Ids", "1, 2, 3, 4, 5" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.ArrayType_Dictionary);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.IsValid);
            Assert.AreEqual(0, context.Result.Errors.Count);
        }

        [TestMethod]
        [TestCategory("XamlValidator: samples")]
        [ExpectedException(typeof(XamlValidatorException))]
        public void Test_Checks_That_Validation_Of_Missing_Property_Causes_XamlValidatorException()
        {
            //<!--Приминение подключаемого ресурсного словаря.
            //    В примере используется файл {Project}\DynamicResources/Container.xaml-->

            var model = new Dictionary<string, string>()
                {
                    { "PropertyNotExists", "value" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Basic_Dictionary);
            var context = new ValidationContext();

            validator.Validate(model, context);
        }

        [TestMethod]
        [TestCategory("XamlValidator: collections")]
        public void Test_Checks_IsUnique_Condition_Fails_On_Duplicates()
        {
            //<!--Приминение подключаемого ресурсного словаря.
            //    В примере используется файл {Project}\DynamicResources/Container.xaml-->

            var model = new Dictionary<string, string>()
                {
                    { "Names", "иван, родил, девчонку, иван, тащить, пелёнку" },
                    { "Ids", "1, 2, 3, 1, 5" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Advanced_ArrayType_Dictionary);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.AreEqual(2, context.Result.Errors.Count);

            Assert.AreEqual("Ids", context.Result.Errors[0].Definition.PropertyName);
            Assert.AreEqual("Значения должны быть уникальны.", context.Result.Errors[0].Message);
            Assert.AreEqual("Names", context.Result.Errors[1].Definition.PropertyName);
            Assert.AreEqual("Значения должны быть уникальны.", context.Result.Errors[1].Message);
        }

        [TestMethod]
        [TestCategory("XamlValidator: collections")]
        public void Test_Checks_IsUnique_Condition_Passes_On_Unique_Items()
        {
            //<!--Приминение подключаемого ресурсного словаря.
            //    В примере используется файл {Project}\DynamicResources/Container.xaml-->

            var model = new Dictionary<string, string>()
                {
                    { "Names", "иван, родил, девчонку, велел, тащить, пелёнку" },
                    { "Ids", "1, 2, 3, 12, 5" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Advanced_ArrayType_Dictionary);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.IsValid);
        }

        [TestMethod]
        [TestCategory("XamlValidator: samples")]
        public void Test_Checks_That_One_Property_Is_Not_Null_In_Case_Of_Different_Alias_And_PropretyName()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", null },
                    { "field_1235", null },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Alias_Dictionary);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.IsNotNull(context.Messages);
            Assert.AreEqual(context.Messages.Count, 0, "Validation summary must be empty.");
            Assert.IsNotNull(context.Result);
            Assert.AreEqual(context.Result.Errors.Count, 1);
            Assert.AreEqual(context.Result.Errors[0].Message, "Не может быть null");
            Assert.AreEqual(context.Result.Errors[0].Definition.PropertyName, "field_1234");
        }

        [TestMethod]
        [TestCategory("XamlValidator: samples")]
        public void Test_Checks_That_Passes_In_Case_Of_Different_Alias_And_PropretyName()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", "Ivanov" },
                    { "field_1235", "Ivanov" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Alias_Dictionary);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.IsValid);
        }

        [TestMethod]
        [TestCategory("XamlValidator: samples")]
        public void Test_WithValueFormattedMessage()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", "Ivanov123" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.ValueProvider);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.AreEqual("Hello Ivanov123", context.Result.Errors[0].Message);

        }


        [TestMethod]
        [TestCategory("XamlValidator: samples")]
        public void Test_Checks_Expressions_Is_Invalid()
        {
            //<!--Приминение подключаемого ресурсного словаря.
            //    В примере используется файл {Project}\DynamicResources/Container.xaml-->

            var model = new Dictionary<string, string>()
                {
                    { "Name", "122" },
                    { "Age", "9"},
                    {"Date", DateTime.Now.AddDays(-11).ToString()}
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Basic_Expressions);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
        }

        [TestMethod]
        [TestCategory("XamlValidator: samples")]
        public void Test_Checks_Expressions_Is_Valid()
        {
            //<!--Приминение подключаемого ресурсного словаря.
            //    В примере используется файл {Project}\DynamicResources/Container.xaml-->

            var model = new Dictionary<string, string>()
                {
                    { "Name", "123" },
                    { "Age", "19"},
                    {"Date", DateTime.Now.AddDays(1).ToString()}
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Basic_Expressions);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.Result.Errors.Count == 0);
        }

        [TestMethod]
        [TestCategory("XamlValidator: samples")]
        public void Test_Checks_Expressions_Custom_001()
        {
            //<!--Приминение подключаемого ресурсного словаря.
            //    В примере используется файл {Project}\DynamicResources/Container.xaml-->

            var model = new Dictionary<string, string>()
                {
                    { "Name", "123" },
                    { "Age", "19"},
                    {"Date", DateTime.Now.AddYears(-10).ToString()}
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Basic_Expressions);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.Result.Errors.Count == 2);
        }

        [TestMethod]
        [TestCategory("ValueArgumentTest")]
        public void Basic_arguments()
        {
            var model = new Dictionary<string, string>()
            {
                { "field_1234", "" },
                { "field_1235", "ТЕСТ все Входящие для ёжика буД!@#$%^&ут бесплатны!123 123 3" },
            };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.ValueArguments.Example_001);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(((string)model["field_1234"]).Length > 100);
        }

        #endregion
    }
}
