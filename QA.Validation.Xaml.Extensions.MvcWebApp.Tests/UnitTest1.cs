using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QA.Validation.Extensions.Tests;
using QA.Validation.Xaml.Extensions.MvcWebApp.Controllers;
using QA.Validation.Xaml.Extensions.MvcWebApp.Tests.Stubs;
using QA.Validation.Xaml.Extensions.Rules;
using QA.Validation.Xaml.Extensions.Rules.Remote;

namespace QA.Validation.Xaml.Extensions.MvcWebApp.Tests
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Тест вызывает  Index e RemoteValidationController на фальшивых объектах
        /// </summary>
        [TestMethod]
        [TestCategory("Remote validation: fixture")]
        public void Test_RemoteValidation_Check_Interaction_With_Mock_Mvc4_App()
        {
            // подготовим и настроим все объекты
            var validator = PrepareDefinitions(new ProcessRemoteValidationIf(new 
                MvcWebInteractionManager<RemoteValidationController>(c => c.Index)), 
                "http://localhost:51380/remotevalidation");

            var values = new Dictionary<string, string> { 
                { "prop1", "" }, 
                { "prop2", "12" },
                { "prop3", "" },
                { "prop4", "99.99" },
                { "prop5", "" } 
            };

            var ctx = new ValidationContext();

            validator.Validate(values, ctx);

            Assert.IsFalse(ctx.IsValid);
            Assert.AreEqual(1, ctx.Messages.Count);
            Assert.AreEqual(1, ctx.Result.Errors.Count);

            Assert.AreEqual(ctx.Messages[0], "Тестовое сообщение");

            Assert.AreEqual(ctx.Result.Errors[0].Definition.PropertyName, "prop3");
            Assert.AreEqual(ctx.Result.Errors[0].Message, "Укажите дату");
        }

        private static XamlValidator PrepareDefinitions(ProcessRemoteValidationIf condition, string url)
        {
            var validator = new XamlValidator();
            validator.Definitions.Add("prop1", new PropertyDefinition("prop1", "prop1", typeof(string)));
            validator.Definitions.Add("prop2", new PropertyDefinition("prop2", "prop2", typeof(int)));
            validator.Definitions.Add("prop3", new PropertyDefinition("prop3", "prop3", typeof(DateTime)));
            validator.Definitions.Add("prop4", new PropertyDefinition("prop4", "prop4", typeof(double)));
            validator.Definitions.Add("prop5", new PropertyDefinition("prop5", "prop5", typeof(bool)));

            condition.HttpMethod = "POST";
            condition.SiteId = 35;
            condition.CustomerCode = "QP123";
            condition.Timeout = 10000;
            condition.Url = new Uri(url);
            condition.DefinitionsToSend.AddRange(validator.Definitions.Values);

            validator.ValidationRules.Add(condition);
            return validator;
        }
    }
}
