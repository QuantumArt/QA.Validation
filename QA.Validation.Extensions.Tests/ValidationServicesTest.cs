using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Configuration;
using QA.Validation.Xaml;
using QA.Validation.Xaml.Extensions.Conditions;
using QA.Validation.Xaml.ListTypes;
using QA.Validation.Xaml.Tests.Util;

namespace QA.Validation.Extensions.Tests
{
    /// <summary>
    /// Тесты интеграционного API.
    /// </summary>
    [TestClass]
    public class ValidationServicesTest
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            // подключение сборки с доп. правилами - иначе не будет работать пространство имен по умолчанию в текстах валидаторов.
            // TODO: сделать bootstrapper для автоматической загрузки в домен приложения таких сборок.
            // В случае веб-приложения можно класть такие сборки в bin
            var email = new IsEmail();
            Trace.WriteLine(email.ToString());
        }
        #region Тестирование установки значений

        [TestMethod]
        [TestCategory("set values")]
        public void Test_Applying_new_value_of_type_string()
        {
            string fieldName = "field_1234";
            string valueToSet = "new value";

            var model = CreateValidatorAndRun<string>(fieldName, valueToSet);

            Assert.AreEqual(valueToSet, model[fieldName]);
        }

        [TestMethod]
        [TestCategory("set values")]
        public void Test_Applying_new_value_of_type_int()
        {
            string fieldName = "field_1234";
            int valueToSet = 12;

            var model = CreateValidatorAndRun<string>(fieldName, valueToSet);

            Assert.AreEqual(valueToSet.ToString(), model[fieldName]);
        }

        [TestMethod]
        [TestCategory("set values")]
        public void Test_Applying_new_value_of_type_bool()
        {
            string fieldName = "field_1234";
            bool valueToSet = true;

            var model = CreateValidatorAndRun<string>(fieldName, valueToSet);

            Assert.AreEqual(valueToSet.ToString(), model[fieldName]);
        }

        private static Dictionary<string, string> CreateValidatorAndRun<T>(string fieldName, object valueToSet)
        {
            // создадим пустой валидатор c одним полем
            string newValidatorText = CreateSimpleValidatorText<T>(fieldName, valueToSet);

            // это модель с полями статьи
            var model = new Dictionary<string, string>()
            {
                {"field_1234", null},
                {"field_1235", null},
            };

            var result = ValidationServices.ValidateModel(model, newValidatorText, null);

            Assert.IsTrue(result.IsValid);

            // после запуска валидатора модель должна поменяться
            return model;
        }

        private static string CreateSimpleValidatorText<T>(string fieldName, object fieldValue)
        {
            var emptyValidatorString = ValidationServices.GenerateXamlValidatorText(new PropertyDefinition[] { new PropertyDefinition
            {
                PropertyName = fieldName,
                PropertyType = typeof(string),
                Alias = fieldName }
            });

            var validator = (XamlValidator)XamlConfigurationParser.CreateFrom(emptyValidatorString);

            // добавим в валидатор логику выставления значения для этого поля
            validator.ValidationRules.Add(new ForMember
            {
                Definition = validator.Definitions[fieldName],
                Condition = new ApplyValue { Value = fieldValue }
            });

            var newValidatorText = XamlConfigurationParser.Save(validator);
            return newValidatorText;
        }
        #endregion


        #region Тестирование проверки валидаторов

        [TestMethod]
        [TestCategory("ValidationServices: test validators")]
        public void Test_TestResourceDictionary_Checks_That_Passes_On_Valid()
        {
            var resourceDictrionary = ValidationHelper.GetEmbeddedResourceText(DynamicResourceConstants.Container);

            ValidationServices.TestResourceDictionary(resourceDictrionary);
        }

        [TestMethod]
        [TestCategory("ValidationServices: test validators")]
        public void Test_TestValidator_Checks_That_Passes_On_Than_Text_Is_Valid()
        {
            var resourceDictrionary = ValidationHelper.GetEmbeddedResourceText(DynamicResourceConstants.Container);
            var validator = ValidationHelper.GetEmbeddedResourceText(ValidatorConstants.Alias_Dictionary);

            ValidationServices.TestValidator(validator, resourceDictrionary);
        }

        [TestMethod]
        [TestCategory("ValidationServices: test validators")]
        public void Test_TestValidator_With_Model_Checks_That_Passes_On_Valid()
        {
            var resourceDictrionary = ValidationHelper.GetEmbeddedResourceText(DynamicResourceConstants.Container);
            var validator = ValidationHelper.GetEmbeddedResourceText(ValidatorConstants.Alias_Dictionary);

            var model = new Dictionary<string, string>()
            {
                {"field_1234", null},
                {"field_1235", null},
            };

            ValidationServices.TestValidator(model, validator, resourceDictrionary);
        }

        [TestMethod]
        [ExpectedException(typeof(XamlValidatorException))]
        [TestCategory("ValidationServices: test validators")]
        public void Test_TestValidator_With_Model_Checks_That_Fails_On_Missing_Property()
        {
            var resourceDictrionary = ValidationHelper.GetEmbeddedResourceText(DynamicResourceConstants.Container);
            var validator = ValidationHelper.GetEmbeddedResourceText(ValidatorConstants.Alias_Dictionary);

            var model = new Dictionary<string, string>()
            {
                // убираем поле из модели. В валидаторе оно по-прежнему присутствует
                //{"field_1234", null},
                {"field_1235", null},
            };

            ValidationServices.TestValidator(model, validator, resourceDictrionary);
        }

        [TestMethod]
        [TestCategory("ValidationServices: test validators")]
        public void Test_TestValidator_With_Model_Checks_That_Passes_When_Model_Contains_Extra_Properties()
        {
            var resourceDictrionary = ValidationHelper.GetEmbeddedResourceText(DynamicResourceConstants.Container);
            var validator = ValidationHelper.GetEmbeddedResourceText(ValidatorConstants.Alias_Dictionary);

            var model = new Dictionary<string, string>()
            {
                // в описании валидатора есть только такие поля:
                {"field_1234", null},
                {"field_1235", null},
                // добавим еще
                {"Name", null},
                {"field_9999", null},

            };

            ValidationServices.TestValidator(model, validator, resourceDictrionary);
        }
        #endregion

        #region Тестирование разных сценариев валидации модели с заполненными полями
        [TestMethod]
        [TestCategory("ValidationServices: samples")]
        public void Test_ValidateModel_Checks_That_One_Property_Is_Not_Null()
        {
            var obj = new ValidationParamObject
            {
                DynamicResource = ValidationHelper.GetEmbeddedResourceText(DynamicResourceConstants.Container),
                Validator = ValidationHelper.GetEmbeddedResourceText(ValidatorConstants.Alias_Dictionary),
                Model = new Dictionary<string, string>()
                {
                    {"field_1234", null},
                    {"field_1235", null},
                }
            };
            var result = ValidationServices.ValidateModel(obj);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotNull(result.Messages);
            Assert.AreEqual(result.Messages.Count, 0, "Validation summary must be empty.");
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(result.Result.Errors.Count, 1);
            Assert.AreEqual(result.Result.Errors[0].Message, "Не может быть null");
            Assert.AreEqual(result.Result.Errors[0].Definition.PropertyName, "field_1234");
        }

        //[TestMethod]
        [TestCategory("ValidationServices: samples")]
        public void Test_ValidateModel_Checks_Complicated_Condition()
        {
            //<!--пример сложного условия: Если  не пустое, то его значение должно быть валидировано-->
            // Условие: если поле Date заполнено, то тожно быть не менее '2012.03.03 00:00:00'
            // указываем непустое значение

            #region описания полей
            //<PropertyDefinition Alias="Name" PropertyName="field_1234" PropertyType="{x:Type x:String}"/>
            //<PropertyDefinition Alias="DuplicateName" PropertyName="field_1235" PropertyType="{x:Type x:String}"/>
            //<PropertyDefinition Alias="Passport" PropertyName="field_1236" PropertyType="{x:Type x:String}"/>
            //<PropertyDefinition Alias="Date" PropertyName="field_1237" PropertyType="{x:Type sys:DateTime}"/>
            //<PropertyDefinition Alias="Email" PropertyName="field_1238" PropertyType="{x:Type x:String}"/>
            //<PropertyDefinition Alias="Age" PropertyName="field_1239" PropertyType="{x:Type x:Int32}"/>
            //<PropertyDefinition Alias="Ids"  PropertyName="field_1210" PropertyType="{x:Type ListOfInt}"/>
            //<PropertyDefinition Alias="Names" PropertyName="field_1211" PropertyType="{x:Type ListOfString}"/>

            #endregion

            var obj = new ValidationParamObject
            {
                DynamicResource = ValidationHelper.GetEmbeddedResourceText(DynamicResourceConstants.Container),
                Validator = ValidationHelper.GetEmbeddedResourceText(ValidatorConstants.Alias_Advanced),
                Model = new Dictionary<string, string>()
                {
                    {"field_1234", "Alexander"},
                    {"field_1235", "Alexander"},
                    {"field_1236", "1234 123456"},
                    {"field_1237", new DateTime(2011, 1, 1).ToString(CultureInfo.InvariantCulture)},
                    {"field_1238", "mail@mail.ru"},
                    {"field_1239", "35"},
                    {"field_1210", "1, 2, 3, 4"},
                    {"field_1211", "a, b, c, d"},
                }
            };

            var result = ValidationServices.ValidateModel(obj);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotNull(result.Messages);
            Assert.IsNotNull(result.Result);

            Assert.AreEqual(result.Result.Errors[0].Definition.PropertyName, "field_1237");
            Assert.AreEqual(result.Result.Errors[0].Message, "Если дата указана, то она должна быть не ранее 2012.03.03");

            Assert.AreEqual(1, result.Result.Errors.Count);

            // Указываем пустое значение,
            obj.Model = new Dictionary<string, string>()
                {
                    { "field_1234", "Alexander" },
                    { "field_1235", "Alexander" },
                    { "field_1236", "1234 123456" },
                    { "field_1237", new DateTime(2014, 1, 1).ToString(CultureInfo.InvariantCulture) },
                    { "field_1238", "mail@mail.ru" },
                    { "field_1239", "35" },
                    { "field_1210", "1, 2, 3, 4" },
                    { "field_1211", "a, b, c, d" },
                };

            result = ValidationServices.ValidateModel(obj);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [TestCategory("ValidationServices: samples")]
        public void Test_TestValidator_Checks_Multiple_Validators()
        {
            var obj = new ValidationParamObject
            {
                DynamicResource = ValidationHelper.GetEmbeddedResourceText(DynamicResourceConstants.Container),
                Validator = ValidationHelper.GetEmbeddedResourceText(ValidatorConstants.Validator_Part1),
                AggregatedValidatorList = new[]
                {
                    ValidationHelper.GetEmbeddedResourceText(ValidatorConstants.Validator_Part2)
                },
                Model = new Dictionary<string, string>()
                {
                    // эти два поля указаны в первом валидаторе (принадлежат главной стаье)
                    {"field_1234", null},
                    {"field_1235", ""},
                    // это поле принадлежит статье-агрегату (указано во втором валидаторе)
                    {"field_1239", "100"},
                }
            };


            var result = ValidationServices.ValidateModel(obj);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotNull(result.Messages);
            Assert.AreEqual(result.Messages.Count, 0, "Validation summary must be empty.");
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(2, result.Result.Errors.Count);

            // из первого валидатора:
            Assert.AreEqual("field_1234", result.Result.Errors[0].Definition.PropertyName);
            Assert.AreEqual("Не может быть null", result.Result.Errors[0].Message);

            // из второго валидатора
            Assert.AreEqual("field_1239", result.Result.Errors[1].Definition.PropertyName);
            Assert.AreEqual("Значение должно быть не более 90", result.Result.Errors[1].Message);

        }

        [TestMethod]
        [TestCategory("ValidationServices: samples")]
        public void Test_TestValidator_Checks_Validation_Of_Aggregated_Models_With_Multiple_Validators()
        {
            var obj = new ValidationParamObject()
            {
                DynamicResource = ValidationHelper.GetEmbeddedResourceText(DynamicResourceConstants.Container),
                Validator = ValidationHelper.GetEmbeddedResourceText(ValidatorConstants.Validator_Part1),
                AggregatedValidatorList = new[]
                {
                    ValidationHelper.GetEmbeddedResourceText(ValidatorConstants.Validator_Part2),
                    ValidationHelper.GetEmbeddedResourceText(ValidatorConstants.Validator_Part3)
                },
                Model = new Dictionary<string, string>()
                {
                    // эти два поля указаны в первом валидаторе (принадлежат главной стаье)
                    {"field_1234", null},
                    {"field_1235", ""},
                    // это поле принадлежит первой статье-агрегату (указано во втором валидаторе)
                    {"field_1239", "100"},
                    // это поле принадлежит второй статье-агрегату (указано в третьем валидаторе)
                    {"field_1240", ""},
                }

            };

            var result = ValidationServices.ValidateModel(obj);

            Assert.IsFalse(result.IsValid);
            Assert.IsNotNull(result.Messages);
            Assert.AreEqual(result.Messages.Count, 0, "Validation summary must be empty.");
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(3, result.Result.Errors.Count);

            // из первого валидатора:
            Assert.AreEqual("field_1234", result.Result.Errors[0].Definition.PropertyName);
            Assert.AreEqual("Не может быть null", result.Result.Errors[0].Message);

            // из второго валидатора:
            Assert.AreEqual("field_1239", result.Result.Errors[1].Definition.PropertyName);
            Assert.AreEqual("Значение должно быть не более 90", result.Result.Errors[1].Message);

            // из третьего валидатора:
            Assert.AreEqual("field_1240", result.Result.Errors[2].Definition.PropertyName);
            Assert.AreEqual("Поле AlternativeName Должно быть заполнено, если не заполнено Name", result.Result.Errors[2].Message);

        }

        #endregion

        #region Тестирование сценариев генерации текста валидатора (включая проверку корректности вводимых данных)
        [TestMethod]
        [TestCategory("ValidationServices: generation")]
        public void Test_GenerateXamlValidatorText_Checks_Deserialization_And_Serialization()
        {
            List<PropertyDefinition> definitions = new List<PropertyDefinition>()
            {
                new PropertyDefinition { Alias = "Name", PropertyName = "field_1234", PropertyType = typeof(string), Description = "Имя" },
                new PropertyDefinition { Alias = "DateOfBirth", PropertyName = "field_1235", PropertyType = typeof(DateTime), Description = "Дата рождения" },
                new PropertyDefinition { Alias = "Age", PropertyName = "field_1236", PropertyType = typeof(int), Description = "Возраст" },
                new PropertyDefinition { Alias = "Regions", PropertyName = "field_1237", PropertyType = typeof(ListOfInt), Description = "Регионы (M2M)" },
            };

            var result = ValidationServices.GenerateXamlValidatorText(definitions);

            Assert.IsNotNull(result);

            // теперь протестируем сгенерированный текст
            ValidationServices.TestValidator(result, null);
        }

        [TestMethod]
        [TestCategory("ValidationServices: generation")]
#if NET_CORE
        [ExpectedException(typeof(TargetInvocationException))]
#else
        [ExpectedException(typeof(XamlObjectWriterException))]
#endif
        public void Test_GenerateXamlValidatorText_Checks_That_PropertyName_Is_Required()
        {
            List<PropertyDefinition> definitions = new List<PropertyDefinition>()
            {
                new PropertyDefinition { Alias = "Name", PropertyName = "", PropertyType = typeof(string), Description = "Имя" },
            };

            var result = ValidationServices.GenerateXamlValidatorText(definitions);
            XamlConfigurationParser.CreateFrom(result);
        }

        [TestMethod]
        [TestCategory("ValidationServices: generation")]
#if NET_CORE
        [ExpectedException(typeof(TargetInvocationException))]
#else
        [ExpectedException(typeof(XamlObjectWriterException))]
#endif
        public void Test_GenerateXamlValidatorText_Checks_That_Alias_Is_Required()
        {
            List<PropertyDefinition> definitions = new List<PropertyDefinition>()
            {
                new PropertyDefinition { Alias = "", PropertyName = "field_1234", PropertyType = typeof(string), Description = "Имя" },
            };

            var result = ValidationServices.GenerateXamlValidatorText(definitions);
            XamlConfigurationParser.CreateFrom(result);
        }

        [TestMethod]
        [TestCategory("ValidationServices: generation")]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_GenerateXamlValidatorText_Checks_That_Alias_Should_Be_Unique()
        {
            List<PropertyDefinition> definitions = new List<PropertyDefinition>()
            {
                new PropertyDefinition { Alias = "Name", PropertyName = "field_1234", PropertyType = typeof(string), Description = "Имя" },
                new PropertyDefinition { Alias = "Name", PropertyName = "field_1235", PropertyType = typeof(string), Description = "Отчество" },
            };

            var result = ValidationServices.GenerateXamlValidatorText(definitions);
            XamlConfigurationParser.CreateFrom(result);
        }

        [TestMethod]
        [TestCategory("ValidationServices: generation")]
#if NET_CORE
        [ExpectedException(typeof(TargetInvocationException))]
#else
        [ExpectedException(typeof(XamlObjectWriterException))]
#endif
        public void Test_GenerateXamlValidatorText_Checks_That_Alias_Consists_Of_Literals()
        {
            List<PropertyDefinition> definitions = new List<PropertyDefinition>()
            {
                new PropertyDefinition { Alias = "Name Name() - 123 !@#$%^&*()_+", PropertyName = "field_1234", PropertyType = typeof(string) },
            };

            var result = ValidationServices.GenerateXamlValidatorText(definitions);
            XamlConfigurationParser.CreateFrom(result);
        }

        [TestMethod]
        [TestCategory("ValidationServices: generation")]
#if NET_CORE
        [ExpectedException(typeof(TargetInvocationException))]
#else
        [ExpectedException(typeof(XamlObjectWriterException))]
#endif
        public void Test_GenerateXamlValidatorText_Checks_That_Alias_Cannot_Start_With_Number()
        {
            List<PropertyDefinition> definitions = new List<PropertyDefinition>()
            {
                new PropertyDefinition { Alias = "1Name", PropertyName = "field_1234", PropertyType = typeof(string), Description = "Имя" },
            };

            var result = ValidationServices.GenerateXamlValidatorText(definitions);
            XamlConfigurationParser.CreateFrom(result);
        }

        [TestMethod]
        [TestCategory("ValidationServices: generation")]
        public void Test_GenerateXamlValidatorText_Checks_That_Alias_Can_Start_With_Underscore()
        {
            List<PropertyDefinition> definitions = new List<PropertyDefinition>()
            {
                new PropertyDefinition { Alias = "_Name", PropertyName = "field_1234", PropertyType = typeof(string), Description = "Имя" },
            };

            var result = ValidationServices.GenerateXamlValidatorText(definitions);

            Assert.IsNotNull(result);
            Debug.WriteLine(result);
            // теперь протестируем сгенерированный текст
            ValidationServices.TestValidator(result, null);
        }

        [TestMethod]
        [TestCategory("ValidationServices: generation")]
        #if NET_CORE
        [ExpectedException(typeof(TargetInvocationException))]
        #else
        [ExpectedException(typeof(XamlObjectWriterException))]
        #endif

        public void Test_GenerateXamlValidatorText_Checks_That_Alias_Cannot_Contain_Spaces()
        {
            List<PropertyDefinition> definitions = new List<PropertyDefinition>()
            {
                new PropertyDefinition { Alias = "_Nam e", PropertyName = "field_1234", PropertyType = typeof(string), Description = "Имя" },
            };

            var result = ValidationServices.GenerateXamlValidatorText(definitions);
            XamlConfigurationParser.CreateFrom(result);
        }

        [TestMethod]
        [TestCategory("ValidationServices: generation")]
        public void Test_GenerateXamlValidatorText_Checks_That_Alias_Can_Contain_Set_Of_Characters()
        {
            List<PropertyDefinition> definitions = new List<PropertyDefinition>()
            {
                new PropertyDefinition { Alias = "_Name123_as12_22aDFGHKLZXCVBNM1_", PropertyName = "field_1234", PropertyType = typeof(string), Description = "Имя" },
            };

            var result = ValidationServices.GenerateXamlValidatorText(definitions);

            Assert.IsNotNull(result);
            Debug.WriteLine(result);
            // теперь протестируем сгенерированный текст
            ValidationServices.TestValidator(result, null);
        }

        /// <summary>
        /// Генерация пустого подключаемого ресурсного словаря
        /// </summary>
        [TestMethod]
        [TestCategory("ValidationServices: generation")]
        public void Test_GenerateEmptyDynamicResourceText_And_Test()
        {
            var result = ValidationServices.GenerateEmptyDynamicResourceText();

            Assert.IsNotNull(result);

            Debug.WriteLine(result);

            // теперь протестируем сгенерированный текст
            ValidationServices.TestResourceDictionary(result);
        }

        [TestMethod]
        [TestCategory("ValidationServices: generation")]
        public void Test_GenerateDynamicResourceText_And_Test()
        {
            // создаем контейнер словарей
            var dynamicResource = new DynamicResourceDictionaryContainer();

            // настраиваем: добавляем ckjdfhb:
            dynamicResource.ResourceDictionaries.Add("First", new DynamicResourceDictionary() { Name = "First" });
            dynamicResource.ResourceDictionaries.Add("Second", new DynamicResourceDictionary() { Name = "Second" });

            // в первый словарь добавляем объекты:
            DynamicResourceDictionary dict;
            dynamicResource.TryGetResourceDictionary("First", out dict);

            //добавим объект типа string
            dict.Resources.Add("MyPhoneNumber", "+7 926 706 48 72");

//            //добавим объекты типа System.Drawing.*
//            dict.Resources.Add("MyLocation", new System.Drawing.Point { X = 554400, Y = 373000 });
//            dict.Resources.Add("MyFavoriteColor", System.Drawing.Color.AliceBlue);

            // добавим объект из этой сборки
            dict.Resources.Add("MyInfo", new Xaml.Tests.Model.Person
            {
                Age = 12,
                Date = DateTime.Now,
                DuplicateName = "Nick",
                Name = "Nick",
                Passport = "1234 567890"
            });

            // генерируем xaml-текст такого словаря:
            var result = ValidationServices.GenerateDynamicResourceText(dynamicResource);

            Assert.IsNotNull(result);
            Debug.WriteLine(result);

            // теперь протестируем сгенерированный текст
            ValidationServices.TestResourceDictionary(result);
        }
        #endregion

        /// <summary>
        /// Проверяем, правильно ли  работрают в связке методы генерации xaml и получение списка описаний полей для xaml
        /// </summary>
        [TestMethod]
        [TestCategory("ValidationServices: test validators")]
        public void Test_GetPropertyDefinitions()
        {
            // Шаг 1
            // Генерируем текст xaml-валидатора со списком полей
            List<PropertyDefinition> definitions = new List<PropertyDefinition>()
            {
                new PropertyDefinition { Alias = "Name", PropertyName = "field_1234", PropertyType = typeof(string), Description = "Имя" },
                new PropertyDefinition { Alias = "DateOfBirth", PropertyName = "field_1235", PropertyType = typeof(DateTime), Description = "Дата рождения" },
                new PropertyDefinition { Alias = "Age", PropertyName = "field_1236", PropertyType = typeof(int), Description = "Возраст" },
                new PropertyDefinition { Alias = "Regions", PropertyName = "field_1237", PropertyType = typeof(ListOfInt), Description = "Регионы (M2M)" },
            };

            var result = ValidationServices.GenerateXamlValidatorText(definitions);

            Assert.IsNotNull(result);

            // Шаг 2
            // Получаем список полей для сгенерированного описания xaml-валидатора
            var resultingDefinitions = ValidationServices.GetPropertyDefinitions(result, null);

            Assert.IsNotNull(resultingDefinitions);
            Assert.AreEqual(definitions.Count, resultingDefinitions.Length, "Коллекции не совпадают.");

            for (int i = 0; i < definitions.Count; i++)
            {
                Assert.AreEqual(definitions[i], resultingDefinitions[i]);
            }
        }
    }
}
