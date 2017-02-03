using System;
using System.Collections.Generic;
using System.IO;

namespace QA.Validation.Xaml
{
    /// <summary>
    /// Базовый класс для условий валидации, которые можут быть привязаны к какому-либо полю модели
    /// </summary>
    public abstract class PropertyValidationCondition : ValidationCondition
    {
        /// <summary>
        /// Поле, для которого проверяется условие валидации (его значение используют валидаторы)
        /// Если выбрано условие ForMember, то данное поле указывать необязательно
        /// </summary>
        public PropertyDefinition Source { get; set; }
    }
}
