using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Xaml;
using QA.Validation.Xaml.Core;
using QA.Validation.Xaml.Initialization;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Основные методы для работы с валидацией
    /// </summary>
    public static class ValidationServices
    {
        private static Lazy<ValidationManager> Manager = new Lazy<ValidationManager>(
            () => new ValidationManager(),
            LazyThreadSafetyMode.ExecutionAndPublication);

        #region ValidateModel
        /// <summary>
        /// Валидация данных.
        /// </summary>
        /// <param name="model">Словарь имя поля - текстовое значение. Массивы string[] и int[] представлять как "12, 23, 12"</param>
        /// <param name="validator">Основное валидатор контента</param>
        /// <param name="aggregatedValidatorList">Валидаторы для агрегированных статей</param>
        /// <param name="dynamicResource">Ресурсный словарь</param>
        /// <param name="connection">текущее подключение к БД</param>
        /// <returns>Объект с результатами валидации</returns>
        public static ValidationContext ValidateModel(Dictionary<string, string> model,
            string validator,
            string[] aggregatedValidatorList,
            string dynamicResource,
            SqlConnection connection)
        {
            var serviceProvider = new ConfigurationProvider();

            if (connection != null)
                serviceProvider[typeof(SqlConnection)] = connection;

            return Manager.Value.ValidateModel(model, validator, aggregatedValidatorList, dynamicResource, serviceProvider);
        }

        /// <summary>
        /// Валидация данных.
        /// </summary>
        /// <param name="model">Словарь имя поля - текстовое значение. Массивы string[] и int[] представлять как "12, 23, 12"</param>
        /// <param name="validator">Основное валидатор контента</param>
        /// <param name="aggregatedValidatorList">Валидаторы для агрегированных статей</param>
        /// <param name="dynamicResource">Ресурсный словарь</param>
        /// <returns>Объект с результатами валидации</returns>
        public static ValidationContext ValidateModel(Dictionary<string, string> model,
            string validator,
            string[] aggregatedValidatorList,
            string dynamicResource)
        {
            return ValidateModel(model, validator, aggregatedValidatorList, dynamicResource, null);
        }

        /// <summary>
        /// Валидация данных.
        /// </summary>
        /// <param name="model">Словарь имя поля - текстовое значение. Массивы string[] и int[] представлять как "12, 23, 12"</param>
        /// <param name="validator">Основное валидатор контента</param>
        /// <param name="dynamicResource">Ресурсный словарь</param>
        /// <returns>Объект с результатами валидации</returns>
        public static ValidationContext ValidateModel(Dictionary<string, string> model,
            string validator,
            string dynamicResource)
        {
            return ValidateModel(model, validator, new string[] { }, dynamicResource);
        }

        /// <summary>
        /// Валидация данных.
        /// </summary>
        /// <param name="model">Словарь имя поля - текстовое значение. Массивы string[] и int[] представлять как "12, 23, 12"</param>
        /// <param name="validator">Основное валидатор контента</param>
        /// <param name="connection">текущее подключение к БД</param>
        /// <param name="dynamicResource">Ресурсный словарь</param>
        /// <returns>Объект с результатами валидации</returns>
        public static ValidationContext ValidateModel(Dictionary<string, string> model,
            string validator,
            string dynamicResource, SqlConnection connection)
        {
            return ValidateModel(model, validator, new string[] { }, dynamicResource, connection);
        } 
        #endregion

        #region TestValidator
        /// <summary>
        /// Проверяет корректность валидатора.
        /// Данный метод можно использовать для начальной проверки корректности синтаксиса Xaml, 
        /// корректности пространств имен используемых типов.
        /// Необходимо передавать ресурный словарь.
        /// </summary>
        /// <param name="validatorText">текст с xaml-описанием валидатора</param>
        /// <param name="dynamicResourceText">ресурсный словарь</param>
        public static void TestValidator(string validatorText, string dynamicResourceText)
        {
            Manager.Value.TestValidator(validatorText, dynamicResourceText);
        }

        /// <summary>
        /// Создает экземпляр валидатора нетипизированных моделей. И валидирует модель с пустыми значениями.
        /// Данный метод можно использовать для проверки корректности логики валидатора.
        /// </summary>
        /// <param name="validatorText">текст с xaml-описанием валидатора</param>
        /// <param name="model">Словарь со всеми полями, участвующими в валидации. Значения должны быть пустыми</param>
        /// <param name="dynamicResourceText">ресурсный словарь</param>
        public static void TestValidator(Dictionary<string, string> model, string validatorText, string dynamicResourceText)
        {
            TestValidator(model, validatorText, dynamicResourceText, null);
        }

        /// <summary>
        /// Создает экземпляр валидатора нетипизированных моделей. И валидирует модель с пустыми значениями.
        /// Данный метод можно использовать для проверки корректности логики валидатора.
        /// </summary>
        /// <param name="validatorText">текст с xaml-описанием валидатора</param>
        /// <param name="model">Словарь со всеми полями, участвующими в валидации. Значения должны быть пустыми</param>
        /// <param name="dynamicResourceText">ресурсный словарь</param>
        /// <param name="connection">текущее подключение к БД</param>
        public static void TestValidator(Dictionary<string, string> model, string validatorText, string dynamicResourceText, SqlConnection connection)
        {
            var serviceProvider = new ConfigurationProvider();

            if (connection != null)
                serviceProvider[typeof(SqlConnection)] = connection;

            Manager.Value.TestValidator(model, validatorText, dynamicResourceText, serviceProvider);
        
        }
        #endregion Manager.Value.TestValidator(model, validatorText, dynamicResourceText);

        /// <summary>
        /// Проверяет корректность ресурсного словаря.
        /// <param name="dynamicResourceText">Текст xaml c описанием словаря</param>
        /// </summary>
        public static void TestResourceDictionary(string dynamicResourceText)
        {
            Manager.Value.TestResourceDictionary(dynamicResourceText);
        }

        /// <summary>
        /// Генерация пустого xaml-валидатора с указанными полями и их типами
        /// </summary>
        /// <param name="definitions"></param>
        /// <returns></returns>
        public static string GenerateXamlValidatorText(IEnumerable<PropertyDefinition> definitions)
        {
            return Manager.Value.GenerateXamlValidatorText(definitions);
        }

        /// <summary>
        /// Генерация текста пустого подключаемого ресурсного словаря
        /// </summary>
        /// <returns></returns>
        public static string GenerateEmptyDynamicResourceText()
        {
            return Manager.Value.GenerateDynamicResourceText(null);
        }

        /// <summary>
        /// Генерация текста подключаемого ресурсного словаря
        /// </summary>
        /// <param name="definitions"></param>
        /// <returns></returns>
        public static string GenerateDynamicResourceText(DynamicResourceDictionaryContainer dictionary)
        {
            return Manager.Value.GenerateDynamicResourceText(dictionary);
        }

        /// <summary>
        /// Получение списка полей, использующихся в валидаторе.
        /// Если валидатор некорректен, выкидывается исключение. 
        /// Данный метод уже включает проверку XamlSerices.TestValidator.
        /// </summary>
        /// <param name="validatorText"></param>
        /// <param name="dynamicResourceText"></param>
        /// <returns></returns>
        public static PropertyDefinition[] GetPropertyDefinitions(string validatorText, string dynamicResourceText)
        {
            return Manager.Value.GetPropertyDefinitions(validatorText, dynamicResourceText);
        }
    }
}
