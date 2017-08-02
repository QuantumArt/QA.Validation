using System.Collections.Generic;
using System.Data.SqlClient;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Параметры валидации
    /// </summary>
    public class ValidationParamObject
    {
        /// <summary>
        /// Словарь имя поля - текстовое значение. Массивы string[] и int[] представлять как "12, 23, 12"
        /// </summary>
        public Dictionary<string, string> Model { get; set; }

        /// <summary>
        /// Основной валидатор контента
        /// </summary>
        public string Validator { get; set; }

        /// <summary>
        /// >Валидаторы для агрегированных статей
        /// </summary>
        public string[] AggregatedValidatorList { get; set; }

        /// <summary>
        /// Ресурсный словарь
        /// </summary>
        public string DynamicResource { get; set; }

        /// <summary>
        /// текущее подключение к БД
        /// </summary>
        public SqlConnection Connection { get; set; }

        /// <summary>
        /// Кастомер-код
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// Id сайта
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Id контента
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ValidationParamObject()
        {
            AggregatedValidatorList = new string[] { };
        }
    }
}