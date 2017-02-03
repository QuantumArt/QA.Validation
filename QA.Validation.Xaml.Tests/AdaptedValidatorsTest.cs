using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Validation.Xaml.Tests.Model;
using QA.Validation.Xaml.Tests.Typed;

namespace QA.Validation.Xaml.Tests
{
    [TestClass]
    public class AdaptedValidatorsTest
    {
        /// <summary>
        /// Тест типизированного валидатора, построенного на базе валидатора словарей
        /// </summary>
        [TestMethod]
        [TestCategory("AdaptedValidator")]
        public void TestAdaptedValidator_001()
        {
            var culture = CultureInfo.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-us");

            var validator = new AdaptedPersonValidator();//.CreateInstance();
            var ctx = new ValidationContext();
            validator.Validate(new Person
            {
                Name = "Ivan",
                DuplicateName = ""
            }, ctx);

            Assert.IsFalse(ctx.IsValid);
            Assert.AreEqual(1, ctx.Messages.Count);

            Assert.AreEqual("Must be equal.", ctx.Messages[0]);
            Assert.AreEqual("DuplicateName", ctx.Result.Errors[0].Definition.PropertyName);

            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        [TestCategory("AdaptedValidator")]
        public void TestAdaptedValidator_002()
        {
            var validator = new AdaptedPersonValidator();
            var ctx = new ValidationContext();
            validator.Validate(new Person
            {
                Name = "Ivan",
                DuplicateName = "Ivan"
            }, ctx);

            Assert.IsTrue(ctx.IsValid);
        }
    }
}
