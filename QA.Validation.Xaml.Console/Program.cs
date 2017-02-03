using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using QA.Configuration;

namespace QA.Validation.Xaml.Console
{
    using System;
    using System.Diagnostics;
    using QA.Core;
    using QA.Validation.Xaml.Console.ValidationRules;

    class Program
    {
        const string Basic_Xaml = "QA.Validation.Xaml.Console.ValidationRules.Basic.xaml";
        const string Advanced_Xaml = "QA.Validation.Xaml.Console.ValidationRules.Advanced.xaml";
        const string LookupValidator_Xaml = "QA.Validation.Xaml.Console.ValidationRules.LookupValidator.xaml";
        const string Messages_Xaml = "QA.Validation.Xaml.Console.DynamicResources.Messages.xaml";

        static void Main(string[] args)
        {

            var type = typeof(Person);
            string propertyName = "Name";

            var accessor = new FastPropertyAccessor(type, propertyName);

            Stopwatch st = new Stopwatch();
            st.Start();
            for (int i = 0; i < 1000000; i++)
            {
                var person = new Person() { Name = "1234" };
                var prop1 = accessor.GetValue(person);
            }

            Console.WriteLine(st.ElapsedMilliseconds);

            XamlObjectValidator validator = null;

            // получаем словарь
            using (var stream = Assembly.GetExecutingAssembly()
               .GetManifestResourceStream(Messages_Xaml))
            {
                var dict = (DynamicResourceDictionaryContainer)XamlConfigurationParser.CreateFrom(stream);
                DynamicResourceDictionaryContainer.SetResourceProvider(() => dict);
            }

            // получаем xaml-файл из сборки
            using (var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(Advanced_Xaml))
            {
                // создаем экземпляр валидатора
                validator = (XamlObjectValidator)XamlConfigurationParser.CreateFrom(stream);
            }

            // еще один вариант создать валидатор:

            var basicValidator = new BasicPersonValidator();

            // валидируем набор объектов

            /**
             * 1) var ctx = new ValidationContext();
             * 
             * 2) validator.Validate(objectToValidate, ctx);
             * 
             * 3) ctx.IsValid, ctx.Result, ctx.Errors, ...
             * */

            ValidateObjects(validator);

            /**
             * Другой подход - валидация словаря <PropertyName, PropertyValue>
             * 
             * 1) var ctx = new ValidationContext();              
             * 2) validator.Validate(dictionaryToValidate, ctx);              
             * 3) ctx.IsValid, ctx.Result, ctx.Errors, ...
             * */

            // получаем xaml-файл из сборки
            using (var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(LookupValidator_Xaml))
            {
                // создаем экземпляр валидатора
                var dValiudator = (XamlValidator)XamlConfigurationParser.CreateFrom(stream);
                var ctx = new ValidationContext();

                var person = new Dictionary<string, string>() 
                { 
                    { "Name", "Ivan" },
                    { "DuplicateName", "Ivan1" },
                    { "Age", "18" },
                    { "Passport", "" },
                    { "Date", DateTime.Now.ToString() },
                };

                dValiudator.Validate(person, ctx);
            }

            NLogLogger logger = new NLogLogger("nlogclient.config");
            logger.Info("123");
            logger.ErrorException("exception", new Exception("exc", new StackOverflowException("SckOverflowException", new InvalidOperationException("invaliud"))));

        }

        private static void ValidateObjects(IObjectValidator validator)
        {
            ValidateAndPrint(validator, new Person { Name = "qwerty", DuplicateName = "qwerty", Age = 17, Passport = "1234 123456", Date = DateTime.Now });
            ValidateAndPrint(validator, new Person { Name = "qwerty", DuplicateName = "qwerty1", Age = 17 });
            ValidateAndPrint(validator, new Person { DuplicateName = "qwerty1", Age = 17 });
            ValidateAndPrint(validator, new Person { Name = "qwerty", DuplicateName = "qwerty1", Age = 17, Passport = "1234 12346" });
            ValidateAndPrint(validator, new Person { Age = 2 });
            ValidateAndPrint(validator, new Person { Name = "Ivan", Age = 99 });

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("fr-fr");
            ValidateAndPrint(validator, new Person { Name = "qwerty1", Age = 12, DuplicateName = "qwerty1" });

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-us");
            ValidateAndPrint(validator, new Person { Name = "qwerty1", Age = 12, DuplicateName = "qwerty1" });

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("ru-ru");
            ValidateAndPrint(validator, new Person { Name = "qwerty1", Age = 12, DuplicateName = "qwerty1" });
        }

        private static void ValidateAndPrint(IObjectValidator validator, Person person1)
        {
            var ctx = new ValidationContext();
            validator.Validate(person1, ctx);

            Console.WriteLine("*******************************************************************");
            Console.WriteLine(QA.Core.ObjectDumper.DumpObject(person1));
            Console.WriteLine("IsValid: {0}", ctx.IsValid);
            Console.WriteLine("Messages: ", ctx.IsValid);

            foreach (var item in ctx.Messages)
            {
                Console.WriteLine("\t\"{0}\"", item);
            }

            Console.WriteLine("Errors:", ctx.IsValid);

            foreach (var item in ctx.Result.Errors)
            {
                Console.WriteLine("\t{0} : \"{1}\"",
                    item.Definition.PropertyName,
                    item.Message);
            }
        }



    }
}
