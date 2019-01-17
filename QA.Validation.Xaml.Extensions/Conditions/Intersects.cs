using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Markup;
using Portable.Xaml.Markup;
using QA.Validation.Xaml.ListTypes;
using QA.Validation.Xaml.RuleConditions;

namespace QA.Validation.Xaml.Extensions.Conditions
{
    /// <summary>
    /// Условие валидации, проверяющее, еслить ли повторы в коллекции
    /// На вход принимает любые типы, реализующие IEnumerable
    /// </summary>
    [ContentProperty("Value")]
    public class Intersects : SqlCondition
    {
        /// <summary>
        /// Признак того, что правило добавляет найденные ошибки валидации в виде текста
        /// </summary>
        public bool ReportErrors { get; set; }

        /// <summary>
        /// Поле, для которого проверяется условие валидации (его значение используют валидаторы)
        /// </summary>
        public PropertyDefinition GroupBy { get; set; }

        /// <summary>
        /// Поле, с которым производится сравнение.
        /// </summary>
        public PropertyDefinition Target { get; set; }

        protected override bool OnValidating(ValidationConditionContext context)
        {
            var source = Source ?? context.Definition;

            if (source == null)
                throw new ArgumentNullException("Source");

            if (Target == null)
                throw new ArgumentNullException("Target");

            ValidatePropertyType(source, typeof(ListOfInt));
            ValidatePropertyType(Target, typeof(ListOfInt));

            if (GroupBy != null)
                ValidatePropertyType(GroupBy, typeof(Int32));

            var sourceValue = context.ValueProvider.GetValue(source);
            var targetValue = context.ValueProvider.GetValue(Target);

            if (source == null || targetValue == null)
            {
                // поле не заполнено.
                context.Messages.Add("Не все поля формы заполнены корректно.");
                return false;
            }

            return true;
        }

        protected override bool Validate(SqlConnection connection, ValidationConditionContext context)
        {
            // do the job

            // получить Id полей, контентов
            // получить типы полей
            // поля source, target долны быть типа M2M или M2O (если не так - бросаем исключение)
            // поля groupby, долно O2M (если не так - бросаем исключение)

            // делаем запрос к БД, результат которого - пересечение id связанных статей (с одним значением группировочного поля)
            // выводим список пересекающихся статей в виде общего сообщения валидации
            
            throw new NotImplementedException();
        }

        private static void ValidatePropertyType(PropertyDefinition source, Type expectedType)
        {
            if (source.PropertyType != expectedType)
            {
                throw new InvalidOperationException(string.Format("The type of the property '{0}' should be an integer.", source.PropertyName));
            }
        }

    }
}
