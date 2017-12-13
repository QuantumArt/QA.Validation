using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Validation.Xaml.Extensions.ValueArguments;
using QA.Validation.Xaml.Tests.Util;
using QA.Validation.Xaml.ListTypes;

namespace QA.Validation.Xaml.Tests
{
    [TestClass]
    public class ValueArgumentTest
    {
        static ValueArgumentTest()
        {
            var enc = new Encode();
            System.Diagnostics.Trace.Write(enc.ToString());
        }

        [TestMethod]
        [TestCategory("ValueArgumentTest")]
        public void Test_setting_value()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", "" }
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.ValueArguments.Example_000);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.AreEqual("new value", model["field_1234"]) ;
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


        [TestMethod]
        [TestCategory("ValueArgumentTest")]
        public void Basic_extract_with_regular_expression_and_apply_to_another()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", "PE1234.123" },
                    { "field_1235", "" },
                    { "field_1236", "" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.ValueArguments.Example_002);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.IsValid, "The model should be valid");
            Assert.AreEqual("PE1234", model["field_1235"], "global code should be extracted");
            Assert.AreEqual("123", model["field_1236"], "version should be extracted");
        }

        [TestMethod]
        [TestCategory("ValueArgumentTest")]
        public void Basic_extract_shuldnot_fail_on_empty_value()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", "" },
                    { "field_1235", "" },
                    { "field_1236", "" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.ValueArguments.Example_002);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.IsValid, "The model should be valid");
            Assert.AreEqual("", model["field_1235"], "global code should be empty");
            Assert.AreEqual("", model["field_1236"], "version should be empty");
        }


        [TestMethod]
        [TestCategory("ValueArgumentTest")]
        public void Basic_extract_shuldnot_fail_on_null()
        {
            var model = new Dictionary<string, string>()
                {
                    { "field_1234", null },
                    { "field_1235", "" },
                    { "field_1236", "" },
                };

            var validator = ValidationHelper.GetXaml<XamlValidator>(ValidatorConstants.ValueArguments.Example_002);
            var context = new ValidationContext();

            validator.Validate(model, context);

            Assert.IsTrue(context.IsValid, "The model should be valid");
            Assert.AreEqual("", model["field_1235"], "global code should be empty");
            Assert.AreEqual("", model["field_1236"], "version should be empty");
        }

        #region Setting of Values
        [TestMethod]
        [TestCategory("ValueArgumentTest")]
        public void Test_Setting_Of_Fields_string()
        {
            var actual = Run<string>("text");
            Assert.AreEqual("text", actual);
        }



        [TestMethod]
        [TestCategory("ValueArgumentTest")]
        public void Test_Setting_Of_Fields_null_value()
        {
            Assert.AreEqual("", Run<string>(null), "string");
            Assert.AreEqual("", Run<bool?>(null), "bool");
            Assert.AreEqual("", Run<int>(null), "int");
            Assert.AreEqual("", Run<double>(null), "double");
            Assert.AreEqual("", Run<ListOfInt>(null), "ListOfInt");
        }

        [TestMethod]
        [TestCategory("ValueArgumentTest")]
        public void Test_Setting_Of_Fields_double()
        {
            var actual = Run<double>(12.123);
            Assert.AreEqual("12.123", actual);
        }

        [TestMethod]
        [TestCategory("ValueArgumentTest")]
        public void Test_Setting_Of_Fields_int()
        {
            var actual = Run<Int32>(123);
            Assert.AreEqual("123", actual);
        }

        [TestMethod]
        [TestCategory("ValueArgumentTest")]
        public void Test_Setting_Of_Fields_ListOfInt()
        {
            var list = new ListOfInt { 1, 2, 3, 4 };
            var actual = Run<ListOfInt>(list);
            Assert.AreEqual("1,2,3,4", actual);
        }


        [TestMethod]
        [TestCategory("ValueArgumentTest")]
        public void Test_Setting_Of_Fields_bool()
        {
            var actual = Run<bool>(true);
            Assert.AreEqual("1", actual);

            actual = Run<bool>(false);
            Assert.AreEqual("0", actual);

        }
        #endregion

       // [TestMethod]
       // [TestCategory("Lambda test")]
        public void Test_lambda_all_args()
        {
            var dict = new Dictionary<string, Type> 
            { 
                { "prop1", typeof(string) },
            };

            var validator = Create(dict, "ctx != null && prop1 != null");
            var ctx = new ValidationContext();
            validator.Validate(GetModel(dict, "123"), ctx);

            Assert.IsTrue(ctx.IsValid);
        }

        #region Helpers
        private static object Run<T>(object expected)
        {
            Dictionary<string, string> model = null;
            model = new Dictionary<string, string>() { { "field_01", "" } };

            var validator = new XamlValidator();

            CreateDefinition(validator, "field_01", typeof(T), expected);

            var context = new ValidationContext();
            validator.Validate(model, context);

            var actual = model["field_01"];
            return actual;
        }

        private Dictionary<string, string> GetModel(Dictionary<string, Type> prop, params string[] values)
        {
            var dict = new Dictionary<string, string>();
            var names = prop.Keys.ToArray();
            for (int i = 0; i < prop.Count; i++)
            {
                dict[names[i]] = values[i];
            }
            return dict;
        }

        private static XamlValidator Create(Dictionary<string, Type> props, string lambda)
        {
            var validator = new XamlValidator();
            foreach (var item in props)
            {
                var def = new PropertyDefinition(item.Key, item.Key, item.Value);
                validator.Definitions.Add(def.Alias, def);
            }
            validator.ValidationRules.Add(new ForMember
            {
                Definition = validator.Definitions.First().Value,
                Condition = new If
                {
                   // Condition = new Lambda { Expression = lambda },
                    Then = new WithMessage { Text = "then" },
                    Else = new WithMessage { Text = "else" }
                }
            });

            return validator;
        }

        private static PropertyDefinition CreateDefinition(XamlValidator validator, string pn, Type pt, object valueToSet)
        {
            var pd = new PropertyDefinition { Alias = pn, PropertyName = pn, PropertyType = pt };
            validator.Definitions.Add(pn, pd);

            validator.ValidationRules.Add(new ForMember()
            {
                Definition = pd,
                Condition =
                    new ApplyValue()
                    {
                        Value = valueToSet
                    }
            });

            return pd;
        }
        #endregion
    }
}
