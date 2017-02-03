using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QA.Validation.Xaml.Extensions.Rules.Remote
{
    public class RemotePropertyDefinition
    {
        /// <summary>
        /// Тип свойства
        /// </summary>
        public string PropertyType
        {
            get;
            set;
        }

        /// <summary>
        /// Название свойства.
        /// По названию осуществляется поиск значения при  валидации
        /// </summary>
        public string PropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// Уникальное дружественное имя данного поля.
        /// Используется в качестве ключа в Xaml при применении DefinitionMarkupExtension
        /// </summary>
        public string Alias
        {
            get;
            set;
        }

        /// <summary>
        /// Текстовый комментарий.
        /// </summary>
        public string Description
        {
            get;
            set;
        }
    }
}
