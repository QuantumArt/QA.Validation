using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Validation.Xaml.Tests.Util;

namespace QA.Validation.Xaml.Tests
{
    [TestClass]
    public class Issues
    {
        #region Equals condition issue
        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Equals_Condition_0001()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", "true" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Equals_Converter_Boolean);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.IsValid);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Equals_Condition_0002()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", "false" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Equals_Converter_Boolean);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.AreEqual("must be true", context.Result.Errors[0].Message);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Equals_Condition_0003()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", "123" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Equals_Converter_Int32);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.IsValid);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Equals_Condition_0004()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", "124" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Equals_Converter_Int32);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.AreEqual("must be 123", context.Result.Errors[0].Message);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Equals_Condition_0005()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", "" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Equals_Converter_Int32);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.AreEqual("must be 123", context.Result.Errors[0].Message);
        }
        #endregion

        #region GreaterThan

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_GreaterThan_Condition_0001()
        {
            var model = new Dictionary<string, string>()
                {
                    { "String_Field", "12" },
                    { "Int32_Field", "12" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.GreaterThan);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.AreEqual("not greater than 99", context.Result.Errors[0].Message);
            Assert.AreEqual("String_Field", context.Result.Errors[0].Definition.PropertyName);

            Assert.AreEqual("not greater than 99", context.Result.Errors[1].Message);
            Assert.AreEqual("Int32_Field", context.Result.Errors[1].Definition.PropertyName);

        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_GreaterThan_Condition_0002()
        {
            var model = new Dictionary<string, string>()
                {
                    { "String_Field", "122" },
                    { "Int32_Field", "122" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.GreaterThan);
            var context = new ValidationContext();

            validator.Validate(model, context);

            // сравнение как строки
            Assert.AreEqual("not greater than 99", context.Result.Errors[0].Message);
            Assert.AreEqual("String_Field", context.Result.Errors[0].Definition.PropertyName);

            // сравнение как числа
            Assert.AreEqual("greater than 99", context.Result.Errors[1].Message);
            Assert.AreEqual("Int32_Field", context.Result.Errors[1].Definition.PropertyName);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_GreaterThan_Condition_0003()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field1", "2012.03.03 18:12:33" },
                    { "field2", "2012.01.03 18:12:33" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.GreaterThan_Two_Properties);
            var context = new ValidationContext();

            validator.Validate(model, context);

            // сравнение как строки
            Assert.AreEqual("true", context.Result.Errors[0].Message);
            Assert.AreEqual("field1", context.Result.Errors[0].Definition.PropertyName);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_GreaterThan_Condition_0004()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field1", "2012.03.03 18:12:33" },
                    { "field2", "2012.03.03 18:12:34" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.GreaterThan_Two_Properties);
            var context = new ValidationContext();

            validator.Validate(model, context);

            // сравнение как строки
            Assert.AreEqual("false", context.Result.Errors[0].Message);
            Assert.AreEqual("field1", context.Result.Errors[0].Definition.PropertyName);
        }

        #endregion

        #region bool converting

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Boolean_From_Digit_One()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", "1" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Equals_Converter_Boolean);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.IsValid);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Boolean_From_Didgit_Zero()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", "0" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Equals_Converter_Boolean);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.AreEqual("must be true", context.Result.Errors[0].Message);
        }
        #endregion

        #region Caching of validators issue

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Caching_Resource_Dictionary()
        {
            lock (typeof(DynamicResourceDictionaryContainer))
            {
                var model = new Dictionary<string, string>()
                {
                    { "field_1234", "" },
                };

                // генерируем словари с разными текстами
                string text1 = "test text 1";
                string text2 = "test text 2";

                var templateText = ValidationHelper.GetEmbeddedResourceText(ValidatorConstants.Issues.TestMessageTemplate);
                var resource1 = String.Format(templateText, text1);
                var resource2 = String.Format(templateText, text2);

                var validatorText = ValidationHelper.GetEmbeddedResourceText(ValidatorConstants.Issues.Uses_DynamicResource);

                var result1 = ValidationServices.ValidateModel(model, validatorText, resource1);
                var result2 = ValidationServices.ValidateModel(model, validatorText, resource2);

                Assert.IsFalse(result1.IsValid);
                Assert.IsTrue(result1.Messages.Count == 1);
                Assert.AreEqual(text1, result1.Messages[0]);

                Assert.IsFalse(result2.IsValid);
                Assert.IsTrue(result2.Messages.Count == 1);
                Assert.AreEqual(text2, result2.Messages[0]);
            }
        }

        private static string CreateDictionaryText(string message)
        {
            #region Генерируем словарь
            // создаем контейнер словарей
            var dynamicResource = new DynamicResourceDictionaryContainer();

            // настраиваем: добавляем ckjdfhb:
            dynamicResource.ResourceDictionaries.Add("Messages", new DynamicResourceDictionary() { Name = "Messages" });

            // в первый словарь добавляем объекты:
            DynamicResourceDictionary dict = null;
            dynamicResource.TryGetResourceDictionary("Messages", out dict);

            //добавим объект типа string
            dict.Resources.Add("test", new WithMessage() { Text = message });
            #endregion

            // генерируем xaml-текст такого словаря:
            var resource1 = ValidationServices.GenerateDynamicResourceText(dynamicResource);
            return resource1;
        }
        #endregion

        #region Matches&MatchesForEachLine

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_MatchesForEachLine_01()
        {
            var model = new Dictionary<string, string>()
                {
                    { "String_Field",
@"12345
123445
345345345" },
          {"String_Field2", @""},
          {"String_Field3", @""},
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.MatchesForEachLine);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.IsValid);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_MatchesForEachLine_Allow_Empty_Strings()
        {
            var model = new Dictionary<string, string>()
                {
                    { "String_Field2",
@"12345

123445
345345345"},
          {"String_Field", @""},
          {"String_Field3", @""},
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.MatchesForEachLine);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.IsValid);
        }

        //[TestMethod]
        //[TestCategory("Issues")]
        public void Issue_MatchesForEachLine_Empty_Strings_Not_Allowed()
        {
            var model = new Dictionary<string, string>()
                {
                    { "String_Field",
@"12345

123445
345345345"},
          {"String_Field2", @""},
          {"String_Field3", @""},
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.MatchesForEachLine);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_MatchesForEachLine_Trim()
        {
            var model = new Dictionary<string, string>()
                {
                    { "String_Field3",
@" 12345
    123445
345345345"},
          {"String_Field", @""},
          {"String_Field2", @""},
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.MatchesForEachLine);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.IsValid);

        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_MatchesForEachLine_Meta()
        {
            var model = new Dictionary<string, string>()
                {
                    { "String_Field",
@"<meta name=""123"" content=""3456"" />
<meta name=""133"" content=""3456"" />"}
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.MatchesForEachLineMeta);
            var context = new ValidationContext();

            validator.Validate(model, context);
            Assert.IsTrue(context.IsValid);

            model = new Dictionary<string, string>()
                {
                    { "String_Field",
@"        <meta name=""123"" content=""3456"" />


<meta name=""133"" content=""3456"" />"}
                };

            context = new ValidationContext();

            validator.Validate(model, context);
            Assert.IsTrue(context.IsValid);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_MatchesForEachLine_Spaces_Not_Allowed()
        {
            var model = new Dictionary<string, string>()
                {
                    { "String_Field",
@" 12345
    123445
345345345"},
          {"String_Field3", @""},
          {"String_Field2", @""},
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.MatchesForEachLine);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_MatchesForEachLine_Allow_SingleLine()
        {
            var model = new Dictionary<string, string>()
                {
                    { "String_Field", @"12345" },
                    { "String_Field2", @"" },
                    {"String_Field3", @""},
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.MatchesForEachLine);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.IsValid);
        }
        #endregion


        #region Length
        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Length_MinLength_invalid()
        {
            var model = new Dictionary<string, string>()
                {
                    { "String_Field", "92" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Length_MinLength);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.AreEqual(1, context.Result.Errors.Count);
            Assert.AreEqual("MinLength=10 else", context.Result.Errors[0].Message);
            Assert.AreEqual("String_Field", context.Result.Errors[0].Definition.PropertyName);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Length_MinLength_exact()
        {
            var model = new Dictionary<string, string>()
                {
                    { "String_Field", "1234567890" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Length_MinLength);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.AreEqual(1, context.Result.Errors.Count);
            Assert.AreEqual("MinLength=10 then", context.Result.Errors[0].Message);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Length_MinLength_valid()
        {
            var model = new Dictionary<string, string>()
                {
                    { "String_Field", "1234567890111" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Length_MinLength);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.AreEqual(1, context.Result.Errors.Count);
            Assert.AreEqual("MinLength=10 then", context.Result.Errors[0].Message);
        }


        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Length_MinLength_Arrays_valid()
        {
            var model = new Dictionary<string, string>()
                {
                    { "ints", "1,2" },
                    { "strings", "a11,aa2,as3" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Length_Lists);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.AreEqual(2, context.Result.Errors.Count);
            Assert.AreEqual("OK", context.Result.Errors[0].Message);
            Assert.AreEqual("OK", context.Result.Errors[1].Message);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Length_MinLength_Arrays_invalid()
        {
            var model = new Dictionary<string, string>()
                {
                    { "ints", "1,2,3,1,2" },
                    { "strings", "asd" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Length_Lists);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.AreEqual(2, context.Result.Errors.Count);
            Assert.AreEqual("OK", context.Result.Errors[0].Message);
            Assert.AreEqual("FAIL", context.Result.Errors[1].Message);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Length_MinLength_Arrays1_valid()
        {
            var model = new Dictionary<string, string>()
                {
                    { "ints", "1,2,3,4" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Length_Lists_MinLength);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.AreEqual(1, context.Result.Errors.Count);
            Assert.AreEqual("OK", context.Result.Errors[0].Message);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Length_MinLength_Arrays1_invalid()
        {
            var model = new Dictionary<string, string>()
                {
                    { "ints", "1,2" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Length_Lists_MinLength);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.AreEqual(1, context.Result.Errors.Count);
            Assert.AreEqual("FAIL", context.Result.Errors[0].Message);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_Length_MinLength_Arrays1_invalid_empty()
        {
            var model = new Dictionary<string, string>()
                {
                    { "ints", "" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Length_Lists_MinLength);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsFalse(context.IsValid);
            Assert.AreEqual(1, context.Result.Errors.Count);
            Assert.AreEqual("FAIL", context.Result.Errors[0].Message);
        }
        #endregion


        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_40666_ok()
        {
            //<PropertyDefinition Alias="Year" Description="Year" PropertyName="field_173111" PropertyType="x:Int32" />
            //<PropertyDefinition Alias="SortOrder" Description="SortOrder" PropertyName="field_173112" PropertyType="x:Int32"/>

            var model = new Dictionary<string, string>()
                {
                    { "field_173111", "2015" },
                    { "field_173112", "12" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Issue_40666);
            var context = new ValidationContext();

            validator.Validate(model, context);
        }

        [TestMethod]
        [TestCategory("Issues")]
        public void Issue_40666_should_not_cat_to_boolean_in_case_of_1()
        {
            //<PropertyDefinition Alias="Year" Description="Year" PropertyName="field_173111" PropertyType="x:Int32" />
            //<PropertyDefinition Alias="SortOrder" Description="SortOrder" PropertyName="field_173112" PropertyType="x:Int32"/>

            var model = new Dictionary<string, string>()
                {
                    { "field_173111", "2015" },
                    { "field_173112", "1" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.Issues.Issue_40666);
            var context = new ValidationContext();

            validator.Validate(model, context);
        }

    }


}
